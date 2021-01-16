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


        public String Class { get; set; }   // Class ist die ausgewählte Kategory, oder ist es ein CategoryVM Objekt? Muss noch in ein VM geändert werden....

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
            if(Class != null)   // Zur Überprüfung, ob die zu exportierende Kategorie ausgewählt wurde.
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();    // Zum Ort auswählen, an dem der Ordner mit dem Export erstellt werden soll
                fbd.Description = "Bitte den Ort wählen, an dem der Export-Ordner erstellt werden soll";
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string filename = Class;    // Der Klassenname für die Datei wird aus der Auswahl getroffen.
                    System.IO.Directory.CreateDirectory(fbd.SelectedPath + @"\Export"); // Der Speicherordner wird an der ausgewählten Stelle geschrieben
                    System.IO.Directory.CreateDirectory(fbd.SelectedPath + @"\Export\content"); // Der Speicherordner für die Bilder wird geschrieben
                    string filepath = fbd.SelectedPath + @"\Export\" + filename + ".xml";   // Speicherort mit Name wird für die Datei erstellt
                    int picCount = 1;   // laufende Variable für die Fotobenennung.

                    XmlTextWriter xmlWriter = new XmlTextWriter(filepath, System.Text.Encoding.UTF8);

                    try  // Versuch des ladens einer Kategoriedatei. Sollte nur die Kategorie ohne Karten angelegt worden sein, wird hier nichts geladen werden können und es wird ein Fehler ausgegeben.
                    {
                        BoxViewModel currentBox = new BoxViewModel();
                        XmlDocument doc = new XmlDocument();
                        doc.Load(saveDirectory + @"\" + filename + ".xml");
                        foreach (XmlNode node in doc.DocumentElement)
                        {
                            currentBox.Enqueue(ImportViewModel.readOwnFormatNode(node));   // Jede Karte wird in Form von einer XmlNode eingelesen, zu einer Karte gemacht und zurück gegeben
                        }
                        currentBox.Bn = Boxnumber.Box1;

                        xmlWriter.Formatting = Formatting.Indented; //Noch mal nachforschen, was es tut
                        xmlWriter.WriteStartDocument();
                        xmlWriter.WriteComment(filename);
                        //xmlWriter.WriteName(filename);
                        xmlWriter.WriteStartElement("Cards");
                        foreach (CardViewModel card in currentBox)
                        {
                            xmlWriter.WriteStartElement("Card");
                            xmlWriter.WriteElementString("Question", card.Question);
                            xmlWriter.WriteElementString("Answer", card.Answer);
                            xmlWriter.WriteElementString("Category", filename);
                            if (card.QuestionPic != null)
                            {
                                string picNew = filename + "_img_" + picCount + ".jpg"; //Name der Bilder wird noch allgemein festgelegt
                                picCount++;
                                xmlWriter.WriteElementString("QuestionPic", picNew);
                                string pathSavePicture = fbd.SelectedPath.ToString() + @"\Export\content\" + picNew;
                                File.Copy(@"..\..\..\Lernkarten\content\question.jpg", pathSavePicture);
                            }

                            if (card.AnswerPic != null)
                            {
                                string picNew = filename + "_img_" + picCount + ".jpg"; //Name der Bilder wird noch allgemein festgelegt
                                picCount++;
                                xmlWriter.WriteElementString("AnswerPic", picNew);
                                string pathSavePicture = fbd.SelectedPath.ToString() + @"\Export\content\" + picNew;
                                File.Copy(@"..\..\..\Lernkarten\content\answer.jpg", pathSavePicture);
                            }
                            xmlWriter.WriteElementString("Boxnumber", currentBox.Bn.ToString());
                            if (inclStat && card.StatisticCollection != null)
                            {
                                xmlWriter.WriteStartElement("StatisticCollection");
                                if (card.StatisticCollection != null)   // Wenn es an dieser Stelle noch keine StatisticCollection gibt, wird hier auch nichts rein geschrieben
                                {
                                    foreach (Statistic stat in card.StatisticCollection)
                                    {
                                        xmlWriter.WriteStartElement("Statistic");   // Beginn eines Statistic Blocks
                                        if(stat.Timestamp != null)
                                        {
                                            xmlWriter.WriteElementString("Timestamp", stat.Timestamp.ToString());
                                        }
                                        if (stat.SuccessfullAnswer)
                                        {
                                            xmlWriter.WriteElementString("SuccessfullAnswer", stat.SuccessfullAnswer.ToString());
                                        }
                                        if (stat.CurrentBoxNumber != Boxnumber.None)
                                        {
                                            xmlWriter.WriteElementString("CurrentBoxNumber", stat.CurrentBoxNumber.ToString());
                                        }
                                        xmlWriter.WriteEndElement(); // Ende eines Statistic Blocks
                                    }
                                }
                                xmlWriter.WriteEndElement(); // Ende der StatisticCollection
                            }
                            xmlWriter.WriteEndElement();
                        }
                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndDocument();
                        xmlWriter.Flush(); // Muss noch mal genau gelesen und beschrieben werden
                        xmlWriter.Close();
                        MessageBox.Show("Dateien wurden exportiert");
                    }
                    catch
                    {
                        MessageBox.Show("Leider konnte kein Export durchgeführt werden, da entweder die Datei nicht gelesen werden konnte oder es noch gar keine Karten zum exportieren gibt.");
                    }
                } else
                {
                    MessageBox.Show("Es muss ein Zielpfad ausgewählt werden");
                }
            }
            

        }


    }
}
