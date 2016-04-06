using System;
using System.IO;
using System.Linq;

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

        protected override string WindowName
        {
            get { return "add file item"; }
        }

        private AddFileGroupForm AddFileGroupForm
        {
            get { return FormFactory.Instance<AddFileGroupForm>(); }
        }

        private FileGroup Group
        {
            get { return FileGroupCollection.Instance.GetGroupById(GroupId); }
        }

        private string ContentsFileName { get; set; }

        private string ContentsFilePath
        {
            get { return Path.Combine(Group.Directory, ContentsFileName); }
        }

        private string GroupId
        {
            get { return _group.Text; }
        }

        private string ItemName
        {
            get { return _name.Text; }
        }

        private string Context
        {
            get { return _context.Text; }
        }

        private string Value
        {
            get { return _value.Text; }
        }

        private FileItem FileItem
        {
            get
            {
                var fileItem = new FileItem
                {
                    Name = ItemName,
                    Context = Context,
                };

                fileItem.Arguments.Add(new StringValue { Value = ContentsFileName });

                return fileItem;
            }
        }

        protected override string ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(GroupId))
            {
                return "Group not specified";
            }
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

            AddFileGroupIfNecessary();
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
            string context = Context.ToLower();

            if (ContextCollection.Instance.Contexts.All(x => x != context))
            {
                ContextCollection.Instance.Contexts.Add(context);
                ContextCollection.Instance.Save();
            }
        }

        private void SaveFileItem()
        {
            string menuFileName = Group.MenuFileName;
            Menu menu = AutomationEngine.Menu.LoadFromFile(menuFileName);
            menu.GroupId = GroupId;
            menu.Items.Add(FileItem);
            menu.SaveToFile();
        }

        private void SaveContentsFile()
        {
            if (!Directory.Exists(Group.Directory))
            {
                Directory.CreateDirectory(Group.Directory);
            }
            File.WriteAllText(ContentsFilePath, Value);
        }

        private void AddFileGroupIfNecessary()
        {
            if (Group == null)
            {
                AddFileGroup();
            }
        }

        private void AddFileGroup()
        {
            AddFileGroupForm.Id = GroupId;
            AddFileGroupForm.ShowDialog(this);
            if (!AddFileGroupForm.Executed)
            {
                ExecutionCanceled = true;
            }
        }

        private void OnMadeVisible()
        {
            ContentsFileName = Guid.NewGuid().ToString().ToLower() + ".txt";
            FileGroupCollection.Instance.FileGroups.ForEach(x => _group.Items.Add(x.Id));
            ContextCollection.Instance.Contexts.ForEach(x => _context.Items.Add(x));
        }
    }
}
