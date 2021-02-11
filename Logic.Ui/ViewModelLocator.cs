using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using De.HsFlensburg.ClientApp101.Logic.Ui.Support;
using De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels;
using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace De.HsFlensburg.ClientApp101.Logic.Ui
{
    public class ViewModelLocator
    {

        public StatisticWindowViewModel StatisticWindowViewModel { get; }
        public ExportViewModel ExportViewModel { get; }
        public ImportViewModel ImportViewModel { get; }
        public MainWindowViewModel MainWindowViewModel { get; }
        public CardLearningViewModel CardLearningViewModel { get; }
        public CategoryManageViewModel CategoryManageViewModel { get; }
        public ImportForeignFormatViewModel ImportForeignFormatViewModel {
            get; }
        public CardWindowViewModel CardWindowViewModel { get; }
        public ModelViewModel MyModelViewModel { get; }

        public ViewModelLocator()
        {
            MyModelViewModel = new ModelViewModel();

            StatisticWindowViewModel = new StatisticWindowViewModel();
            MainWindowViewModel = new MainWindowViewModel(MyModelViewModel);
            CategoryManageViewModel = new CategoryManageViewModel(
                MyModelViewModel.myCategoryCollection);
            ImportViewModel = new ImportViewModel(
                MyModelViewModel.myCategoryCollection);
            ExportViewModel = new ExportViewModel(
                MyModelViewModel.myCategoryCollection);
            CardWindowViewModel = new CardWindowViewModel(
                MyModelViewModel.BoxCollectionVM, 
                MyModelViewModel.myCategoryCollection);
            ImportForeignFormatViewModel = new ImportForeignFormatViewModel(
                MyModelViewModel.myCategoryCollection, 
                Boxnumber.Box1, 
                MyModelViewModel.BoxCollectionVM);

            CardLearningViewModel = new CardLearningViewModel(
            MyModelViewModel.BoxCollectionVM,
            MyModelViewModel.myCategoryCollection,
            StatisticWindowViewModel);

            Support.LoadCards.LoadAllCategorysFromFileSystem(MyModelViewModel.BoxCollectionVM, MyModelViewModel.myCategoryCollection);
           
        }
    }
}
