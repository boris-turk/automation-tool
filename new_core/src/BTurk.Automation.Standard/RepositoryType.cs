using System.Runtime.Serialization;

namespace BTurk.Automation.Standard
{
    public enum RepositoryType
    {
        [EnumMember(Value = "Svn")]
        Svn,

        [EnumMember(Value = "Git")]
        Git
    }
}