using BTurk.Automation.Core.Queries;

namespace BTurk.Automation.Standard.SecurityServices;

public class IsMasterPasswordValidQuery : IQuery<bool>
{
    public IsMasterPasswordValidQuery(string password)
    {
        Password = password;
    }

    public string Password { get; }
}