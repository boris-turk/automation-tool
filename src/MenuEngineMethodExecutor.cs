using System.Collections.Generic;
using System.Linq;

namespace AutomationEngine
{
    public class MenuEngineMethodExecutor
    {
        private readonly MainForm _mainForm;
        private readonly List<string> _arguments;

        public MenuEngineMethodExecutor(MainForm mainForm, IEnumerable<string> arguments)
        {
            _mainForm = mainForm;
            _arguments = arguments.ToList();
        }

        public void Execute()
        {
            _mainForm.Close();

            if (_arguments.Count == 0)
            {
                return;
            }

            if (_arguments[0] == "add_file_item")
            {
                ExecuteAddFileItem();
            }
        }

        private void ExecuteAddFileItem()
        {
            FormFactory.Instance<AddFileItemForm>().Show();
        }
    }
}