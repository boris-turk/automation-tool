namespace BTurk.Automation.Core.FileSystem;

public class FileParameters
{
    public FileParameters(string absoluteFilePath)
    {
        DirectoryParameters = DirectoryParameters.None;
        FileName = absoluteFilePath;
    }

    public FileParameters(DirectoryParameters directoryParameters, string fileName)
    {
        DirectoryParameters = directoryParameters;
        FileName = fileName;
    }

    public string FileName { get; }

    public DirectoryParameters DirectoryParameters { get; }
}