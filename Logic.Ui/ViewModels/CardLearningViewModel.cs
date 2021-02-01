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
        private Boxnumber curBoxNumber { get; set; }
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
        
        public RelayCommand TrueAnswer { get; }
        public RelayCommand FalseAnswer { get; }
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
            TrueAnswer = new RelayCommand(() => TrueAnswerMethod());
            FalseAnswer = new RelayCommand(() => FalseAnswerMethod());
           

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
                CardVM.StatisticCollection.Add(new Statistic(DateTime.Now, true, curBoxNumber));
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
                CardVM.StatisticCollection.Add(new Statistic(DateTime.Now, false, curBoxNumber));
                moveCard();
                getNextCard();
                aTimer.Enabled = true;
            }
            else
            {
                CurrentProgressBarValue += 100;
            }
        }
        //public void TrueAnswerMethod1()
        //{
        //    if (CardVM != null)
        //    {
        //        if (Answer != null && !Answer.Equals(""))
        //        {
        //            if (Answer.Equals(CardVM.Answer))
        //            { //  correct answer

        //                MessageBox.Show("correct answer");
        //                Answer = "";
        //                //-------------------           
        //                //MessageBox.Show(DateTime.Now.ToString());
        //                CardVM.StatisticCollection.Add(new Statistic(DateTime.Now,true,Boxnumber.None));
        //            }
        //            else
        //            { //  incorrect answer
        //                Answer = CardVM.Answer;
        //                MessageBox.Show("Errror");                        
        //                Answer = "";
        //                //-------------------
        //                CardVM.StatisticCollection.Add(new Statistic(DateTime.Now,false,Boxnumber.None));
        //            }
        //            // go to next card
        //            getNextCard();
        //            CurrentProgressBarValue += 1;
        //            aTimer.Enabled = false;
        //            aTimer.Enabled = true;
        //            if (CardVM == null)
        //            {
        //                MessageBox.Show("Cards Finished");
        //                aTimer.Enabled = false;
        //                //------------------
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Please enter the Answer");
        //        }
        //    }

        // }
        //public void FalseAnswerMethod1()
        //{
        //    Answer = CardVM.Answer;
        //    MessageBox.Show("try again later");
        //    Answer = "";
        //    go to next card
        //    getNextCard();
        //    CurrentProgressBarValue += 1;
        //    aTimer.Enabled = false;
        //    aTimer.Enabled = true;
        //    if (CardVM == null)
        //    {
        //        MessageBox.Show("Cards Finished");
        //        aTimer.Enabled = false;
        //        ------------------
        //    }
        //}
        public void getNextCard()
        {
            //--------------------
            CardVM = myBoxCollectionViewModel.giveCard(Boxnumber.Box1);
            curBoxNumber = Boxnumber.Box1;
            if (CardVM == null)
            {
                CardVM = myBoxCollectionViewModel.giveCard(Boxnumber.Box2);
                curBoxNumber = Boxnumber.Box2;
            }
            if (CardVM == null)
            {
                CardVM = myBoxCollectionViewModel.giveCard(Boxnumber.Box3);
                curBoxNumber = Boxnumber.Box3;
            }
            if (CardVM == null)
            {
                CardVM = myBoxCollectionViewModel.giveCard(Boxnumber.Box4);
                curBoxNumber = Boxnumber.Box4;
            }
            if (CardVM == null)
            {
                CardVM = myBoxCollectionViewModel.giveCard(Boxnumber.Box5);
                curBoxNumber = Boxnumber.Box5;
            }
        }
        public void moveCard()
        {
            if (curBoxNumber == Boxnumber.Box1)
            {
                myBoxCollectionViewModel.storeCard(CardVM, Boxnumber.Box2);
            }
            else if (curBoxNumber == Boxnumber.Box2)
            {
                myBoxCollectionViewModel.storeCard(CardVM, Boxnumber.Box3);
            }
            else if (curBoxNumber == Boxnumber.Box3)
            {
                myBoxCollectionViewModel.storeCard(CardVM, Boxnumber.Box4);
            }
            else if (curBoxNumber == Boxnumber.Box4)
            {
                myBoxCollectionViewModel.storeCard(CardVM, Boxnumber.Box5);
            }
            else if (curBoxNumber == Boxnumber.Box5)
            {
                myBoxCollectionViewModel.storeCard(CardVM, Boxnumber.Box1);
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
