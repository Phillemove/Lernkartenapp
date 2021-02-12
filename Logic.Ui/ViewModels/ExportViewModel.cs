using De.HsFlensburg.ClientApp101.Logic.Ui.Support;
using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Xml;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    public class ExportViewModel
    {
        public readonly string saveDirectory =
            ModelViewModel.saveDirectory;
        public readonly string savePicDirectory =
            ModelViewModel.savePicDirectory;

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
        //private int picCount = 1;

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
                FolderBrowserDialog folderBD = new FolderBrowserDialog();
                folderBD.Description = "Bitte den Ort wählen," +
                    " an dem der Export-Ordner erstellt werden soll";
                // If the filepath is okay
                if (folderBD.ShowDialog() == 
                    System.Windows.Forms.DialogResult.OK) 
                {
                    // The Categoryname is choosen from the choosen 
                    // Entry of the ComboBox
                    string filename = Class.Name;
                    // The Folder are going to be created if not created
                    System.IO.Directory.CreateDirectory(
                        folderBD.SelectedPath + @"\Export");
                    // The Folder for the images
                    System.IO.Directory.CreateDirectory(
                        folderBD.SelectedPath + @"\Export\content");
                    /* Try to load a file with the Categoryname. 
                    * If there is no file with the naming, the try doesn't 
                    * work and going to the cache
                    */ 
                    try
                    {
                        BoxViewModel currentBox = ReadSavedFile(filename);
                        SaveCards.WriteXMLFile(
                            folderBD.SelectedPath + @"\Export\",
                            currentBox,
                            filename,
                            InclStat);
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
                    System.Windows.MessageBox.Show(
                        "Es muss ein Zielpfad ausgewählt werden");
                }
            }
        }

        /*
         * This Method receives the filename (Category) and try to read a file
         * of the Category. If there is a File, the Method Adds the loaded 
         * Cards to the BoxViewModel. If there is no Card, the BoxViewModel
         * stays empty. The Method returns the BoxViewModel.
         */
        private BoxViewModel ReadSavedFile(string catName)
        {
            BoxViewModel currentBox = new BoxViewModel();
            XmlDocument xmlDoc = new XmlDocument();
            // load the nodes from the categoryname.xml file
            xmlDoc.Load(saveDirectory + catName + ".xml");
            // Every node is going to be a Card 
            foreach (XmlNode node in xmlDoc.DocumentElement)
            {
                currentBox.Enqueue(LoadCards.ReadOwnFormatNode(node));
            }         
            return currentBox;
        }
    }
}
