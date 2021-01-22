﻿using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using De.HsFlensburg.ClientApp101.Logic.Ui.Support;
using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    public class ImportViewModel: INotifyPropertyChanged
    {
        public RelayCommand ChooseData { get; }
        public RelayCommand ImportData { get; }
        public CategoryViewModel Class { get; set; }
        public BoxViewModel bvm;
        private readonly static Random random = new Random();
        private readonly static int randPicNameLength = 10; // The stringlength for the Method RandomString()
        private OpenFileDialog ofd;
        private static string filepath; // The variable for the FileSystemPath, where the to be imported file is stored
        private readonly static string pictureDirectory = @"..\..\..\Lernkarten\content\";
        public CategoryCollectionViewModel MyModelViewModel { get; set; }

        private string classname;   // the variable for the Label in the View

        /*
         * This String is for the displaying the Classname of the loaded file in the View.
         */
        public String NewClassName { get
            {
                return classname; 
            }
            set {
                classname = value;
                OnPropertyChanged("NewClassName");  // If the NewClassName gets a new value, the OnPropertyChanged() is going to be called
            } } // The Property for the Label in View, which shows the choosen Classname

        public event PropertyChangedEventHandler PropertyChanged;

        

        public Boolean RadioButtonNewCatIsChecked { get; set; }
        public Boolean RadioButtonExistentCatIsChecked { get; set; }

        public ImportViewModel(CategoryCollectionViewModel categorys)
        {
            ChooseData = new RelayCommand(() => ChooseDataMethod());
            ImportData = new RelayCommand(() => ImportDataMethod());
            MyModelViewModel = categorys;
        }

        /*
         * This Method is to "inform" the View about the new String in NewClassName, so the new value is going to be shown in the view
         */
        protected void OnPropertyChanged(string propertyName)   
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /*
         * This Method saves the Cards under a new Categoryname or fuse them with available Cards of the same category. If the imported Files contains
         * a picture, the Method copys them to the correct directory and replace the Name in the Card with the new Name.
         */
        private void ImportDataMethod()
        {
            if(!RadioButtonNewCatIsChecked && !RadioButtonExistentCatIsChecked)
            {
                MessageBox.Show("Leider nichts ausgewählt, somit kein Import möglich");
            }
            else
            {
                foreach (CardViewModel card in this.bvm)
                {
                    if (card.AnswerPic != null)
                    {
                        string newFile = CopyPic(card.AnswerPic);
                        card.AnswerPic = newFile;
                    }
                    if (card.QuestionPic != null)
                    {
                        string newFile = CopyPic(card.QuestionPic);
                        card.QuestionPic = newFile;
                    }
                }
                if (RadioButtonNewCatIsChecked) 
                {
                    CategoryViewModel cat = new CategoryViewModel(new Category(System.IO.Path.GetFileNameWithoutExtension(ofd.FileName)));
                    foreach (CardViewModel card in this.bvm)    // Every Card gets the Category of the Filename
                    {
                        card.Category = cat.category;
                    }
                    MyModelViewModel.Add(cat);

                    SaveCards.SaveBoxToFileSystem(this.bvm); 
                }
                else 
                {
                    foreach(CategoryViewModel catVM in MyModelViewModel)
                    {
                        if(catVM.Name == Class.Name)
                        {
                            foreach (CardViewModel card in this.bvm)    // Every Card gets the choosen Category
                            {
                                card.Category = catVM.category;
                            }
                            SaveCards.SaveAdditionalCardBox(catVM,this.bvm);
                        }
                    }
                };
            }
        }
        
        /*
         * This Method opens the OpenFileDialog with which the user can choose a .xml File. This reades the xml Nodes and creates with them new cards.
         */
        private void ChooseDataMethod()
        {
            ofd = new OpenFileDialog();  // creating a OpenFileDialog to choose the File to be imported
            ofd.Filter = "XML-Files|*.xml";     // Limitation for .xml Files
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) // If the choosen File works
            {
                filepath = Path.GetDirectoryName(ofd.FileName);
                NewClassName = System.IO.Path.GetFileNameWithoutExtension(ofd.FileName);    // Show the Filename in the View
                XmlDocument doc = new XmlDocument();    // a new XmlDocument, which is going to load the file in the next step
                doc.Load(ofd.FileName);
                this.bvm = new BoxViewModel();  // A BoxViewModel, in which the cards are going to be stored
                foreach (XmlNode node in doc.DocumentElement)   // The Node is going to be a card and Enqueue to the BoxViewModel
                {
                    CardViewModel card = ReadOwnFormatNode(node);
                    this.bvm.Enqueue(card);
                }
            }
            else
            {
                MessageBox.Show("Die Auswahl hat leider nicht geklappt");
            }
        }
        
        /*
         * This Method reads the xml Nodes and creates cards by our selfe sheme
         */
        public static CardViewModel ReadOwnFormatNode(XmlNode node)
        {
            CardViewModel card = new CardViewModel();   // Creating the card, which is going to be given back

            foreach (XmlNode child in node)
            {
                /*
                 * This Switch-Case looks what kind the actual Node is and creates the equivalent part of the card
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
                        card.StatisticCollection = new StatisticCollection();   // Wird noch zu einer StatisticCollectionViewModel
                        /*
                         * loop to go throug every Statistic
                         */
                        foreach(XmlNode statNode in child) 
                        {
                            Statistic stat = new Statistic();   // Creates a new Statistic
                            /*
                             * loop for the details of every StatisticObject
                             */
                            foreach(XmlNode statDet in statNode)
                            {
                                switch (statDet.Name)
                                {
                                    case "Timestamp":
                                        stat.Timestamp = Convert.ToDateTime(statDet.InnerText);
                                        break;
                                    case "SuccessfullAnswer":
                                        stat.SuccessfullAnswer = Convert.ToBoolean(statDet.InnerText);
                                        break;
                                    case "CurrentBoxNumber":
                                        switch(statDet.InnerText)
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
                            card.StatisticCollection.Add(stat);
                        }
                        break;
                }
            };
            return card;
            
        }

        /*
         * This Method creates a random String for the Filenames and give this back. The idea and the most of the code is from the following page:
         * https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
         * The Website call was on: 22.01.2021 16:33MEZ
         */
        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"; // This is the Range and possible Chars for the String
            return new string(Enumerable.Repeat(chars, randPicNameLength)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /*
         * This Method copys pictures from the actual importfile to the own saveplace and returns the new Name of the Picture
         */
        public static string CopyPic(string currentPath)
        {
            string randName = RandomString() + ".jpg";
            File.Copy(filepath + @"\content\" + currentPath, pictureDirectory +  randName);
            return (randName);
        }


    }

    
}
