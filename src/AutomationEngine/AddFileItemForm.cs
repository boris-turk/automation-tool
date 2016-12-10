namespace AutomationEngine
{
    public partial class AddFileItemForm : AutomationEngineForm
    {
        public AddFileItemForm()
        {
            InitializeComponent();
            VisibleChanged += (sender, args) =>
            {
                if (Visible) OnMadeVisible();
            };
        }

        protected override string WindowName => "add file item";

        private string ItemName => _name.Text;

        private string Value => _value.Text;

        protected override string ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(ItemName))
            {
                return "Name not specified";
            }
            if (string.IsNullOrWhiteSpace(Value))
            {
                return "Value not specified";
            }
            return null;
        }

        protected override void OnExecute()
        {
            base.OnExecute();

            AddContextIfNecessary();

            if (ExecutionCanceled)
            {
                return;
            }

            SaveFileItem();
            SaveContentsFile();
        }

        private void AddContextIfNecessary()
        {
            //if (ContextCollection.Instance.Contexts.All(x => x.ToLower() != Context.ToLower()))
            //{
            //    ContextCollection.Instance.Contexts.Add(Context);
            //    ContextCollection.Instance.Save();
            //}
        }

        private void SaveFileItem()
        {
            //string menuFileName = Group.MenuFileName;
            //if (!menuFileName.ToLower().EndsWith(".xml"))
            //{
            //    menuFileName += ".xml";
            //}
            //Menu menu = AutomationEngine.Menu.LoadFromFile(menuFileName);
            //menu.GroupId = GroupId;
            //menu.Items.Add(FileItem);
            //menu.SaveToFile();
        }

        private void SaveContentsFile()
        {
            //if (!Directory.Exists(Group.Directory))
            //{
            //    Directory.CreateDirectory(Group.Directory);
            //}
            //File.WriteAllText(ContentsFilePath, Value);
        }

        private void OnMadeVisible()
        {
            //ContentsFileName = Guid.NewGuid().ToString().ToLower() + ".txt";

            //_group.Items.Clear();
            //FileGroupCollection.Instance.FileGroups.ForEach(x => _group.Items.Add(x.Id));

            //_context.Items.Clear();
            //ContextCollection.Instance.Contexts.ForEach(x => _context.Items.Add(x));
        }
    }
}
