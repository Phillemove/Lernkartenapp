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
        public static int rightAnswer(StatisticCollectionViewModel collection)
        {
            int counter = 0;
            foreach (StatisticViewModel statistic in collection)
            {
                Console.WriteLine("Object");
                if (statistic.SuccessfulAnswer == true)
                {   
                    counter++;
                    Console.WriteLine(counter);
                }
            }
            return counter;
        }
    }
}
