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

        public StatisticWindowViewModel StatWindowVM { get; }
        public ExportViewModel ExportVM { get; }
        public ImportViewModel ImportVM { get; }
        public MainWindowViewModel MainWindowVM { get; }
        public CardLearningViewModel CardLearningVM { get; }
        public CategoryManageViewModel CategoryManageVM { get; }
        public ImportForeignFormatViewModel ImportFFVM {
            get; }
        public CardWindowViewModel CardWindowVM { get; }
        public ModelViewModel ModelVM { get; }

        public ViewModelLocator()
        {
            ModelVM = new ModelViewModel();
            Support.LoadCards.LoadAllCategorysFromFileSystem(
                ModelVM.BoxCollectionVM,
                ModelVM.myCatCollection);

            StatWindowVM = new StatisticWindowViewModel();
            MainWindowVM = new MainWindowViewModel();
            CategoryManageVM = new CategoryManageViewModel(
                ModelVM.myCatCollection);
            ImportVM = new ImportViewModel(
                ModelVM.myCatCollection);
            ExportVM = new ExportViewModel(
                ModelVM.myCatCollection);
            CardWindowVM = new CardWindowViewModel(
                ModelVM.BoxCollectionVM, 
                ModelVM.myCatCollection);
            ImportFFVM = new ImportForeignFormatViewModel(
                ModelVM.myCatCollection, 
                Boxnumber.Box1, 
                ModelVM.BoxCollectionVM);

            CardLearningVM = new CardLearningViewModel(
            ModelVM.BoxCollectionVM,
            ModelVM.myCatCollection,
            StatWindowVM); 
        }
    }
}
