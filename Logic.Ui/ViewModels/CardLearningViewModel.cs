using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    public class CardLearningViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private String ans;

        private int currentProgressBarValue;
        private int maximumProgressBarValue;
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
        
        public RelayCommand ShowCard { get; }
        public RelayCommand doNotKnow { get; }
        public BoxCollectionViewModel myBoxCollectionViewModel { get; set; }

        public CardLearningViewModel(BoxCollectionViewModel bcvm)
        {
            //-----------------------------------
            aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 10000;
            //-----------------------------------

            myBoxCollectionViewModel = bcvm;
            CurrentProgressBarValue = 0;
            MaximumProgressBarValue = GetCardsCount();

            CardVM = myBoxCollectionViewModel.giveCard(Boxnumber.Box1);
            ShowCard = new RelayCommand(() => ShowCardMethod());
            doNotKnow= new RelayCommand(() => doNotKnowMethod());
           

        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            //Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",e.SignalTime);

            aTimer.Enabled = false;
            MessageBox.Show("Time finished");
            aTimer.Enabled = true;
            // go to next card
            getNextCard();
            CurrentProgressBarValue += 1;
        }
        public void ShowCardMethod()
        {
            if (CardVM != null)
            {
                if (Answer != null && !Answer.Equals(""))
                {
                    if (Answer.Equals(CardVM.Answer))
                    { //  correct answer

                        MessageBox.Show("correct answer");
                        Answer = "";
                        //-------------------           
                        //MessageBox.Show(DateTime.Now.ToString());
                        CardVM.StatisticCollection.Add(new Statistic(DateTime.Now,true,Boxnumber.None));
                    }
                    else
                    { //  incorrect answer
                        Answer = CardVM.Answer;
                        MessageBox.Show("Errror");                        
                        Answer = "";
                        //-------------------
                        CardVM.StatisticCollection.Add(new Statistic(DateTime.Now,false,Boxnumber.None));
                    }
                    // go to next card
                    getNextCard();
                    CurrentProgressBarValue += 1;
                    aTimer.Enabled = false;
                    aTimer.Enabled = true;
                    if (CardVM == null)
                    {
                        MessageBox.Show("Cards Finished");
                        aTimer.Enabled = false;
                        //------------------
                    }
                }
                else
                {
                    MessageBox.Show("Please enter the Answer");
                }
            }
            
         }
        public void doNotKnowMethod()
        {
            Answer = CardVM.Answer;
            MessageBox.Show("try again later");
            Answer = "";
            // go to next card
            getNextCard();
            CurrentProgressBarValue += 1;
            aTimer.Enabled = false;
            aTimer.Enabled = true;
            if (CardVM == null)
            {
                MessageBox.Show("Cards Finished");
                aTimer.Enabled = false;
                //------------------
            }
        }
        public void getNextCard()
        {
            //--------------------
            CardVM = myBoxCollectionViewModel.giveCard(Boxnumber.Box1);
            if (CardVM == null)
            {
                CardVM = myBoxCollectionViewModel.giveCard(Boxnumber.Box2);
            }
            if (CardVM == null)
            {
                CardVM = myBoxCollectionViewModel.giveCard(Boxnumber.Box3);
            }
            if (CardVM == null)
            {
                CardVM = myBoxCollectionViewModel.giveCard(Boxnumber.Box4);
            }
            if (CardVM == null)
            {
                CardVM = myBoxCollectionViewModel.giveCard(Boxnumber.Box5);
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


    }
}
