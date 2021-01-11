using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
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

        public readonly string saveDirectory = @"..\..\..\Lernkarten";

        public Boolean inclStat { get; set; }

        public Category Class { get; set; }   // Class ist die ausgewählte Kategory, oder ist es ein CategoryVM Objekt? Muss noch in ein VM geändert werden....

        public RelayCommand ExportData { get; }

        public ExportViewModel()
        {
            ExportData = new RelayCommand(() => ExportDataMethod());
        }

        /*
         * Diese Datei Exportiert eine von dem Nutzer ausgewählte Kategorie an eine beliebige Stelle. Dabei werden keine Statistikobjekte mit exportiert, sondern nur die sozusagen nie 
         * benutzten Karten. 
         */
        private void ExportDataMethod()
        {
            //BoxCollectionViewModel bcvm = new BoxCollectionViewModel(); //Darf hier nicht erstellt werden, sondern muss irgendwo zentral erstellt und abgegriffen werden
            FolderBrowserDialog fbd = new FolderBrowserDialog();    // Zum Ort auswählen, an dem der Ordner mit dem Export erstellt werden soll
            fbd.Description = "Bitte den Ort wählen, an dem der Export-Ordner erstellt werden soll";
            if(fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                MessageBox.Show(fbd.SelectedPath);
            string filename = Class.Name;    // Der Klassenname für die Datei wird aus der Auswahl getroffen.
            System.IO.Directory.CreateDirectory(fbd.SelectedPath + @"\Export"); // Der Speicherordner wird an der ausgewählten Stelle geschrieben
            System.IO.Directory.CreateDirectory(fbd.SelectedPath + @"\Export\content"); // Der Speicherordner für die Bilder wird geschrieben
            string filepath = fbd.SelectedPath + @"\Export\" + filename + ".xml";   // Speicherort mit Name wird für die Datei erstellt
            int picCount = 1;   // laufende Variable für die Fotobenennung.
            
            XmlTextWriter xmlWriter = new XmlTextWriter(filepath, System.Text.Encoding.UTF8);

            BoxViewModel currentBox = new BoxViewModel();
            XmlDocument doc = new XmlDocument();
            doc.Load(saveDirectory + @"\" + filename + ".xml");
            foreach (XmlNode node in doc.DocumentElement)
            {
                currentBox.Enqueue(ImportViewModel.readOwnFormatNode(node));   // Jede Karte wird in Form von einer XmlNode eingelesen, zu einer Karte gemacht und zurück gegeben
            }

            xmlWriter.Formatting = Formatting.Indented; //Noch mal nachforschen, was es tut
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteComment(filename);
            xmlWriter.WriteName(filename);
            xmlWriter.WriteStartElement("Cards");
                foreach (CardViewModel card in currentBox)
                {
                    xmlWriter.WriteStartElement("Card");
                        xmlWriter.WriteElementString("Question",card.Question);
                        xmlWriter.WriteElementString("Answer", card.Answer);
                        xmlWriter.WriteElementString("Category", filename);
                        if (card.QuestionPic != "")
                            {
                                string picNew = filename + "_img_" + picCount++; //Name der Bilder wird noch allgemein festgelegt
                                xmlWriter.WriteElementString("QuestionPic", picNew);
                                File.Copy(card.QuestionPic, fbd.SelectedPath +  @"\content\" + picNew);
                            }

                        if(card.AnswerPic != "")
                        {
                            string picNew = filename + "_img_" + picCount++; //Name der Bilder wird noch allgemein festgelegt
                            xmlWriter.WriteElementString("AnswerPic",picNew);
                            File.Copy(card.AnswerPic, fbd.SelectedPath + @"\content\" + picNew);
                        } 
                    xmlWriter.WriteEndElement();
                }
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush(); // Muss noch mal genau gelesen und beschrieben werden
            xmlWriter.Close();
            MessageBox.Show("Dateien wurden exportiert");

        }
    }
}
