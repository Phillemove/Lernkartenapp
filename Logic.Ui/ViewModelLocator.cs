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
        private BoxCollectionViewModel myBoxCollectionViewModel;

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

            
            // maha
            myBoxCollectionViewModel = new BoxCollectionViewModel();
            // myModelViewModel = new ModelViewModel();
           // myBoxCollectionViewModel = new BoxCollectionViewModel(myModelViewModel.BoxCollection);
            //myCategoryCollectionViewModel = new CategoryCollectionViewModel(myModelViewModel.CategoryCollection);
            //
            // BoxViewModel bvm1 = new BoxViewModel(new Box(Boxnumber.Box1));
            //  BoxViewModel bvm2 = new BoxViewModel(new Box(Boxnumber.Box2));
            //  BoxViewModel bvm3 = new BoxViewModel(new Box(Boxnumber.Box3));
            //  BoxViewModel bvm4 = new BoxViewModel(new Box(Boxnumber.Box4));
            // BoxViewModel bvm5 = new BoxViewModel(new Box(Boxnumber.Box5));

            // CardViewModel cvm1 = new CardViewModel(new Card(new Category("Math")));
            //  CardViewModel cvm2 = new CardViewModel(new Card(new Category("computer")));
            //  CardViewModel cvm3 = new CardViewModel(new Card(new Category("IT")));

            //  myModelViewModel.BoxCollection.Add(bvm1);
            //  myModelViewModel.BoxCollection.Add(bvm2);
            //   myModelViewModel.BoxCollection.Add(bvm3);
            // myModelViewModel.BoxCollection.Add(bvm4);
            //  myModelViewModel.BoxCollection.Add(bvm5);

            //  myModelViewModel.BoxCollection.storeCard(cvm1, Boxnumber.Box1);
            //  myModelViewModel.BoxCollection.storeCard(cvm2, Boxnumber.Box2);
            //   myModelViewModel.BoxCollection.storeCard(cvm3, Boxnumber.Box2);
            //  myModelViewModel.BoxCollection.storeCard(cvm1, Boxnumber.Box3);


            StatisticWindowViewModel.MakeStatistic();

        }
    }
}
