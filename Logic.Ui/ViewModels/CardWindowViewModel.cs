using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    public class CardWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public BoxViewModel BoxVM { get; set; }
        public String Question { get; set; }
        public String Answer { get; set; }
        public String QuestionPic { get; set; }
        public String AnswerPic { get; set; }
        public CategoryViewModel myCategory { get; set; }
        public StatisticCollectionViewModel StatisticCollection { get; set; }
        public RelayCommand AddCard { get; }
        public RelayCommand AddQuestionPic { get; }
        public RelayCommand AddAnswerPic { get; }
        public BoxCollectionViewModel myBoxCollectionViewModel { get; set; }
        public BoxViewModel myBoxesViewModel;
        public BoxViewModel MyBoxesViewModel
        {
            get { return this.myBoxesViewModel; }
            set
            {
                this.myBoxesViewModel = value;
                OnPropertyChanged("MyBox1ViewModel");
            }
        }
        public CategoryCollectionViewModel myCatCollectionViewModel { get; set; }

        public CardWindowViewModel(BoxCollectionViewModel bcvm, CategoryCollectionViewModel ccvm)
        {
            AddCard = new RelayCommand(() => AddCardMethod());
            AddQuestionPic = new RelayCommand(() => AddQuestionPicMethod());
            AddAnswerPic = new RelayCommand(() => AddAnswerPicMethod());
            myBoxCollectionViewModel = bcvm;
            MyBoxesViewModel = new BoxViewModel();
            //int cardCount = GetCardsCount();
            //for (int i = 1; i <= cardCount; i++)
            //{
            //    MyBoxesViewModel.add(getNextCard());
            //}
        }
        public int GetCardsCount()
        {
            int count = 0;
            for (int i = 0; i < 5; i++)
            {
                count += myBoxCollectionViewModel[i].Count;
            }
            //--------------------
            MessageBox.Show("Cards Count = "+count);
            return count;
        }
        public CardViewModel getNextCard()
        {
            CardViewModel CardVM = myBoxCollectionViewModel.giveCard(Boxnumber.Box1);
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
            return CardVM;
        }
        private void AddCardMethod()
        {
            CardViewModel cardVM = new CardViewModel();
            cardVM.Question = Question;
            cardVM.Answer = Answer;
            cardVM.QuestionPic = QuestionPic;
            cardVM.AnswerPic = AnswerPic;
            cardVM.Category = null;
            cardVM.StatisticCollection = null;
           //MessageBox.Show("myCategory :" + myCategory.Name);
            //MessageBox.Show(cardVM.Question+"-"+ cardVM.Answer+" - "+ cardVM.QuestionPic+"-"+ cardVM.AnswerPic
            // +"-"+ myCategory.Name + "- boxNumber:"+ BoxVM.Bn);
            myBoxCollectionViewModel.storeCard(cardVM, BoxVM.Bn);

        }
        private void AddQuestionPicMethod()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg files(*.jpg)|*.jpg| PNG files(*.png)|*.png| All Files(*.*)|*.*";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //Image newImage = Image.FromFile(dialog.FileName);
                    this.QuestionPic = dialog.FileName;
                    MessageBox.Show(this.QuestionPic);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("An Error Occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void AddAnswerPicMethod()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg files(*.jpg)|*.jpg| PNG files(*.png)|*.png| All Files(*.*)|*.*";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //Image newImage = Image.FromFile(dialog.FileName);
                    this.AnswerPic = dialog.FileName;
                    MessageBox.Show(this.AnswerPic);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("An Error Occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}




