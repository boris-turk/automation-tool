using System.Collections.Generic;
using System.IO;
using System.Linq;
using BTurk.Automation.Core.Requests;

// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

namespace BTurk.Automation.Standard
{
    public class NotesProvider : IRequestsProvider<Note>
    {
        public virtual IEnumerable<Note> Load()
        {
            var directory = @"C:\Users\boris\Dropbox\Automation\notes";

            foreach (var file in new DirectoryInfo(directory).GetFiles("*.txt"))
            {
                var text = GetText(file.Name);

                yield return new Note
                {
                    Text = text,
                    Path = file.FullName
                };
            }
        }

        private string GetText(string fileName)
        {
            var parts =
                from part in fileName.Split('_')
                where !part.ToLower().Contains("note")
                select part;

            return string.Join(" ", parts);
        }
    }
}