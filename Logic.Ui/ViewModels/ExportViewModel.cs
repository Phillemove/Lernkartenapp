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
        public readonly string saveDirectory = 
            @"..\..\..\Lernkarten";
        public readonly string savePicDirectory = 
            @"..\..\..\Lernkarten\content\";

        public CategoryCollectionViewModel MyModelViewModel { get; set; }
        // The CheckBoxStatus to choose betwen export 
        //without Statistics and export with Statistics
        public Boolean InclStat { get; set; } 
        public CategoryViewModel Class { get; set; } 
        public RelayCommand ExportData { get; }
        public ExportViewModel(CategoryCollectionViewModel categorys)
        {
            ExportData = new RelayCommand(() => ExportDataMethod());
            MyModelViewModel = categorys;
        }

        /*
         * This Method exports all cards of the user choosen category.
         * The Place is free to choose and the user has the 
         * choise to export the statistic Objects or not
         */
        private void ExportDataMethod()
        {
            if(Class != null)   // If a Category is choosen
            {
                // new FolderBrwoserDialog to choose the export filepath
                FolderBrowserDialog fbd = new FolderBrowserDialog();    
                fbd.Description = "Bitte den Ort wählen," +
                    " an dem der Export-Ordner erstellt werden soll";
                // If the filepath is okay
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
                {
                    // The Categoryname is choosen from the choosen 
                    // Entry of the ComboBox
                    string filename = Class.Name;
                    // The Folder are going to be created if not created
                    System.IO.Directory.CreateDirectory(
                        fbd.SelectedPath + @"\Export");
                    // The Folder for the images
                    System.IO.Directory.CreateDirectory(
                        fbd.SelectedPath + @"\Export\content");
                    // declare a variable for the to be exported
                    // file with filename (Categoryname)
                    string filepath = fbd.SelectedPath +
                        @"\Export\" + filename + ".xml";   
                    int picCount = 1;   // for the running imagenaming
                    // A xmlWriter to write the nodes
                    XmlTextWriter xmlWriter = new XmlTextWriter(
                        filepath, System.Text.Encoding.UTF8);
                    /* Try to load a file with the Categoryname. 
                    * If there is no file with the naming, the try doesn't 
                    * work and going to the cache
                    */ 
                    try
                    {
                        BoxViewModel currentBox = new BoxViewModel();
                        XmlDocument doc = new XmlDocument();
                        // load the nodes from the categoryname.xml file
                        doc.Load(saveDirectory + @"\" + filename + ".xml");
                        // Every node is going to be a Card 
                        foreach (XmlNode node in doc.DocumentElement)   
                        {
                            currentBox.Enqueue(ImportViewModel.ReadOwnFormatNode(node)); 
                        }
                        // So the .xml File is more readable and every Element
                        //get an own Line and is intended
                        xmlWriter.Formatting = Formatting.Indented; 
                        xmlWriter.WriteStartDocument();
                        xmlWriter.WriteComment(filename);
                        xmlWriter.WriteStartElement("Cards");
                        foreach (CardViewModel card in currentBox)
                        {
                            xmlWriter.WriteStartElement("Card");
                            xmlWriter.WriteElementString(
                                "Question", card.Question);
                            xmlWriter.WriteElementString(
                                "Answer", card.Answer);
                            if (card.QuestionPic != null && 
                                card.QuestionPic != "")
                            {
                                string picNew = filename + "_img_" +
                                    picCount + ".jpg";
                                picCount++;
                                string pathSavePicture = 
                                    fbd.SelectedPath.ToString() +
                                    @"\Export\content\" + picNew;
                                File.Copy(savePicDirectory + 
                                    card.QuestionPic, pathSavePicture);
                                xmlWriter.WriteElementString(
                                    "QuestionPic", picNew);
                            }

                            if (card.AnswerPic != null &&
                                card.AnswerPic != "")
                            {
                                string picNew = filename + "_img_" +
                                    picCount + ".jpg";
                                picCount++;
                                string pathSavePicture = 
                                    fbd.SelectedPath.ToString() +
                                    @"\Export\content\" + picNew;
                                File.Copy(savePicDirectory +
                                    card.AnswerPic, pathSavePicture);
                                xmlWriter.WriteElementString("AnswerPic",
                                    picNew);
                            }
                            if (InclStat && card.StatisticCollection != null)
                            {
                                xmlWriter.WriteStartElement(
                                    "StatisticCollection");
                                if (card.StatisticCollection != null)
                                {
                                    foreach (Statistic stat in
                                        card.StatisticCollection)
                                    {
                                        xmlWriter.WriteStartElement("Statistic");
                                            xmlWriter.WriteElementString(
                                                "Timestamp",
                                                stat.Timestamp.ToString());
                                            xmlWriter.WriteElementString(
                                                "SuccessfullAnswer",
                                                stat.SuccessfullAnswer.ToString());
                                            xmlWriter.WriteElementString(
                                                "CurrentBoxNumber",
                                                stat.CurrentBoxNumber.ToString());
                                        xmlWriter.WriteEndElement();
                                        xmlWriter.Flush();
                                    }
                                }
                                xmlWriter.WriteEndElement();
                            }
                            xmlWriter.WriteEndElement();
                            xmlWriter.Flush();
                        }
                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndDocument();
                        xmlWriter.Flush();
                        xmlWriter.Close();
                    }
                    catch
                    {
                        MessageBox.Show(
                            "Leider konnte kein Export durchgeführt werden," +
                            " da entweder die Datei nicht gelesen werden" +
                            " konnte oder es noch gar keine" +
                            " Karten zum exportieren gibt.");
                    }
                } else
                {
                    MessageBox.Show("Es muss ein Zielpfad ausgewählt werden");
                }
            }
        }
    }
}
