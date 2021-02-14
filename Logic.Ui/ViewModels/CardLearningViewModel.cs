using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using System;
using System.ComponentModel;
using System.Timers;
using De.HsFlensburg.ClientApp101.Services.MessageBus;
using De.HsFlensburg.ClientApp101.Logic.Ui.MessageBusMessages;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    public class CardLearningViewModel : INotifyPropertyChanged
    {
        public StatisticWindowViewModel StatisticWindowVM { get; set; }        
        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand OpenStatisticsWindow { get; }
        public RelayCommand CloseWindow { get; }
        private String ans;
        private String ansPic;
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
        public String AnswerPic
        {
            get { return this.ansPic; }
            set
            {
                this.ansPic = value;
                OnPropertyChanged("AnswerPic");
            }
        }
        private CardViewModel cvm;
        public CardViewModel CardVM
        {
            get { return this.cvm; }
            set
            {
                this.cvm = value;
                OnPropertyChanged("CardVM");
            }
        }
        public RelayCommand TrueAnswer { get; }
        public RelayCommand FalseAnswer { get; }
        public BoxCollectionViewModel BoxCollectionViewModel { get; set; }

        public BoxCollectionViewModel tempBoxCollectionViewModel { get; set; }
        public CardLearningViewModel(BoxCollectionViewModel bcvm,
            CategoryCollectionViewModel ccvm,
            StatisticWindowViewModel swvm)
        {
            BoxCollectionViewModel = new BoxCollectionViewModel();
            copyBoxCollectionViewModels(bcvm, BoxCollectionViewModel);
            tempBoxCollectionViewModel = new BoxCollectionViewModel();
            copyBoxCollectionViewModels(BoxCollectionViewModel,
                                        tempBoxCollectionViewModel);
            aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 10000;
            this.StatisticWindowVM = swvm;
            CurrentProgressBarValue = 0;
            MaximumProgressBarValue = GetCardsCount();
            CardVM = BoxCollectionViewModel.GiveCard(Boxnumber.Box1);
            curBoxNumber = Boxnumber.Box1;
            if (CardVM == null)
            {
                getNextCard();
            }
            TrueAnswer = new RelayCommand(() => TrueAnswerMethod());
            FalseAnswer = new RelayCommand(() => FalseAnswerMethod());
            OpenStatisticsWindow = new RelayCommand(() =>
            {
                this.StatisticWindowVM.MakeStatistic();
                ServiceBus.Instance.Send(new OpenStatisticMessage());
            });
            CloseWindow = new RelayCommand(param => Close(param));
            this.ccvm = ccvm;
            Answer = "";
            AnswerPic = Path.GetDirectoryName(
                        Assembly.GetEntryAssembly().Location)
                        + "\\..\\..\\..\\Lernkarten\\content\\wait.jpg";
        }

        private void Close(object param)
        {
            Support.SaveCards.SaveBoxCollectionsToFilesystem(
                BoxCollectionViewModel, ccvm);   
            System.Windows.Window window = (System.Windows.Window)param;
            window.Close();
        }
        public void copyBoxCollectionViewModels(BoxCollectionViewModel from
            , BoxCollectionViewModel to)
        {
            to.Clear();
            to.Add(new BoxViewModel(Boxnumber.Box1));
            to.Add(new BoxViewModel(Boxnumber.Box2));
            to.Add(new BoxViewModel(Boxnumber.Box3));
            to.Add(new BoxViewModel(Boxnumber.Box4));
            to.Add(new BoxViewModel(Boxnumber.Box5));
            for (int boxnum = 0; boxnum < 5; boxnum++)
            {
                CardViewModel[] array = new CardViewModel[from[boxnum].Count];
                for (int i = 0; i < from[boxnum].Count; i++)
                {
                    array[i] = from[boxnum].Remove();
                    from[boxnum].Add(array[i]);
                }
                for (int i = 0; i < from[boxnum].Count; i++)
                {
                    to[boxnum].Add(array[i]);
                }
            }
        }
        public void addStaticsToCardVM(BoxCollectionViewModel from,
            CardViewModel cardvm, StatisticViewModel svm)
        {
            BoxCollectionViewModel to = new BoxCollectionViewModel();
            to.Add(new BoxViewModel(Boxnumber.Box1));
            to.Add(new BoxViewModel(Boxnumber.Box2));
            to.Add(new BoxViewModel(Boxnumber.Box3));
            to.Add(new BoxViewModel(Boxnumber.Box4));
            to.Add(new BoxViewModel(Boxnumber.Box5));
            for (int boxnum = 0; boxnum < 5; boxnum++)
            {
                CardViewModel[] array = new CardViewModel[from[boxnum].Count];
                for (int i = 0; i < from[boxnum].Count; i++)
                {
                    array[i] = from[boxnum].Remove();
                    if (array[i].Equals(cardvm))
                    {
                        array[i].StatisticCollection.Add(svm);
                    }
                    from[boxnum].Add(array[i]);
                }
                for (int i = 0; i < from[boxnum].Count; i++)
                {
                    to[boxnum].Add(array[i]);
                }
            }
            copyBoxCollectionViewModels(to, from);
        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            aTimer.Enabled = false;
            if (CardVM != null)
            {
                Answer = CardVM.Answer;
                AnswerPic = CardVM.AnswerPic;
            }
            else
            {
                CurrentProgressBarValue += 100;
                var confirmResult = MessageBox.Show(
                    "Do you want to start again ?",
                    "Confirm start again!!",
                     MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    CurrentProgressBarValue = 0;
                    copyBoxCollectionViewModels(tempBoxCollectionViewModel,
                                                BoxCollectionViewModel);
                    getNextCard();
                    aTimer.Enabled = true;
                }
                else
                {
                    copyBoxCollectionViewModels(tempBoxCollectionViewModel,
                                                BoxCollectionViewModel);
                }
            }
        }
        public void TrueAnswerMethod()
        {
            if (CardVM != null)
            {
                Answer = "";
                AnswerPic = Path.GetDirectoryName(
                            Assembly.GetEntryAssembly().Location)
                            + "\\..\\..\\..\\Lernkarten\\content\\wait.jpg";
                addStaticsToCardVM(tempBoxCollectionViewModel, CardVM,
                                    new StatisticViewModel(new Statistic(
                                        DateTime.Now, true, curBoxNumber)));
                this.StatisticWindowVM.StatisticCollectionVM = 
                                CardVM.StatisticCollection;
                getNextCard();
                aTimer.Enabled = true;
                CurrentProgressBarValue += 1;
            }
            else
            {
                CurrentProgressBarValue += 100;
            }
        }
        public void FalseAnswerMethod()
        {
            if (CardVM != null)
            {
                Answer = "";
                AnswerPic = Path.GetDirectoryName(
                            Assembly.GetEntryAssembly().Location)
                            + "\\..\\..\\..\\Lernkarten\\content\\wait.jpg";
                addStaticsToCardVM(tempBoxCollectionViewModel, CardVM,
                new StatisticViewModel(
                    new Statistic(DateTime.Now, false, curBoxNumber)));
                this.StatisticWindowVM.StatisticCollectionVM = 
                                CardVM.StatisticCollection;
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
                if (curBoxNumber == Boxnumber.Box1)
                {
                    CardVM = BoxCollectionViewModel.GiveCard(Boxnumber.Box2);
                    curBoxNumber = Boxnumber.Box2;
                }else if (curBoxNumber == Boxnumber.Box2)
                {
                    CardVM = BoxCollectionViewModel.GiveCard(Boxnumber.Box3);
                    curBoxNumber = Boxnumber.Box3;
                }else if (curBoxNumber == Boxnumber.Box3)
                {
                    CardVM = BoxCollectionViewModel.GiveCard(Boxnumber.Box4);
                    curBoxNumber = Boxnumber.Box4;
                }
                else if (curBoxNumber == Boxnumber.Box4)
                {
                    CardVM = BoxCollectionViewModel.GiveCard(Boxnumber.Box5);
                    curBoxNumber = Boxnumber.Box5;
                }
                else if (curBoxNumber == Boxnumber.Box5)
                {
                    CardVM = BoxCollectionViewModel.GiveCard(Boxnumber.Box1);
                    curBoxNumber = Boxnumber.Box1;
                }
        }
        public void moveCard()
        {
            if (curBoxNumber == Boxnumber.Box1)
            {
                BoxCollectionViewModel.StoreCard(CardVM, Boxnumber.Box5);
            }
            else if (curBoxNumber == Boxnumber.Box2)
            {
                BoxCollectionViewModel.StoreCard(CardVM, Boxnumber.Box1);
            }
            else if (curBoxNumber == Boxnumber.Box3)
            {
                BoxCollectionViewModel.StoreCard(CardVM, Boxnumber.Box2);
            }
            else if (curBoxNumber == Boxnumber.Box4)
            {
                BoxCollectionViewModel.StoreCard(CardVM, Boxnumber.Box3);
            }
            else if (curBoxNumber == Boxnumber.Box5)
            {
                BoxCollectionViewModel.StoreCard(CardVM, Boxnumber.Box4);
            }
        }
        public int GetCardsCount()
        {
            int count = 0;
            for (int i = 0; i < 5; i++)
            {
                count += BoxCollectionViewModel[i].Count;
            }
            return count;
        }
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs
                                     (propertyName));
        }
    }
}
