using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    public class ExportViewModel
    {

        public RelayCommand ExportData { get; }

        public ExportViewModel()
        {
            ExportData = new RelayCommand(() => ExportDataMethod());
        }

        private void ExportDataMethod()
        {

            BoxCollectionViewModel bcvm = new BoxCollectionViewModel();
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Bitte den Ort wählen, an dem der Export-Ordner erstellt werden soll";
            if(fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                MessageBox.Show(fbd.SelectedPath);
            string klassenname = "Klassenname"; //Muss noch in den echten Klassenname geändert werden, wenn ich drauf zugreifen kann
            string filename = fbd.SelectedPath + @"\" + klassenname + ".xml";
            int picCount = 1;
            System.IO.Directory.CreateDirectory(fbd.SelectedPath + @"\Export");
            XmlTextWriter xmlWriter = new XmlTextWriter(filename, System.Text.Encoding.UTF8);

            xmlWriter.Formatting = Formatting.Indented; //Noch mal nachforschen, was es tut
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteComment("Hier sind alle exportierten Karten der Klasse " + klassenname);
            xmlWriter.WriteStartElement("Cards");
            foreach(BoxViewModel box in bcvm)
            {
                foreach(CardViewModel card in box)
                {
                    if (card.category.equals(klassenname))
                    {
                        xmlWriter.WriteStartElement("Karte");
                        xmlWriter.WriteElementString("Question",card.Question);
                        xmlWriter.WriteElementString("Answer", card.Answer);
                        if(card.AnswerPic != "")
                        {
                            xmlWriter.WriteElementString("AnswerPic",@"\src\" + klassenname + "_img_" + picCount++);
                        }
                        if (card.QuestionPic != "")
                        {
                            xmlWriter.WriteElementString("QuestionPic", @"\src\" + klassenname + "_img_" + picCount++);
                        }
                        xmlWriter.WriteEndElement();

                    }
                }
            }
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush(); // Muss noch mal genau gelesen und beschrieben werden
            xmlWriter.Close();

        }
    }
}
