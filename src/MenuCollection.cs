using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ahk
{
    public class MenuCollection
    {
        private const string tortoise_proc = @"c:\Program Files\TortoiseSVN\bin\TortoiseProc.exe";

        public List<Menu> Menus { get; }

        public MenuCollection()
        {
            Menus = new List<Menu>();
            Initialize();
        }

        private void Initialize()
        {
            Menus.Add(CreateProgramMenu());
            Menus.Add(CreateFileMenu());
            Menus.Add(CreateUrlMenu());
            Menus.Add(CreateSolutionMenu());
            Menus.Add(CreateRepositoryUpdateMenu());
        }

        private Menu CreateRepositoryUpdateMenu()
        {
            var menu = new ExecutableMenu
            {
                Name = "update",
                Execute = x => SvnUpdate(x.Parameter)
            };
            menu.LoadItemsFromFile(Configuration.RepositoriesPath);

            return menu;
        }

        private void SvnUpdate(string parameter)
        {
            string program = tortoise_proc;
            string arguments = "/command:update /path:" + parameter;
            System.Diagnostics.Process.Start(program, arguments);
        }

        private Menu CreateSolutionMenu()
        {
            var menu = new ExecutableMenu
            {
                Name = "solution",
                Execute = x => System.Diagnostics.Process.Start(x.Parameter),
            };
            menu.LoadItemsFromFile(Configuration.SolutionsPath);

            return menu;
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

        public IEnumerable<Menu> GetOrderedMenuItems()
        {
            return Menus.OrderBy(x => x, new MenuItemComparator());
        }
    }
}