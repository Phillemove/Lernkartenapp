using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using De.HsFlensburg.ClientApp101.Services.MessageBus;
using De.HsFlensburg.ClientApp101.Logic.Ui.MessageBusMessages;
using System.Xml;
using System.IO;
using System.Reflection;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    public class CardLearningViewModel : INotifyPropertyChanged
    {
        public StatisticWindowViewModel StatisticWindowVM { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand OpenStatisticsWindow { get; }
        public RelayCommand CloseWindow { get; }

        private String ans;
        private Boxnumber curBoxNumber { get; set; }
        private int currentProgressBarValue;
        private int maximumProgressBarValue;
        private readonly CategoryCollectionViewModel ccvm;
        public static System.Timers.Timer aTimer { get; set; }
        public int CurrentProgressBarValue
        {
            get { return this.currentProgressBarValue; }
            set
            {
                this.currentProgressBarValue = value;
                OnPropertyChanged("CurrentProgressBarValue");
            }
        }
        public int MaximumProgressBarValue
        {
            get { return this.maximumProgressBarValue; }
            set
            {
                this.maximumProgressBarValue = value;
                OnPropertyChanged("MaximumProgressBarValue");
            }
        }
        public String Answer
        {
            get { return this.ans; }
            set
            {
                this.ans = value;
                OnPropertyChanged("Answer");
            }
        }
        private CardViewModel cvm;
        public CardViewModel CardVM
        { 
            get { return this.cvm; } 
            set { this.cvm = value;
                OnPropertyChanged("CardVM"); } 
        }        
        public RelayCommand TrueAnswer { get; }
        public RelayCommand FalseAnswer { get; }
        public BoxCollectionViewModel myBoxCollectionViewModel { get; set; }

       

        public CardLearningViewModel(BoxCollectionViewModel bcvm, CategoryCollectionViewModel ccvm, StatisticWindowViewModel swvm)
        {
            myBoxCollectionViewModel = bcvm;
            // load cards from xml file
            LoadCards();

            //-----------------------------------
            aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 2000;
            //-----------------------------------

            this.StatisticWindowVM = swvm;


            CurrentProgressBarValue = 0;
            MaximumProgressBarValue = GetCardsCount();


            CardVM = myBoxCollectionViewModel.GiveCard(Boxnumber.Box1);
            TrueAnswer = new RelayCommand(() => TrueAnswerMethod());
            FalseAnswer = new RelayCommand(() => FalseAnswerMethod());

            OpenStatisticsWindow = new RelayCommand(() => {
                this.StatisticWindowVM.MakeStatistic();
                ServiceBus.Instance.Send(new OpenStatisticMessage());
                });
            CloseWindow = new RelayCommand(param => Close(param));
            this.ccvm = ccvm;
            
        }

        private void Close(object param)
        {
            Support.SaveCards.SaveBoxCollectionsToFilesystem(myBoxCollectionViewModel, ccvm);   //Saves the learned Cards to the Filesystem
            Window window = (Window)param;
            window.Close();
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            //Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",e.SignalTime);

            aTimer.Enabled = false;
            //MessageBox.Show("Time finished");
            //aTimer.Enabled = true;
            // go to next card
            if (CardVM != null)
            {
                Answer = CardVM.Answer;
            }
            else {
                CurrentProgressBarValue += 100;
            }
            //getNextCard();
            //CurrentProgressBarValue += 1;
        }
        public void TrueAnswerMethod()
         {
            if (CardVM != null)
            {
                Answer = "";
                CardVM.StatisticCollection.Add(new StatisticViewModel(new Statistic(DateTime.Now, true, curBoxNumber)));
                this.StatisticWindowVM.StatisticCollectionVM = CardVM.StatisticCollection;
                //StatisticWindowVM = new StatisticWindowViewModel(CardVM.StatisticCollection);
                getNextCard();
                aTimer.Enabled = true;
                CurrentProgressBarValue += 1;
            }
            else {
                CurrentProgressBarValue += 100;
            }
        }
        public void FalseAnswerMethod()
        {
            if (CardVM != null)
            {
                Answer = "";
                CardVM.StatisticCollection.Add(new StatisticViewModel(new Statistic(DateTime.Now, false, curBoxNumber)));
                this.StatisticWindowVM.StatisticCollectionVM = CardVM.StatisticCollection;
                //StatisticWindowVM = new StatisticWindowViewModel(CardVM.StatisticCollection);
                moveCard();
                getNextCard();
                aTimer.Enabled = true;
            }
            else
            {
                CurrentProgressBarValue += 100;
            }
        }
        public void getNextCard()
        {
            //--------------------
            CardVM = myBoxCollectionViewModel.GiveCard(Boxnumber.Box1);
            curBoxNumber = Boxnumber.Box1;
            if (CardVM == null)
            {
                CardVM = myBoxCollectionViewModel.GiveCard(Boxnumber.Box2);
                curBoxNumber = Boxnumber.Box2;
            }
            if (CardVM == null)
            {
                CardVM = myBoxCollectionViewModel.GiveCard(Boxnumber.Box3);
                curBoxNumber = Boxnumber.Box3;
            }
            if (CardVM == null)
            {
                CardVM = myBoxCollectionViewModel.GiveCard(Boxnumber.Box4);
                curBoxNumber = Boxnumber.Box4;
            }
            if (CardVM == null)
            {
                CardVM = myBoxCollectionViewModel.GiveCard(Boxnumber.Box5);
                curBoxNumber = Boxnumber.Box5;
            }
        }
        public void moveCard()
        {
            if (curBoxNumber == Boxnumber.Box1)
            {
                myBoxCollectionViewModel.StoreCard(CardVM, Boxnumber.Box2);
            }
            else if (curBoxNumber == Boxnumber.Box2)
            {
                myBoxCollectionViewModel.StoreCard(CardVM, Boxnumber.Box3);
            }
            else if (curBoxNumber == Boxnumber.Box3)
            {
                myBoxCollectionViewModel.StoreCard(CardVM, Boxnumber.Box4);
            }
            else if (curBoxNumber == Boxnumber.Box4)
            {
                myBoxCollectionViewModel.StoreCard(CardVM, Boxnumber.Box5);
            }
            else if (curBoxNumber == Boxnumber.Box5)
            {
                myBoxCollectionViewModel.StoreCard(CardVM, Boxnumber.Box1);
            }
        }
        public int GetCardsCount()
        {
            int count =0;
            for (int i = 0; i < 5; i++)
            { 
               count += myBoxCollectionViewModel[i].Count;
            }
            //--------------------
            //MessageBox.Show("Cards Count = "+count);
            return count;
        }
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        //-------------------------------------------------------------------------------------
        public void LoadCards()
        {
            XmlDocument doc1 = new XmlDocument();
            doc1.Load(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)+ 
                                            "\\..\\..\\..\\Lernkarten\\AWP.xml");
            foreach (XmlNode node in doc1.DocumentElement)
            {
                CardViewModel cardvm = LoadCardViewModel(node);
                cardvm.QuestionPic= Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)+
                                    "\\..\\..\\..\\Lernkarten\\content\\" + cardvm.QuestionPic;
                cardvm.AnswerPic = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
                                    "\\..\\..\\..\\Lernkarten\\content\\" + cardvm.AnswerPic;
                if (cardvm.StatisticCollection != null)
                {
                    this.myBoxCollectionViewModel.StoreCard(cardvm,
                         cardvm.StatisticCollection[0].CurrentBoxNumber);
                    //MessageBox.Show(cardvm.StatisticCollection[0].CurrentBoxNumber+" ");

                }
                else
                {
                    this.myBoxCollectionViewModel.StoreCard(cardvm,Boxnumber.Box1);
                }
            }
            //-----------------------------------------------------------------------------------------------------
            XmlDocument doc2 = new XmlDocument();
            doc2.Load(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
                                           "\\..\\..\\..\\Lernkarten\\Netzwerkadministration.xml");
            foreach (XmlNode node in doc2.DocumentElement)
            {
                CardViewModel cardvm = LoadCardViewModel(node);
                cardvm.QuestionPic = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
                                    "\\..\\..\\..\\Lernkarten\\content\\" + cardvm.QuestionPic;
                cardvm.AnswerPic = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
                                    "\\..\\..\\..\\Lernkarten\\content\\" + cardvm.AnswerPic;
                if (cardvm.StatisticCollection != null)
                {
                    this.myBoxCollectionViewModel.StoreCard(cardvm,
                         cardvm.StatisticCollection[0].CurrentBoxNumber);
                    //MessageBox.Show(cardvm.StatisticCollection[0].CurrentBoxNumber + " ");

                }
                else
                {
                    this.myBoxCollectionViewModel.StoreCard(cardvm, Boxnumber.Box1);
                }
            }
            //-----------------------------------------------------------------------------------------------------
            XmlDocument doc3 = new XmlDocument();
            doc3.Load(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
                                           "\\..\\..\\..\\Lernkarten\\Projektmanagement.xml");
            foreach (XmlNode node in doc3.DocumentElement)
            {
                CardViewModel cardvm = LoadCardViewModel(node);
                cardvm.QuestionPic = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
                                    "\\..\\..\\..\\Lernkarten\\content\\" + cardvm.QuestionPic;
                cardvm.AnswerPic = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
                                    "\\..\\..\\..\\Lernkarten\\content\\" + cardvm.AnswerPic;
                if (cardvm.StatisticCollection != null)
                {
                    this.myBoxCollectionViewModel.StoreCard(cardvm,
                         cardvm.StatisticCollection[0].CurrentBoxNumber);
                    //MessageBox.Show(cardvm.StatisticCollection[0].CurrentBoxNumber + " ");

                }
                else
                {
                    this.myBoxCollectionViewModel.StoreCard(cardvm, Boxnumber.Box1);
                }
            }
            //-----------------------------------------------------------------------------------------------------
        }
        public static CardViewModel LoadCardViewModel(XmlNode node)
        {
            CardViewModel card = new CardViewModel();
            foreach (XmlNode child in node)
            {
                switch (child.Name)
                {
                    case "Question":
                        //MessageBox.Show(child.InnerText);
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
                        foreach (XmlNode statNode in child)
                        {
                            StatisticViewModel stat = LoadStatisticViewModel(statNode);
                            card.StatisticCollection.Add(stat);
                        }
                        break;
                }
            };
            return card;

        } // end CardViewModel ReadOwnFormatNode(XmlNode node)
        private static StatisticViewModel LoadStatisticViewModel(XmlNode statNode)
        {
            StatisticViewModel stat = new StatisticViewModel();
            foreach (XmlNode statDet in statNode)
            {
                switch (statDet.Name)
                {
                    case "Timestamp":
                        //MessageBox.Show(statDet.InnerText);
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
        //-------------------------------------------------------------------------

    }

}
