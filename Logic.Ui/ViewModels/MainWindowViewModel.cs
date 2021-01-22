using De.HsFlensburg.ClientApp101.Logic.Ui.MessageBusMessages;
using De.HsFlensburg.ClientApp101.Logic.Ui.Support;
using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using De.HsFlensburg.ClientApp101.Services.MessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    public class MainWindowViewModel
    {
        public RelayCommand OpenCardAddWindow { get; }
        public RelayCommand OpenCardLearningWindow { get; }
        public RelayCommand OpenImportWindow { get; }
        public RelayCommand OpenExportWindow { get; }
        public RelayCommand OpenStatisticsWindow { get; }
        public RelayCommand OpenCategoryWindow { get; }
        public RelayCommand OpenCategoryAddWindow { get; }
        public RelayCommand OpenImportForeignFormatWindow { get; }
        public RelayCommand OpenManageCardsWindow { get; }
        public RelayCommand SaveAndCloseAll { get; }

        private BoxCollectionViewModel myBoxCollectionViewModel { get; set; }
        private ModelViewModel myModelViewModel { get; set; }


        public MainWindowViewModel(ModelViewModel mvm)
        {
            OpenCardAddWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenCardAddMessage()));
            OpenCardLearningWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenCardLearningMessage()));
            OpenImportWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenImportMessage()));
            OpenExportWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenExportMessage()));
            OpenStatisticsWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenStatisticMessage()));
            OpenCategoryWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenCategoryMessage()));
            OpenCategoryAddWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenCategoryAddMessage()));
            OpenImportForeignFormatWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenImportForeignFormatMessage()));
            OpenManageCardsWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenManageCardMessage()));
            SaveAndCloseAll = new RelayCommand(() => Save());

            myBoxCollectionViewModel = mvm.BoxCollectionVM; 
            myModelViewModel = mvm;
        }


        private void Save()
        {
            //myModelViewModel.SaveCategorys();
            SaveCards.SaveCardsToFile(myBoxCollectionViewModel, myModelViewModel.myCategoryCollection);
        }


    }
}
