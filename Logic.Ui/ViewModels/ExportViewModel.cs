using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
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
    public class ExportViewModel
    {

        public RelayCommand ExportData { get; }

        public ExportViewModel()
        {
            ExportData = new RelayCommand(() => ExportDataMethod());
        }

        private void ExportDataMethod()
        {

            BoxCollectionViewModel bcvm = new BoxCollectionViewModel(); //Darf hier nicht erstellt werden, sondern muss irgendwo zentral erstellt und abgegriffen werden
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
            xmlWriter.WriteComment(klassenname);
            xmlWriter.WriteStartElement("Cards");
            // Kann nur auskommentiert werden, wenn auch alles andere steht
            /*
            foreach(BoxViewModel box in bcvm)
            {
                foreach(CardViewModel card in box)
                {
                    if (card.Category.equals(klassenname))
                    {
                        xmlWriter.WriteStartElement("Karte");
                        xmlWriter.WriteElementString("Question",card.Question);
                        xmlWriter.WriteElementString("Answer", card.Answer);
                        if(card.AnswerPic != "")
                        {
                            string picNew = @"\src\" + klassenname + "_img_" + picCount++;
                            xmlWriter.WriteElementString("AnswerPic",picNew);
                            File.Copy(card.AnswerPic, picNew);
                        }
                        if (card.QuestionPic != "")
                        {
                            string picNew = @"\src\" + klassenname + "_img_" + picCount++;
                            xmlWriter.WriteElementString("QuestionPic", picNew);
                            File.Copy(card.QuestionPic, picNew);
                        }
                        xmlWriter.WriteEndElement();

                    }
                }
            }
            */
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush(); // Muss noch mal genau gelesen und beschrieben werden
            xmlWriter.Close();

        }
    }
}
