using System;
using De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels;
using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.Support
{
    public class StatisticAlgorithms
    {
        /*
         * All DateTime Methods return the time convert to string. This allows 
         * to display the DateTime in a local format (german format).
         */

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
        public static String LastSucessfullAnswer(StatisticCollectionViewModel collection)
        {
            DateTime time = new DateTime();
            foreach(StatisticViewModel statistic in collection)
            {
                if (statistic.SuccessfulAnswer == true)
                {   
                    if (DateTime.Compare(time, statistic.Timestamp) < 0)
                    {
                        time = statistic.Timestamp;
                    }
                    
                }
            }
            if (time == default(DateTime))
            {
                return "Nie";
            }
            else
            {
                return time.ToString();
            }
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
        public static String LearnedLast(StatisticCollectionViewModel collection)
        {
            DateTime time = new DateTime();
            foreach(StatisticViewModel statistic in collection)
            {
                if (DateTime.Compare(time, statistic.Timestamp) < 0)
                {
                    time = statistic.Timestamp;
                }
            }
            if (time == default(DateTime))
            {
                return "Nie";
            }
            else
            {
                return time.ToString();
            }
        }

        // Last Answer
        public static String LastAnswer(StatisticCollectionViewModel collection)
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
            if (lastanswer)
            {
                return "Richtig";
            } else
            {
                return "Falsch";
            }
            
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
        public static String LastWrongAnswer(StatisticCollectionViewModel collection)
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
            if (time == default(DateTime))
            {
                return "Nie";
            } else
            {
                return time.ToString();
            }
                  
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

        // Current Box
        public static int CurrentBoxNumber(StatisticCollectionViewModel collection)
        {
            int currentBoxNumber = 0;
            DateTime time = new DateTime();
            foreach (StatisticViewModel statistic in collection)
            {
                if (DateTime.Compare(time, statistic.Timestamp) < 0)
                {
                    time = statistic.Timestamp;
                    currentBoxNumber = (int)statistic.CurrentBoxNumber;
                }
            }
            return currentBoxNumber;
        }

        // How often was a Card in a Box
        public static int ActualBoxPassed(StatisticCollectionViewModel collection, int currentBoxNumber)
        {
            int counter = 0;

            foreach(StatisticViewModel statistic in collection)
            {
                if((int)statistic.CurrentBoxNumber == currentBoxNumber)
                {
                    counter++;
                }
            }
            return counter;
        }

        // How many Box shifts has the Card
        public static int BoxShift(StatisticCollectionViewModel collection, int currentBoxNumber)
        {
            int counter = 0;
            foreach(StatisticViewModel statistic in collection)
            {   
                if((int)statistic.CurrentBoxNumber != currentBoxNumber)
                {
                    counter++;
                    currentBoxNumber = (int)statistic.CurrentBoxNumber;  
                }
            }
            /* currentBoxNumber can be from an Statistic Object somwhere in the Collection.
               This caused, that the currentBox is counted twice here. 
               To handle this, it is nessescary to decrease counter with 1 
               at the return statement. If counter is 0 this caused to return a -1, the return
               Statement will be 0
             */
            if (counter == 0)
            {
                return 0;
            } else
            {
                return counter - 1;
            }
            
        }

        // When was the Card dropped into the current Box
        public static String DropintoCurrentBox(StatisticCollectionViewModel collection, int currentBoxNumber, DateTime currentTimestamp)
        {
            DateTime time = currentTimestamp;
            foreach (StatisticViewModel statistic in collection)
            {
                if ((int)statistic.CurrentBoxNumber == currentBoxNumber && DateTime.Compare(time, statistic.Timestamp) > 0)
                {
                    time = statistic.Timestamp;
                }

            }
            return time.ToString();
        }

    }
}
