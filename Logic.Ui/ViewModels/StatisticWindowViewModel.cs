﻿using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using De.HsFlensburg.ClientApp101.Logic.Ui.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    /*
     * ViewModel for the UI-Element StatisticWindow. Method fill the Properties
     * with Data, that they can be displayed in the UI
     */
    public class StatisticWindowViewModel
    {
        public StatisticCollectionViewModel StatisticCollectionVM { get; set; }
        public int LearnedCard { get; set; }
        public int RightAnswer { get; set; }
        public String LastSucessfullAnswer { get; set; }
        public int LearningSucess { get; set; }
        public String LastLearned { get; set; }
        public String LastAnswer { get; set; }
        public int WrongAnswer { get; set; }
        public String LastWrongAnswer { get; set; }
        public int LearningWrong { get; set; }
        public int CurrentBoxNumber { get; set; }
        public int ActualBoxPassed { get; set; }
        public int BoxShift { get; set; }
        public String DropintoCurrentBox { get; set; }

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
            CurrentBoxNumber = StatisticAlgorithms.CurrentBoxNumber(StatisticCollectionVM);
            ActualBoxPassed = StatisticAlgorithms.ActualBoxPassed(StatisticCollectionVM,CurrentBoxNumber);
            BoxShift = StatisticAlgorithms.BoxShift(StatisticCollectionVM, CurrentBoxNumber);
            DropintoCurrentBox = StatisticAlgorithms.DropintoCurrentBox(StatisticCollectionVM, CurrentBoxNumber, DateTime.Parse(LastLearned));
        }

    }
}
