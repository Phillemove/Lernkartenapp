using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using De.HsFlensburg.ClientApp101.Logic.Ui.Support;
using De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels;
using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public CategoryViewModel CategoryViewModel { get; }

        private StatisticCollectionViewModel statisticCollectionVM;


        //<maha>
        public CardWindowViewModel CardWindowViewModel { get; }
        public ModelViewModel myModelViewModel { get; }
        //</maha>
        public ViewModelLocator()
        {
            statisticCollectionVM = new StatisticCollectionViewModel();

            MainWindowViewModel = new MainWindowViewModel();
            StatisticWindowViewModel = new StatisticWindowViewModel(statisticCollectionVM);
            ExportViewModel = new ExportViewModel();
            ImportViewModel = new ImportViewModel();
            
            //CardWindowViewModel = new CardWindowViewModel();
            CategoryViewModel = new CategoryViewModel();


            // Statistic DummyData for testing statistics
            // normally is generated when a Card is learned in the learning algorithm
            Statistic statistic = new Statistic();
            statistic.Timestamp = DateTime.UtcNow;
            statistic.SuccessfullAnswer = true;
            statistic.CurrentBoxNumber = Boxnumber.Box2;
            statisticCollectionVM.Add(new StatisticViewModel(statistic));
          
            Statistic statistic1 = new Statistic();
            statistic1.Timestamp = new DateTime(2020, 12, 12, 7, 30, 32);
            statistic1.SuccessfullAnswer = true;
            statistic1.CurrentBoxNumber = Boxnumber.Box2;
            statisticCollectionVM.Add(new StatisticViewModel(statistic1));

            Statistic statistic2 = new Statistic();
            statistic2.Timestamp = DateTime.Now;
            statistic2.SuccessfullAnswer = true;
            statistic2.CurrentBoxNumber = Boxnumber.Box2;
            statisticCollectionVM.Add(new StatisticViewModel(statistic2));

            // This mthod must be executed each time the statistics Window is opened
            StatisticWindowViewModel.MakeStatistic();


            //<maha>
            //myBoxCollectionViewModel = new BoxCollectionViewModel();
            myModelViewModel = new ModelViewModel();
            
            CategoryManageViewModel = new CategoryManageViewModel(myModelViewModel.myCategoryCollection);

            //myBoxCollectionViewModel = new BoxCollectionViewModel(myModelViewModel.BoxCollection);
            //myCategoryCollectionViewModel = new CategoryCollectionViewModel(myModelViewModel.CategoryCollection);

            BoxViewModel bvm1 = new BoxViewModel(new Box(Boxnumber.Box1));
            BoxViewModel bvm2 = new BoxViewModel(new Box(Boxnumber.Box2));
            BoxViewModel bvm3 = new BoxViewModel(new Box(Boxnumber.Box3));
            BoxViewModel bvm4 = new BoxViewModel(new Box(Boxnumber.Box4));
            BoxViewModel bvm5 = new BoxViewModel(new Box(Boxnumber.Box5));

            CardViewModel cvm1 = new CardViewModel(new Card("what is sky color?","blue"));
            CardViewModel cvm2 = new CardViewModel(new Card("what is year?","2021"));
            CardViewModel cvm3 = new CardViewModel(new Card("is mouse input or output device?","input"));

            myModelViewModel.BoxCollectionVM.Add(bvm1);
            myModelViewModel.BoxCollectionVM.Add(bvm2);
            myModelViewModel.BoxCollectionVM.Add(bvm3);
            myModelViewModel.BoxCollectionVM.Add(bvm4);
            myModelViewModel.BoxCollectionVM.Add(bvm5);

            myModelViewModel.BoxCollectionVM.storeCard(cvm1, Boxnumber.Box1);
            myModelViewModel.BoxCollectionVM.storeCard(cvm2, Boxnumber.Box1);
            myModelViewModel.BoxCollectionVM.storeCard(cvm3, Boxnumber.Box1);
           
            myModelViewModel.BoxCollectionVM.storeCard(cvm2, Boxnumber.Box2);
            myModelViewModel.BoxCollectionVM.storeCard(cvm3, Boxnumber.Box2);
            myModelViewModel.BoxCollectionVM.storeCard(cvm3, Boxnumber.Box3);
            myModelViewModel.BoxCollectionVM.storeCard(cvm2, Boxnumber.Box4);
            myModelViewModel.BoxCollectionVM.storeCard(cvm1, Boxnumber.Box5);

            //</maha>
            CardWindowViewModel = new CardWindowViewModel(myModelViewModel.BoxCollectionVM , myModelViewModel.myCategoryCollection);
            

            //marah
            CardLearningViewModel = new CardLearningViewModel(myModelViewModel.BoxCollectionVM);

           

        }
    }
}
