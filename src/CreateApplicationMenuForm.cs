using System.IO;

namespace AutomationEngine
{
    public partial class CreateApplicationMenuForm : AutomationEngineForm
    {
        public CreateApplicationMenuForm()
        {
            InitializeComponent();
        }

        public string ContextRegex
        {
            get { return _contextRegex.Text; }
            set { _contextRegex.Text = value; }
        }

        protected override string ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(ContextRegex))
            {
                return "Context regex not specified";
            }
            if (string.IsNullOrWhiteSpace(_menuFileName.Text))
            {
                return "Menu file name not specified";
            }
            return null;
        }

        protected override void OnExecute()
        {
            string directory = Configuration.Instance.ApplicationMenuDirectory;

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var applicationMenu = new ApplicationMenuFileContext
            {
                MenuFileName = _menuFileName.Text,
                ContextRegex = ContextRegex
            };

            ApplicationMenuCollection.Instance.Menus.Add(applicationMenu);
            ApplicationMenuCollection.Instance.Save();
        }
    }
}
