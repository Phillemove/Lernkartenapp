using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using De.HsFlensburg.ClientApp101.Logic.Ui.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Runtime.CompilerServices;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    /*
     * ViewModel for the UI-Element StatisticWindow. Method fill the Properties
     * with Data, that they can be displayed in the UI
     */
    public class StatisticWindowViewModel : INotifyPropertyChanged
    {

        public StatisticCollectionViewModel StatisticCollectionVM { get; set; }

        private int learnedCard;
        public int LearnedCard {
            get
            {
                return learnedCard;
            }
            set
            {
                learnedCard = value;
                OnPropertyChanged();
            }
        }

        private int rightAnswer;
        public int RightAnswer {
            get
            {
                return rightAnswer;
            }
            set
            {
                rightAnswer = value;
                OnPropertyChanged();
            }
        }

        private String lastSucessfullAnswer;
        public String LastSucessfullAnswer {
            get
            {
                return lastSucessfullAnswer;
            }
            set
            {
                lastSucessfullAnswer = value;
                OnPropertyChanged();
            }
        }

        private int learningSucess;
        public int LearningSucess {
            get
            {
                return learningSucess;
            }
            set
            {
                learningSucess = value;
                OnPropertyChanged();
            }
        }

        private String lastLearned;
        public String LastLearned {
            get
            {
                return lastLearned;
            }
            set
            {
                lastLearned = value;
                OnPropertyChanged();
            } 
        }

        private String lastAnswer;
        public String LastAnswer {
            get
            {
                return lastAnswer;
            }
            set
            {
                lastAnswer = value;
                OnPropertyChanged();
            } 
        }

        private int wrongAnswer;
        public int WrongAnswer {
            get
            {
                return wrongAnswer;
            }
            set
            {
                wrongAnswer = value;
                OnPropertyChanged();
            }
        }

        private String lastWrongAnswer;
        public String LastWrongAnswer {
            get
            {
                return lastWrongAnswer;
            }
            set
            {
                lastWrongAnswer = value;
                OnPropertyChanged();
            } 
        }

        private int learningWrong;
        public int LearningWrong {
            get
            {
                return learningWrong;
            }
            set
            {
                learningWrong = value;
                OnPropertyChanged();
            }
        }

        private int currentBoxNumber;
        public int CurrentBoxNumber {
            get
            {
                return currentBoxNumber;
            }
            set
            {
                currentBoxNumber = value;
                OnPropertyChanged();
            }
        }

        private int actualBoxPassed;
        public int ActualBoxPassed {
            get
            {
                return actualBoxPassed;
            }
            set
            {
                actualBoxPassed = value;
                OnPropertyChanged();
            }
        }

        private int boxShift;
        public int BoxShift {
            get
            {
                return boxShift;
            }
            set
            {
                boxShift = value;
                OnPropertyChanged();
            }
        }

        private String dropintoCurrentBox;
        public String DropintoCurrentBox {
            get
            {
                return dropintoCurrentBox;
            }
            set
            {
                dropintoCurrentBox = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

        public StatisticWindowViewModel()
        {
            StatisticCollectionVM = new StatisticCollectionViewModel();
        }

        // Writing statistics for the Window
        public void MakeStatistic()
        {
            LearnedCard = StatisticAlgorithms.LearnedCard(
                StatisticCollectionVM);
            RightAnswer = StatisticAlgorithms.RightAnswer(
                StatisticCollectionVM);
            LastSucessfullAnswer = StatisticAlgorithms.LastSucessfullAnswer(
                StatisticCollectionVM);
            LearningSucess = StatisticAlgorithms.LearningSucess(
                StatisticCollectionVM);
            LastLearned = StatisticAlgorithms.LearnedLast(
                StatisticCollectionVM);
            LastAnswer = StatisticAlgorithms.LastAnswer(
                StatisticCollectionVM);
            WrongAnswer = StatisticAlgorithms.WrongAnswer(
                StatisticCollectionVM);
            LastWrongAnswer = StatisticAlgorithms.LastWrongAnswer(
                StatisticCollectionVM);
            LearningWrong = StatisticAlgorithms.LearningWrong(
                StatisticCollectionVM);
            CurrentBoxNumber = StatisticAlgorithms.CurrentBoxNumber(
                StatisticCollectionVM);
            ActualBoxPassed = StatisticAlgorithms.ActualBoxPassed(
                StatisticCollectionVM,CurrentBoxNumber);
            BoxShift = StatisticAlgorithms.BoxShift(
                StatisticCollectionVM, CurrentBoxNumber);
            DropintoCurrentBox = StatisticAlgorithms.DropintoCurrentBox(
                StatisticCollectionVM, CurrentBoxNumber,
                DateTime.Parse(LastLearned));
        }

    }
}
