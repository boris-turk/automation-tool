using BTurk.Automation.Core.Annotations;
using BTurk.Automation.Core.Configuration;
using BTurk.Automation.Core.Queries;
using BTurk.Automation.Standard;
using KeePassLib;
using KeePassLib.Interfaces;
using KeePassLib.Keys;
using KeePassLib.Serialization;

namespace BTurk.Automation.DependencyResolution.KeePassInterop;

[IgnoreUnusedTypeWarning<IsMasterPasswordValidQueryHandler>]
public class IsMasterPasswordValidQueryHandler : IQueryHandler<IsMasterPasswordValidQuery, bool>
{
    public IsMasterPasswordValidQueryHandler(IConfigurationProvider configurationProvider)
    {
        ConfigurationProvider = configurationProvider;
    }

    private IConfigurationProvider ConfigurationProvider { get; }

    public bool Handle(IsMasterPasswordValidQuery queryData)
    {
        var databasePath = ConfigurationProvider.Configuration.GetKeePassDatabase();

        try
        {
            var ioConnectionInfo = new IOConnectionInfo { Path = databasePath };
            var compositeKey = new CompositeKey();

            var enteredKey = new KcpPassword(queryData.Password);
            compositeKey.AddUserKey(enteredKey);

            var db = new PwDatabase();

            db.Open(ioConnectionInfo, compositeKey, new NullStatusLogger());
            db.Close();

            return true;
        }
        catch (InvalidCompositeKeyException)
        {
            return false;
        }
    }
}