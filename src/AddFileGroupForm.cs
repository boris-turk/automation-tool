using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AutomationEngine
{
    public partial class AddFileGroupForm : AutomationEngineForm
    {
        public AddFileGroupForm()
        {
            InitializeComponent();
            _menuFileName.AutoFillFrom(_id);
            _directory.AutoFillFrom(_menuFileName);
        }

        private List<FileGroup> FileGroups
        {
            get { return FileGroupCollection.Instance.FileGroups; }
        }

        public string Id
        {
            get { return _id.Text; }
            set { _id.Text = value; }
        }

        private string Directory
        {
            get { return _directory.Text; }
        }

        private string MenuFileName
        {
            get
            {
                string fileName = _menuFileName.Text;

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    return fileName;
                }

                if (!fileName.ToLower().EndsWith(".xml"))
                {
                    fileName += ".xml";
                }

                return fileName;
            }
        }

        protected override void OnExecute()
        {
            base.OnExecute();

            var fileGroup = new FileGroup
            {
                Id = Id,
                Directory = Directory,
                MenuFileName = MenuFileName
            };

            FileGroupCollection.Instance.AddFileGroup(fileGroup);
            FileGroupCollection.Instance.Save();
        }

        protected override string ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                return "Id not specified";
            }
            if (string.IsNullOrWhiteSpace(Directory))
            {
                return "Directory not specified";
            }
            if (string.IsNullOrWhiteSpace(MenuFileName))
            {
                return "Menu file name not specified";
            }
            if (FileGroups.Any(x => x.Id == Id))
            {
                return "Id already exists";
            }
            if (FileGroups.Any(x => x.Directory == Directory))
            {
                return "Directory already exists";
            }
            if (FileGroups.Any(x => x.MenuFileName == MenuFileName))
            {
                return "Menu file name already exists";
            }
            return null;
        }
    }
}
