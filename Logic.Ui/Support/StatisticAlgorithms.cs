using System;
using De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels;
using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.Support
{
    public class StatisticAlgorithms
    {
        // How often is this card learned successfull
        public static int RightAnswer(StatisticCollectionViewModel collection)
        {
            int counter = 0;
            foreach (StatisticViewModel statistic in collection)
            {
                if (statistic.SuccessfulAnswer == true)
                {   
                    counter++;
                }
            }
            return counter;
        }

        // How often is this card learned
        public static int LearnedCard(StatisticCollectionViewModel collection)
        {
            int counter = 0;
            foreach (StatisticViewModel statistic in collection)
            {
                counter++;
            }
            return counter;
        }

        // On which Date is a Card learned correct the last time
        public static DateTime LastSucessfullAnswer(StatisticCollectionViewModel collection)
        {
            DateTime time = new DateTime();
            foreach(StatisticViewModel statistic in collection)
            {
                if (statistic.SuccessfulAnswer == true)
                {   
                    if (DateTime.Compare(time, statistic.Timestamp)<0)
                    {
                        time = statistic.Timestamp;
                    }
                    
                }
            }
            return time;
        }

        // How often in a row is a Card learned successfull
        public static int LearningSucess(StatisticCollectionViewModel collection)
        {
            int counter = 0;
           
            for(int i = 0; i < collection.Count; i++)
            {
                
                if (i+1 < collection.Count && collection[i].SuccessfulAnswer == false && collection[i+1].SuccessfulAnswer == true)
                {
                    
                } else if (i + 1 < collection.Count && collection[i].SuccessfulAnswer == true && collection[i+1].SuccessfulAnswer == true)
                {
                    counter++;
                } else if (i + 1 < collection.Count && collection[i].SuccessfulAnswer == true && collection[i+1].SuccessfulAnswer == false)
                {
                    counter++;
                    break;
                } else if (i + 1 >= collection.Count && collection[i].SuccessfulAnswer == true)
                {
                    counter++;
                    break;
                } 
            }
            return counter;
        }

        // When is the Card learned last
        public static DateTime LearnedLast(StatisticCollectionViewModel collection)
        {
            DateTime time = new DateTime();
            foreach(StatisticViewModel statistic in collection)
            {
                if (DateTime.Compare(time, statistic.Timestamp) < 0)
                {
                    time = statistic.Timestamp;
                }
            }
            return time;
        }

        // Last Answer
        public static bool LastAnswer(StatisticCollectionViewModel collection)
        {
            bool lastanswer = false;
            DateTime time = new DateTime();
            foreach (StatisticViewModel statistic in collection)
            {
                if (DateTime.Compare(time, statistic.Timestamp) < 0)
                {
                    time = statistic.Timestamp;
                    lastanswer = statistic.SuccessfulAnswer;
                }
            }
            return lastanswer;
        }

        // How often ist this card learned unsuccessfull
        public static int WrongAnswer(StatisticCollectionViewModel collection)
        {
            int counter = 0;
            foreach (StatisticViewModel statistic in collection)
            {
                if (statistic.SuccessfulAnswer == false)
                {
                    counter++;
                }
            }
            return counter;
        }

        // On which Date is a Card learned wrong the last time
        public static DateTime LastWrongAnswer(StatisticCollectionViewModel collection)
        {
            DateTime time = new DateTime();
            foreach (StatisticViewModel statistic in collection)
            {
                if (statistic.SuccessfulAnswer == false)
                {
                    if (DateTime.Compare(time, statistic.Timestamp) < 0)
                    {
                        time = statistic.Timestamp;
                    }

                }
            }
            return time;
        }

        // How often in a row is a Card learned Wrong
        public static int LearningWrong(StatisticCollectionViewModel collection)
        {
            int counter = 0;

            for (int i = 0; i < collection.Count; i++)
            {

                if (i + 1 < collection.Count && collection[i].SuccessfulAnswer == true && collection[i + 1].SuccessfulAnswer == false)
                {

                }
                else if (i + 1 < collection.Count && collection[i].SuccessfulAnswer == false && collection[i + 1].SuccessfulAnswer == false)
                {
                    counter++;
                }
                else if (i + 1 < collection.Count && collection[i].SuccessfulAnswer == false && collection[i + 1].SuccessfulAnswer == true)
                {
                    counter++;
                    break;
                }
                else if (i + 1 >= collection.Count && collection[i].SuccessfulAnswer == false)
                {
                    counter++;
                    break;
                }
            }
            return counter;
        }

    }
}
