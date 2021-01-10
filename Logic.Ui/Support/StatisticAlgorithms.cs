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
        // Anzahl aller richtig beantworteter Fragen
        public static int rightAnswer(StatisticCollectionViewModel collection)
        {
            int counter = 0;
            foreach (StatisticViewModel statistic in collection)
            {
                if (statistic.SuccessfulAnswer == true)
                {   
                    counter++;
                    Console.WriteLine(counter);
                }
            }
            return counter;
        }

        // Anzahl wie oft die Karte überhaupt gelernt wurde
        public static int learnedCard(StatisticCollectionViewModel collection)
        {
            int counter = 0;
            foreach (StatisticViewModel statistic in collection)
            {
                counter++;
            }
            return counter;
        }

        // Wann wurde sie zuletzt richtig Beantwortet
        public static DateTime lastSucessfullAnswer(StatisticCollectionViewModel collection)
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

        // Wir oft hintereinander richtig gelernt
        public static int learningSucess(StatisticCollectionViewModel collection)
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

    }
}
