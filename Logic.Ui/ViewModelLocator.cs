using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels;
using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Logic.Ui
{
    public class ViewModelLocator
    {

        public StatisticWindowViewModel StatisticWindowViewModel { get; }
        public ExportViewModel ExportViewModel { get; }
        public ImportViewModel ImportViewModel { get; }
        public MainWindowViewModel MainWindowViewModel { get; }
        public CardLearningViewModel CardLearningViewModel { get; }
        public CardWindowViewModel CardWindowViewModel { get; }
        public CategoryViewModel CategoryViewModel { get; }

        private StatisticCollectionViewModel statisticCollectionVM;

        public ViewModelLocator()
        {
            statisticCollectionVM = new StatisticCollectionViewModel();

            MainWindowViewModel = new MainWindowViewModel();
            StatisticWindowViewModel = new StatisticWindowViewModel(statisticCollectionVM);
            ExportViewModel = new ExportViewModel();
            ImportViewModel = new ImportViewModel();
            CardLearningViewModel = new CardLearningViewModel();
            CardWindowViewModel = new CardWindowViewModel();
            CategoryViewModel = new CategoryViewModel();

            // Statistic DummyDaten
            Statistic statistic = new Statistic();
            statistic.Timestamp = DateTime.Now;
            statistic.SuccessfullAnswer = true;
            statisticCollectionVM.Add(new StatisticViewModel(statistic));

            Statistic statistic2 = new Statistic();
            statistic2.Timestamp = DateTime.UtcNow;
            statistic2.SuccessfullAnswer = false;
            statisticCollectionVM.Add(new StatisticViewModel(statistic2));

        }
    }
}
