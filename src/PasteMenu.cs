using System.Collections.Generic;

namespace Ahk
{
    public class PasteMenu : Menu
    {
        public PasteMenu()
        {
            Name = "paste";
        }

        protected override IEnumerable<Menu> GetSubmenuItems()
        {
            yield return CreateFileMenu();
            yield return CreateProgramMenu();
            yield return CreateUrlMenu();
        }

        private Menu CreateUrlMenu()
        {
            var menu = new ExecutableMenu
            {
                Name = "url",
                Execute = x => System.Diagnostics.Process.Start(x.Parameter),
            };
            menu.LoadItemsFromFile(Configuration.UrlsPath);

            return menu;
        }

        private Menu CreateProgramMenu()
        {
            var menu = new ExecutableMenu
            {
                Name = "program",
                Execute = x => System.Diagnostics.Process.Start(x.Parameter),
            };
            menu.LoadItemsFromFile(Configuration.ProgramsPath);

            return menu;
        }

        private Menu CreateFileMenu()
        {
            var menu = new ExecutableMenu
            {
                Name = "file",
                Execute = x => System.Diagnostics.Process.Start(x.Parameter),
            };
            menu.LoadItemsFromFile(Configuration.FilesPath);

            return menu;
        }
    }
}