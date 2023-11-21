namespace BTurk.Automation.Core.FileSystem;

public interface IDirectoryProvider
{
    string GetDirectory(DirectoryParameters parameters);
}