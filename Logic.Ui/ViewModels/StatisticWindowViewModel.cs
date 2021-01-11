using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using De.HsFlensburg.ClientApp101.Logic.Ui.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    public class StatisticWindowViewModel
    {
        public StatisticCollectionViewModel StatisticCollectionVM { get; set; }
        public int LearnedCard { get; set; }
        public int RightAnswer { get; set; }
        public DateTime LastSucessfullAnswer { get; set; }
        public int LearningSucess { get; set; }
        public DateTime LastLearned { get; set; }
        public bool LastAnswer { get; set; }
        public int WrongAnswer { get; set; }
        public DateTime LastWrongAnswer { get; set; }
        public int LearningWrong { get; set; }

        public StatisticWindowViewModel(StatisticCollectionViewModel statisticCollectionVM)
        {
            StatisticCollectionVM = statisticCollectionVM;
        }

        // Writing statistics for the Window
        public void MakeStatistic()
        {
            LearnedCard = StatisticAlgorithms.LearnedCard(StatisticCollectionVM);
            RightAnswer = StatisticAlgorithms.RightAnswer(StatisticCollectionVM);
            LastSucessfullAnswer = StatisticAlgorithms.LastSucessfullAnswer(StatisticCollectionVM);
            LearningSucess = StatisticAlgorithms.LearningSucess(StatisticCollectionVM);
            LastLearned = StatisticAlgorithms.LearnedLast(StatisticCollectionVM);
            LastAnswer = StatisticAlgorithms.LastAnswer(StatisticCollectionVM);
            WrongAnswer = StatisticAlgorithms.WrongAnswer(StatisticCollectionVM);
            LastWrongAnswer = StatisticAlgorithms.LastWrongAnswer(StatisticCollectionVM);
            LearningWrong = StatisticAlgorithms.LearningWrong(StatisticCollectionVM);
        }

    }
}
