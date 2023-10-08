using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace BTurk.Automation.Core.Helpers;

public class DirectoryIterator : IEnumerable<DirectoryScope>
{
    public DirectoryIterator(params string[] rootDirectories)
    {
        RootDirectories = rootDirectories;
    }

    public string[] RootDirectories { get; }

    public Predicate<DirectoryScope> CanVisit { get; set; }

    public IEnumerator<DirectoryScope> GetEnumerator()
    {
        foreach (var rootDirectory in RootDirectories)
        {
            foreach (var subDirectory in GetSubDirectories(rootDirectory))
                yield return subDirectory;
        }
    }

    private IEnumerable<DirectoryScope> GetSubDirectories(string rootDirectory)
    {
        foreach (var directory in Directory.GetDirectories(rootDirectory))
        {
            var directoryScope = new DirectoryScope(directory);

            yield return directoryScope;

            if (CanVisit != null && !CanVisit(directoryScope))
                continue;

            foreach (var subDirectoryScope in GetSubDirectories(directory))
                yield return subDirectoryScope;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<DirectoryScope>)this).GetEnumerator();
}