using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper
{
    public class StatisticCollectionViewModel: ObservableCollection<StatisticViewModel>
    {
        public StatisticCollection statisticCollection;

        public StatisticCollectionViewModel()
        {
            statisticCollection = new StatisticCollection();
        }
    }
}
