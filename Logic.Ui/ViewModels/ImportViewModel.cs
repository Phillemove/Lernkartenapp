using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using De.HsFlensburg.ClientApp101.Logic.Ui.Support;
using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public BoxViewModel bvm;
        private readonly static Random random = new Random();
        // The stringlength for the Method RandomString()
        private readonly static int randPicNameLength = 10; 
        private OpenFileDialog ofd;
        // The variable for the FileSystemPath,
        // where the to be imported file is stored
        private static string filepath; 
        public CategoryCollectionViewModel MyModelViewModel { get; set; }

        private string classname;   // the variable for the Label in the View

        /*
         * This String is for the displaying the 
         * Classname of the loaded file in the View.
         */
        public String NewClassName { get
            {
                return classname; 
            }
            set {
                classname = value;
                OnPropertyChanged("NewClassName");
            } }

        public event PropertyChangedEventHandler PropertyChanged;

        public Boolean RadioButtonNewCatIsChecked { get; set; }
        public Boolean RadioButtonExistentCatIsChecked { get; set; }

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
            if(!RadioButtonNewCatIsChecked && !RadioButtonExistentCatIsChecked)
            {
                System.Windows.MessageBox.Show(
                    "Leider nichts ausgewählt, somit kein Import möglich");
            }
            else
            {
                foreach (CardViewModel card in this.bvm)
                {
                    if (card.AnswerPic != null)
                    {
                        string newFile = SaveCards.CopyPic(
                            filepath + 
                            @"\content\" +
                            card.AnswerPic);
                        card.AnswerPic = newFile;
                    }
                    if (card.QuestionPic != null)
                    {
                        string newFile = SaveCards.CopyPic(
                            filepath +
                            @"\content\" +
                            card.QuestionPic);
                        card.QuestionPic = newFile;
                    }
                }
                if (RadioButtonNewCatIsChecked) 
                {
                    SaveCardsWithNewCategory();
                }
                else 
                {
                    SaveCardsToExistCategory();
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
        private void SaveCardsWithNewCategory()
        {
            string catName = System.IO.Path.GetFileNameWithoutExtension(
                    ofd.FileName);
            CategoryViewModel savedCat = MyModelViewModel.Where
                (cat => cat.Name == catName).FirstOrDefault();
            if (savedCat == null)
            {
                savedCat = new CategoryViewModel(new Category(catName));
                MyModelViewModel.Add(savedCat);
                MyModelViewModel.SaveCategorys();
            }
            foreach (CardViewModel card in this.bvm)
            {
                card.Category = savedCat;
            }
            SaveCards.SaveBoxToFileSystem(this.bvm);
        }

        /*
         * This Method gives all cards the existing CategoryViewModel and 
         * push the BoxViewModel to the SaveCards Class to 
         * add the Cards to the existing Cards of the Category and saves them 
         * to the FileSystem.
         */
        private void SaveCardsToExistCategory()
        {
            foreach (CategoryViewModel catVM in MyModelViewModel)
            {
                if (catVM.Name == Class.Name)
                {
                    // Every Card gets the choosen Category
                    foreach (CardViewModel card in this.bvm)
                    {
                        card.Category = catVM;
                    }
                    SaveCards.SaveAdditionalCardBox(catVM, this.bvm);
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
            ofd = new OpenFileDialog();  
            ofd.Filter = "XML-Files|*.xml";     // Limitation for .xml Files
            // If the choosen File works
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
            {
                filepath = Path.GetDirectoryName(ofd.FileName);
                // Show the Filename in the View
                NewClassName =
                    System.IO.Path.GetFileNameWithoutExtension(ofd.FileName);
                // a new XmlDocument, which is going to load
                // the file in the next step
                XmlDocument doc = new XmlDocument();
                doc.Load(ofd.FileName);
                // A BoxViewModel, in which the cards are going to be stored
                this.bvm = new BoxViewModel();
                // The Node is going to be a 
                // card and Enqueue to the BoxViewModel
                foreach (XmlNode node in doc.DocumentElement) 
                {
                    CardViewModel card = ReadOwnFormatNode(node);
                    this.bvm.Enqueue(card);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Die Auswahl hat leider nicht geklappt");
            }
        }
        
        /*
         * This Method reads the xml Nodes and creates cards by our selfe sheme
         */
        public static CardViewModel ReadOwnFormatNode(XmlNode node)
        {
            // Creating the card, which is going to be given back
            CardViewModel card = new CardViewModel();   

            foreach (XmlNode child in node)
            {
                /*
                 * This Switch-Case looks what kind the actual
                 * Node is and creates the equivalent part of the card
                 */
                switch (child.Name)
                {
                    case "Question":
                        card.Question = child.InnerText;
                        break;
                    case "Answer":
                        card.Answer = child.InnerText;
                        break;
                    case "QuestionPic":
                        card.QuestionPic = child.InnerText;
                        break;
                    case "AnswerPic":
                        card.AnswerPic = child.InnerText;
                        break;
                    case "StatisticCollection":
                        card.StatisticCollection = new StatisticCollectionViewModel();
                        /*
                         * loop to go throug every Statistic
                         */
                        foreach(XmlNode statNode in child) 
                        {
                            StatisticViewModel stat = ReadStat(statNode);
                            
                            card.StatisticCollection.Add(stat);
                        }
                        break;
                }
            };
            return card;
            
        }

        /*
         * This Method creates a random String for the Filenames and give
         * this back. The idea and the most of the code is
         * from the following page:
         * https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
         * The Website call was on: 22.01.2021 16:33MEZ
         */
        public static string RandomString()
        {
            // This is the Range and possible Chars for the String
            const string chars =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"; 
            return new string(Enumerable.Repeat(chars, randPicNameLength)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /*
         * This Method gets an XmlNode with Statistic Nodes, create out of
         * it a Statistic Object and give this Object back.
         */
        private static StatisticViewModel ReadStat(XmlNode statNode)
        {
            // Creates a new Statistic
            StatisticViewModel stat = new StatisticViewModel();
            /*
             * loop for the details of every StatisticObject
             */
            foreach (XmlNode statDet in statNode)
            {
                switch (statDet.Name)
                {
                    case "Timestamp":
                        stat.Timestamp = Convert.ToDateTime(statDet.InnerText);
                        break;
                    case "SuccessfullAnswer":
                        stat.SuccessfulAnswer =
                            Convert.ToBoolean(statDet.InnerText);
                        break;
                    case "CurrentBoxNumber":
                        switch (statDet.InnerText)
                        {
                            case "None":
                                stat.CurrentBoxNumber = Boxnumber.None;
                                break;
                            case "Box1":
                                stat.CurrentBoxNumber = Boxnumber.Box1;
                                break;
                            case "Box2":
                                stat.CurrentBoxNumber = Boxnumber.Box2;
                                break;
                            case "Box3":
                                stat.CurrentBoxNumber = Boxnumber.Box3;
                                break;
                            case "Box4":
                                stat.CurrentBoxNumber = Boxnumber.Box4;
                                break;
                            case "Box5":
                                stat.CurrentBoxNumber = Boxnumber.Box5;
                                break;
                        }
                        break;
                }
            }
            return stat;
        }


    }

    
}
