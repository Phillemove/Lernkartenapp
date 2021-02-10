using De.HsFlensburg.ClientApp101.Logic.Ui.MessageBusMessages;
using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using De.HsFlensburg.ClientApp101.Services.MessageBus;
using System.Windows;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    public class MainWindowViewModel
    {
        public RelayCommand OpenCardAddWindow { get; }
        public RelayCommand OpenCardLearningWindow { get; }
        public RelayCommand OpenImportWindow { get; }
        public RelayCommand OpenExportWindow { get; }
        public RelayCommand OpenCategoryWindow { get; }
        public RelayCommand OpenCategoryAddWindow { get; }
        public RelayCommand OpenImportForeignFormatWindow { get; }
        public RelayCommand OpenManageCardsWindow { get; }
        public RelayCommand SaveAndCloseAll { get; }

        private BoxCollectionViewModel MyBoxCollectionViewModel { get; set; }
        private ModelViewModel MyModelViewModel { get; set; }


        public MainWindowViewModel(ModelViewModel mvm)
        {
            OpenCardAddWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenCardAddMessage()));
            OpenCardLearningWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenCardLearningMessage()));
            OpenImportWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenImportMessage()));
            OpenExportWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenExportMessage()));
            OpenCategoryWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenCategoryMessage()));
            OpenImportForeignFormatWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenImportForeignFormatMessage()));
            OpenManageCardsWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenManageCardMessage()));
            SaveAndCloseAll = new RelayCommand(param => Save(param));

            MyBoxCollectionViewModel = mvm.BoxCollectionVM; 
            MyModelViewModel = mvm;
        }


        private void Save(object param)
        {
            Window window = (Window)param;
            window.Close();
            //myModelViewModel.SaveCategorys();
            //SaveCards.SaveBoxCollectionsToFilesystem(myBoxCollectionViewModel, myModelViewModel.myCategoryCollection);
        }


    }
}
