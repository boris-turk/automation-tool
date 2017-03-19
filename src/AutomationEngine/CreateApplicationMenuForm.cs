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
        }
    }
}
