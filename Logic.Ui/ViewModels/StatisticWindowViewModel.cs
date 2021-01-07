using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
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
        public StatisticWindowViewModel(StatisticCollectionViewModel statisticCollectionVM)
        {
            StatisticCollectionVM = statisticCollectionVM;
        }
    }
}
