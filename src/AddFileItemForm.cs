using System.Linq;
using System.Windows.Forms;

namespace AutomationEngine
{
    public partial class AddFileItemForm : AutomationEngineForm
    {
        public AddFileItemForm()
        {
            InitializeComponent();
        }

        protected override string WindowName
        {
            get { return "add file item"; }
        }

        protected override void OnExecute()
        {
        }
    }
}
