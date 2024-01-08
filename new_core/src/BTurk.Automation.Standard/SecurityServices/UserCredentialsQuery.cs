using BTurk.Automation.Core.Queries;

namespace BTurk.Automation.Standard.SecurityServices;

public class UserCredentialsQuery : IQuery<UserCredentials>
{
    public UserCredentialsQuery(string groupName, string entryTitle)
    {
        GroupName = groupName;
        EntryTitle = entryTitle;
    }

    public string GroupName { get; }

    public string EntryTitle { get; }
}