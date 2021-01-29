using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using System;
using System.IO;
using System.Windows;
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
        public RelayCommand CloseWindow { get; }
        public ExportViewModel(CategoryCollectionViewModel categorys)
        {
            ExportData = new RelayCommand(() => ExportDataMethod());
            MyModelViewModel = categorys;
            CloseWindow = new RelayCommand(param => Close(param));
        }
        private int picCount = 1;

        private void Close(object param)
        {
            Window window = (Window)param;
            window.Close();
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
                    // A xmlWriter to write the nodes
                    XmlTextWriter xmlWriter = new XmlTextWriter(
                        filepath, System.Text.Encoding.UTF8);
                    /* Try to load a file with the Categoryname. 
                    * If there is no file with the naming, the try doesn't 
                    * work and going to the cache
                    */ 
                    try
                    {
                        BoxViewModel currentBox = ReadSavedFile(filename);
                        // So the .xml File is more readable and every Element
                        //get an own Line and is intended
                        WriteXML(xmlWriter, filename, currentBox, fbd);
                    }
                    catch
                    {
                        System.Windows.MessageBox.Show(
                            "Leider konnte kein Export durchgeführt werden," +
                            " da entweder die Datei nicht gelesen werden" +
                            " konnte oder es noch gar keine" +
                            " Karten zum exportieren gibt.");
                    }
                } else
                {
                    System.Windows.MessageBox.Show("Es muss ein Zielpfad ausgewählt werden");
                }
            }
        }

        /*
         * This Method writes the BoxViewModel in an own designes Format 
         * with a XmlTextWriter to the FileSystem.
         */
        private void WriteXML(XmlTextWriter xmlWriter,
            string filename,
            BoxViewModel currentBox,
            FolderBrowserDialog fbd)
        {
            
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
                CopyCardPics(card, xmlWriter, filename, fbd);
                if (InclStat && card.StatisticCollection != null)
                {
                    WriteStatistic(card, xmlWriter);
                }
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
            }
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            xmlWriter.Close();
        }

        /*
         * This Method receives a Card, the XmlTextWriter, 
         * the Filename (Category) and the FolderBrowserDialog and copy the 
         * Pictures if necessery.
         */
        private void CopyCardPics(CardViewModel card,
            XmlTextWriter xmlWriter,
            string filename,
            FolderBrowserDialog fbd)
        {
            if(card.QuestionPic != null && card.QuestionPic != "")
            {
                CopyPic(filename, fbd, card.QuestionPic, xmlWriter);
            }
            if (card.AnswerPic != null && card.AnswerPic != "")
            {
                CopyPic(filename, fbd, card.AnswerPic, xmlWriter);
            }

        }

        /*
         * This Method copy Copy the Card and writes the xmlNode
         */
        private void CopyPic(string filename,
            FolderBrowserDialog fbd, string file, XmlTextWriter xmlWriter)
        {
            string picNew = filename + "_img_" + picCount + ".jpg";
            picCount++;
            string pathSavePicture = fbd.SelectedPath.ToString() +
                @"\Export\content\" + picNew;
            File.Copy(savePicDirectory + file, pathSavePicture);
            xmlWriter.WriteElementString("QuestionPic", picNew);
        }

        /*
         * This Method receives the CardViewModel and the XmlTextWriter and 
         * write the StatisticNodes if necessery
         */
        private void WriteStatistic(CardViewModel card, XmlTextWriter xmlWriter)
        {
            xmlWriter.WriteStartElement(
                        "StatisticCollection");
            if (card.StatisticCollection != null)
            {
                foreach (Statistic stat in card.StatisticCollection)
                {
                    xmlWriter.WriteStartElement("Statistic");
                    xmlWriter.WriteElementString("Timestamp",
                        stat.Timestamp.ToString());
                    xmlWriter.WriteElementString("SuccessfullAnswer",
                        stat.SuccessfullAnswer.ToString());
                    xmlWriter.WriteElementString("CurrentBoxNumber",
                        stat.CurrentBoxNumber.ToString());
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();
                }
            }
            xmlWriter.WriteEndElement();
        }

        /*
         * This Method receives the filename (Category) and try to read a file
         * of the Category. If there is a File, the Method Adds the loaded 
         * Cards to the BoxViewModel. If there is no Card, the BoxViewModel
         * stays empty. The Method returns the BoxViewModel.
         */
        private BoxViewModel ReadSavedFile(string filename)
        {
            BoxViewModel currentBox = new BoxViewModel();
            XmlDocument doc = new XmlDocument();
            // load the nodes from the categoryname.xml file
            doc.Load(saveDirectory + @"\" + filename + ".xml");
            // Every node is going to be a Card 
            foreach (XmlNode node in doc.DocumentElement)
            {
                currentBox.Enqueue(
                    ImportViewModel.ReadOwnFormatNode(node));
            }         
            return currentBox;
        }
    }
}
