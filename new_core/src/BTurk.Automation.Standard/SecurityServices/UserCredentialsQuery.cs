using BTurk.Automation.Core.Queries;

// ReSharper disable InconsistentNaming

namespace BTurk.Automation.Standard.SecurityServices;

public class UserCredentialsQuery : IQuery<UserCredentials>
{
    private UserCredentialsQuery(string tag)
    {
        Tag = tag;
    }

    public string Tag { get; }

    public static UserCredentialsQuery BorisISL => new("BorisISL");
}