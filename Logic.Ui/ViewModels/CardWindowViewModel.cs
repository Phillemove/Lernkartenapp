using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

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

        public CategoryViewModel Catvm { get; set; }
        public RelayCommand AddCard { get; }
        public RelayCommand AddQuestionPic { get; }
        public RelayCommand AddAnswerPic { get; }
        public RelayCommand UpdateQuestionPic { get; }
        public RelayCommand UpdateAnswerPic { get; }
        public RelayCommand EditQuestionPic { get; }
        public RelayCommand EditAnswerPic { get; }
        public RelayCommand DeleteCard { get; }
        public RelayCommand CloseManageWindow { get; }
        public BoxCollectionViewModel MyBoxCollectionViewModel { get; set; }

        public BoxViewModel selectedBox;
        public BoxViewModel SelectedBox
        {
            get { return this.selectedBox; }
            set
            {
                this.selectedBox = value;
                OnPropertyChanged("SelectedBox");
            }
        }
        public CardViewModel selectedCard;
        public CardViewModel SelectedCard
        {
            get { return this.selectedCard; }
            set
            {
                this.selectedCard = value;
                OnPropertyChanged("SelectedCard");
            }
        }
        public CategoryCollectionViewModel MyCatCollectionViewModel 
        { get; set; }

        public CardWindowViewModel(BoxCollectionViewModel bcvm,
            CategoryCollectionViewModel ccvm)
        {
            AddCard = new RelayCommand(() =>
            AddCardMethod());
            AddQuestionPic = new RelayCommand(() =>
            AddQuestionPicMethod());
            AddAnswerPic = new RelayCommand(() =>
            AddAnswerPicMethod());

            UpdateQuestionPic = new RelayCommand(() =>
            UpdateQuestionPicMethod());
            UpdateAnswerPic = new RelayCommand(() =>
            UpdateAnswerPicMethod());

            EditQuestionPic = new RelayCommand(() =>
            EditQuestionPicMethod());
            EditAnswerPic = new RelayCommand(() =>
            EditAnswerPicMethod());

            DeleteCard = new RelayCommand(() =>
            DeleteCardMethod());

            CloseManageWindow = new RelayCommand((param) =>
            CloseManage(param));
            MyBoxCollectionViewModel = bcvm;
            MyCatCollectionViewModel = ccvm;
        }
        public void CloseManage(object param)
        {
            Support.SaveCards.SaveBoxCollectionsToFilesystem
                (MyBoxCollectionViewModel,MyCatCollectionViewModel);
            Window window = (Window)param;
            window.Close();
        }
        private void AddCardMethod()
        {
            CardViewModel cardVM = new CardViewModel();
            cardVM.Question = Question;
            cardVM.Answer = Answer;
            cardVM.QuestionPic = QuestionPic;
            cardVM.AnswerPic = AnswerPic;
            cardVM.Category = Catvm;
            cardVM.StatisticCollection = null;
            MyBoxCollectionViewModel.StoreCard(cardVM, BoxVM.Bn);
            Support.SaveCards.SaveAdditionalCard(cardVM.Category, cardVM);
        }
        private void AddQuestionPicMethod()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg files(*.jpg)|*" +
                    ".jpg| PNG files(*.png)|*.png| All Files(*.*)|*.*";
                if (dialog.ShowDialog() == 
                    System.Windows.Forms.DialogResult.OK)
                {
                    this.QuestionPic = dialog.FileName;
                    MessageBox.Show(this.QuestionPic);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("An Error Occured", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void UpdateQuestionPicMethod()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg files(*.jpg)|*" +
                    ".jpg| PNG files(*.png)|*.png| All Files(*.*)|*.*";
                if (dialog.ShowDialog() == 
                    System.Windows.Forms.DialogResult.OK)
                {
                    SelectedCard.QuestionPic = dialog.FileName;
                    MessageBox.Show(SelectedCard.QuestionPic);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("An Error Occured", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void EditQuestionPicMethod()
        {
            try
            {
                ProcessStartInfo startInfo = 
                    new ProcessStartInfo(SelectedCard.QuestionPic);
                startInfo.Verb = "edit";
                Process.Start(startInfo);

            }
            catch (Exception)
            {
                MessageBox.Show("An Error Occured", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AddAnswerPicMethod()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg files(*.jpg)|*.jpg|" +
                    " PNG files(*.png)|*.png| All Files(*.*)|*.*";
                if (dialog.ShowDialog() == 
                    System.Windows.Forms.DialogResult.OK)
                {
                    this.AnswerPic = dialog.FileName;
                    MessageBox.Show(this.AnswerPic);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("An Error Occured", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateAnswerPicMethod()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg files(*.jpg)|*.jpg|" +
                    " PNG files(*.png)|*.png| All Files(*.*)|*.*";
                if (dialog.ShowDialog() == 
                    System.Windows.Forms.DialogResult.OK)
                {
                    SelectedCard.AnswerPic = dialog.FileName;
                    MessageBox.Show(this.AnswerPic);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("An Error Occured", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void EditAnswerPicMethod()
        {
            try
            {
                ProcessStartInfo startInfo =
                    new ProcessStartInfo(SelectedCard.AnswerPic);
                startInfo.Verb = "edit";
                Process.Start(startInfo);

            }
            catch (Exception)
            {
                MessageBox.Show("An Error Occured", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DeleteCardMethod()
        {
                String s = "";
                BoxViewModel boxvm = new BoxViewModel();
                int leng = SelectedBox.Count;
                for (int i = 0; i < leng; i++)
                {
                    CardViewModel cvm = SelectedBox.Remove();
                if (!cvm.Equals(selectedCard))
                {
                    boxvm.Add(cvm);
                    
                }
                else
                {
                    s += cvm.Question;
                }
                }
                leng = boxvm.Count;
                for (int i = 0; i < leng; i++)
                {
                    CardViewModel cvm = boxvm.Remove();
                    SelectedBox.Add(cvm);
                    
                }
                MessageBox.Show(selectedCard.Question +
                    "was deleted successfully");
        }
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, 
                    new PropertyChangedEventArgs(propertyName));
        }

    }

}




