using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    public class ImportViewModel
    {
        public RelayCommand ImportMethod { get; }
        public string File { get; set; }
        public ImportViewModel()
        {
            this.File = "Test";
            File = "Test3";
            ImportMethod = new RelayCommand(() => ImportFileMethod());

        }

        public void ImportFileMethod()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML-Files|*.xml";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show(ofd.FileName);
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(ofd.FileName);
            foreach(XmlNode node in doc)
            {
                foreach(XmlNode child in node)
                {
                    Console.WriteLine(child.Name);
                    Console.WriteLine(child.InnerText);
                }
            }
        }

    }

    
}
