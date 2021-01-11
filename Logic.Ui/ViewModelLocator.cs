using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using De.HsFlensburg.ClientApp101.Logic.Ui.Support;
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
        private StatisticCollectionViewModel statisticCollectionCard1;
        private StatisticCollectionViewModel statisticCollectionCard2;

        public ViewModelLocator()
        {
            statisticCollectionVM = new StatisticCollectionViewModel();
            statisticCollectionCard1 = new StatisticCollectionViewModel();
            statisticCollectionCard2 = new StatisticCollectionViewModel();

            MainWindowViewModel = new MainWindowViewModel();
            StatisticWindowViewModel = new StatisticWindowViewModel(statisticCollectionVM);
            ExportViewModel = new ExportViewModel();
            ImportViewModel = new ImportViewModel();
            CardLearningViewModel = new CardLearningViewModel();
            CardWindowViewModel = new CardWindowViewModel();
            CategoryViewModel = new CategoryViewModel();


            // Statistic DummyDaten
            Statistic statistic = new Statistic();
            statistic.Timestamp = DateTime.UtcNow;
            statistic.SuccessfullAnswer = true;
            statisticCollectionVM.Add(new StatisticViewModel(statistic));
          
            Statistic statistic1 = new Statistic();
            statistic1.Timestamp = new DateTime(2020, 12, 12, 7, 30, 32);
            statistic1.SuccessfullAnswer = true;
            statisticCollectionVM.Add(new StatisticViewModel(statistic1));

            Statistic statistic2 = new Statistic();
            statistic2.Timestamp = DateTime.Now;
            statistic2.SuccessfullAnswer = false;
            statisticCollectionVM.Add(new StatisticViewModel(statistic2));

            StatisticWindowViewModel.MakeStatistic();
        }
    }
}
