using System.Windows.Forms;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core
{
    internal partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public ISearchHandler SearchHandler { get; set; }
    }
}
