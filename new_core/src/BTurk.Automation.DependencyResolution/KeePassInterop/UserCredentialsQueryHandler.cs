using BTurk.Automation.Core.Annotations;
using BTurk.Automation.Core.Configuration;
using BTurk.Automation.Core.Credentials;
using BTurk.Automation.Core.Queries;
using BTurk.Automation.Standard.SecurityServices;
using KeePassLib;
using KeePassLib.Interfaces;
using KeePassLib.Keys;
using KeePassLib.Serialization;

namespace BTurk.Automation.DependencyResolution.KeePassInterop;

[IgnoreUnusedTypeWarning<UserCredentialsQueryHandler>]
public class UserCredentialsQueryHandler : IQueryHandler<UserCredentialsQuery, UserCredentials>
{
    public UserCredentialsQueryHandler(IConfigurationProvider configurationProvider)
    {
        ConfigurationProvider = configurationProvider;
    }

    private IConfigurationProvider ConfigurationProvider { get; }

    public UserCredentials Handle(UserCredentialsQuery queryData)
    {
        var masterPassword = SecurePasswordStorage.RetrievePassword();
        var databasePath = ConfigurationProvider.Configuration.GetKeePassDatabase();

        PwDatabase db = null;

        try
        {
            var ioConnectionInfo = new IOConnectionInfo {Path = databasePath};
            var compositeKey = new CompositeKey();
            var enteredKey = new KcpPassword(masterPassword);
            compositeKey.AddUserKey(enteredKey);

            db = new PwDatabase();
            db.Open(ioConnectionInfo, compositeKey, new NullStatusLogger());

            var group = FindGroupByName(db.RootGroup, queryData.GroupName);

            if (group == null)
                return UserCredentials.Empty;

            var entry = FindEntryByTitle(group, queryData.EntryTitle);

            if (entry == null)
                return UserCredentials.Empty;

            var userName = entry.Strings.GetSafe("UserName").ReadString();
            var password = entry.Strings.GetSafe("Password").ReadString();

            return new UserCredentials(userName, password);
        }
        catch
        {
            return UserCredentials.Empty;
        }
        finally
        {
            db?.Close();
        }
    }

    private PwGroup FindGroupByName(PwGroup group, string groupName)
    {
        if (group.Name == groupName)
            return group;

        foreach (PwGroup subGroup in group.Groups)
        {
            var foundGroup = FindGroupByName(subGroup, groupName);

            if (foundGroup != null)
                return foundGroup;
        }

        return null;
    }

    private PwEntry FindEntryByTitle(PwGroup group, string entryTitle)
    {
        foreach (PwEntry entry in group.Entries)
        {
            if (entry.Strings.GetSafe("Title").ReadString() == entryTitle)
                return entry;
        }

        return null;
    }
}