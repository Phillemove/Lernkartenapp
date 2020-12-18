using De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Logic.Ui
{
    public class ViewModelLocator
    {

        public StatisticViewModel StatisticViewModel { get; }
        public ExportViewModel ExportViewModel { get; }
        public ImportViewModel ImportViewModel { get; }
        public MainWindowViewModel MainWindowViewModel { get; }
        public CardLearningViewModel CardLearningViewModel { get; }
        public CardViewModel CardViewModel { get; }
        public CategoryViewModel CategoryViewModel { get; }

        public ViewModelLocator()
        {
            MainWindowViewModel = new MainWindowViewModel();
            StatisticViewModel = new StatisticViewModel();
            ExportViewModel = new ExportViewModel();
            ImportViewModel = new ImportViewModel();
            CardLearningViewModel = new CardLearningViewModel();
            CardViewModel = new CardViewModel();
            CategoryViewModel = new CategoryViewModel();

        }
    }
}
