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

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    public class CardLearningViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private String ans;
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
        public BoxCollectionViewModel myBoxCollectionViewModel { get; set; }

        public CardLearningViewModel(BoxCollectionViewModel bcvm)
        {            
            myBoxCollectionViewModel = bcvm;
            CardVM = myBoxCollectionViewModel.giveCard(Boxnumber.Box1);
            ShowCard = new RelayCommand(() => ShowCardMethod());
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
                    }
                    else
                    { //  incorrect answer
                        MessageBox.Show("Errror");
                    }

                    Answer = "";

                    // go to next card
                    CardVM = myBoxCollectionViewModel.giveCard(Boxnumber.Box1);
                }
                else
                {
                    MessageBox.Show("Please enter the Answer");
                }
            }
            else
            {
                MessageBox.Show("Cards Finished");
            }
         }
        
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
