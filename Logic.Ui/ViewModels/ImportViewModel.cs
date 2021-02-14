using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using De.HsFlensburg.ClientApp101.Logic.Ui.Support;
using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Xml;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    public class ImportViewModel: INotifyPropertyChanged
    {
        public RelayCommand ChooseData { get; }
        public RelayCommand ImportData { get; }
        public RelayCommand CloseWindow { get; }
        public CategoryViewModel Class { get; set; }
        public BoxViewModel boxVM;
        private OpenFileDialog openFD;
        // The variable for the FileSystemPath,
        // where the to be imported file is stored
        private static string filepath; 
        public CategoryCollectionViewModel MyModelViewModel { get; set; }
        private string newClassName;  // the variable for the Label in the View
        /*
         * This String is for the displaying the 
         * Classname of the loaded file in the View.
         */
        public String NewClassName { get
            {
                return newClassName; 
            }
            set {
                newClassName = value;
                OnPropertyChanged("NewClassName");
            } }

        public event PropertyChangedEventHandler PropertyChanged;
        public Boolean NewCatCheck { get; set; }
        public Boolean ExistCatCheck { get; set; }

        public ImportViewModel(CategoryCollectionViewModel categorys)
        {
            ChooseData = new RelayCommand(() => ChooseDataMethod());
            ImportData = new RelayCommand(() => ImportDataMethod());
            CloseWindow = new RelayCommand(param => Close(param));
            MyModelViewModel = categorys;
        }

        /*
         * This Method is to "inform" the View about the new String in
         * NewClassName, so the new value is going to be shown in the view
         */
        protected void OnPropertyChanged(string propertyName)   
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

        /*
         * This Method close the Window
         */
        private void Close(object param)
        {
            Window window = (Window)param;
            window.Close();
        }

        /*
         * This Method saves the Cards under a new Categoryname or fuse 
         * them with available Cards of the same category.
         * If the imported Files contains a picture,
         * the Method copys them to the correct directory
         * and replace the Name in the Card with the new Name.
         */
        private void ImportDataMethod()
        {
            if(!NewCatCheck && !ExistCatCheck)
            {
                System.Windows.MessageBox.Show(
                    "Leider nichts ausgewählt, somit kein Import möglich");
            }
            else
            {
                foreach (CardViewModel card in boxVM)
                {
                    if (card.AnswerPic != null)
                    {
                        if (card.AnswerPic.Contains("default.jpg"))
                        {
                            /*
                             * Because on 14.02.2021 there comes a change 
                             * which needs this step to fix it for the first
                             * time
                             */
                            card.AnswerPic = null;
                        }
                        else
                        {
                            card.AnswerPic = SaveCards.CopyPic(
                                filepath +
                                @"\content\" +
                                card.AnswerPic);
                        }
                    }
                    if (card.QuestionPic != null)
                    {
                        if (card.QuestionPic.Contains("default.jpg"))
                        {
                            /*
                             * Because on 14.02.2021 there comes a change 
                             * which needs this step to fix it for the first
                             * time
                             */
                            card.QuestionPic = null;
                        }
                        else
                        {
                            card.QuestionPic = SaveCards.CopyPic(
                            filepath +
                            @"\content\" +
                            card.QuestionPic);
                        }
                        
                    }
                }
                if (NewCatCheck) 
                {
                    SaveCardsWithNewCategory(MyModelViewModel);
                }
                else 
                {
                    SaveCardsToExistCategory(MyModelViewModel);
                };
                NewClassName = "Keine Datei ausgewählt";
            }
        }

        /*
         * This Method looks if there is a CategoryViewModel allready with the 
         * same Name and Overwrite the Cards in the Category. 
         * If there is no CategoryViewModel with the Name, it creates
         * a new one, add it to the CategoryCollectionViewModel, saves the
         * CategoryCollectionViewModel and saves the Cards to the Filesystem
         * with the help of the SaveCards Class.
         */
        private void SaveCardsWithNewCategory(CategoryCollectionViewModel ccvm)
        {
            string catName = System.IO.Path.GetFileNameWithoutExtension(
                    openFD.FileName);
            CategoryViewModel savedCat = MyModelViewModel.Where
                (cat => cat.Name == catName).FirstOrDefault();
            if (savedCat == null)
            {
                savedCat = new CategoryViewModel(new Category(catName));
                MyModelViewModel.Add(savedCat);
                MyModelViewModel.SaveCategorys();
            }
            foreach (CardViewModel card in boxVM)
            {
                card.Category = savedCat;
            }
            SaveCards.SaveBoxToFileSystem(boxVM, ccvm);
        }

        /*
         * This Method gives all cards the existing CategoryViewModel and 
         * push the BoxViewModel to the SaveCards Class to 
         * add the Cards to the existing Cards of the Category and saves them 
         * to the FileSystem.
         */
        private void SaveCardsToExistCategory(CategoryCollectionViewModel ccvm)
        {
            foreach (CategoryViewModel catVM in MyModelViewModel)
            {
                if (catVM.Name == Class.Name)
                {
                    // Every Card gets the choosen Category
                    foreach (CardViewModel card in boxVM)
                    {
                        card.Category = catVM;
                    }
                    SaveCards.SaveAdditionalCardBox(catVM, boxVM, ccvm);
                }
            }
        }


        /*
         * This Method opens the OpenFileDialog with which the
         * user can choose a .xml File. This reades the xml Nodes
         * and creates with them new cards.
         */
        private void ChooseDataMethod()
        {
            // creating a OpenFileDialog to choose the File to be imported
            openFD = new OpenFileDialog
            {
                Filter = "XML-Files|*.xml"     // Limitation for .xml Files
            };
            // If the choosen File works
            if (openFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filepath = Path.GetDirectoryName(openFD.FileName);
                // Show the Filename in the View
                NewClassName =
                   System.IO.Path.GetFileNameWithoutExtension(openFD.FileName);
                // a new XmlDocument, which is going to load
                // the file in the next step
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(openFD.FileName);
                // A BoxViewModel, in which the cards are going to be stored
                boxVM = new BoxViewModel();
                // The Node is going to be a 
                // card and Enqueue to the BoxViewModel
                foreach (XmlNode node in xmlDoc.DocumentElement)
                {
                    CardViewModel card = LoadCards.ReadOwnFormatNode(node);
                    boxVM.Enqueue(card);
                }
            }
            else
            {
                System.Windows.MessageBox.Show(
                    "Die Auswahl hat leider nicht geklappt");
            }
        }
    }   
}
