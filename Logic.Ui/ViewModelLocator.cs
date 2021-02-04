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
       // public CategoryViewModel CategoryViewModel { get; }
        public ImportForeignFormatViewModel ImportForeignFormatViewModel { get; }

        // for Statistics Dummydata
        private StatisticCollectionViewModel statisticCollectionVM;


        //<maha>
        public CardWindowViewModel cardWindowViewModel { get; }
        public ModelViewModel myModelViewModel { get; }
        //</maha>
        public ViewModelLocator()
        {
            // for Statistics Dummydata
            statisticCollectionVM = new StatisticCollectionViewModel();


            //StatisticWindowViewModel = new StatisticWindowViewModel(CardLearningViewModel.CardVM.StatisticCollection);
            //CardLearningViewModel.CardVM.StatisticCollection;



            //CardWindowViewModel = new CardWindowViewModel();
            //CategoryViewModel = new CategoryViewModel();


            // Statistic DummyData for testing statistics
            // normally is generated when a Card is learned in the learning algorithm
            StatisticViewModel statistic = new StatisticViewModel();
            statistic.Timestamp = new DateTime(2021, 01, 01, 7, 20, 22);
            statistic.SuccessfulAnswer = true;
            statistic.CurrentBoxNumber = Boxnumber.Box2;
            statisticCollectionVM.Add(statistic);

            StatisticViewModel statistic1 = new StatisticViewModel();
            statistic1.Timestamp = new DateTime(2021, 01, 03, 8, 45, 25);
            statistic1.SuccessfulAnswer = true;
            statistic1.CurrentBoxNumber = Boxnumber.Box3;
            statisticCollectionVM.Add(statistic1);

            StatisticViewModel statistic2 = new StatisticViewModel();
            statistic2.Timestamp = new DateTime(2021, 01, 15, 9, 36, 21);
            statistic2.SuccessfulAnswer = true;
            statistic2.CurrentBoxNumber = Boxnumber.Box4;
            statisticCollectionVM.Add(statistic2);

            StatisticViewModel statistic3 = new StatisticViewModel();
            statistic3.Timestamp = DateTime.Now;
            statistic3.SuccessfulAnswer = false;
            statistic3.CurrentBoxNumber = Boxnumber.Box4;
            statisticCollectionVM.Add(statistic3);



            // This mthod must be executed each time the statistics Window is opened
            //StatisticWindowViewModel.MakeStatistic();


            //<maha>
            //myBoxCollectionViewModel = new BoxCollectionViewModel();
            myModelViewModel = new ModelViewModel();
            
            CategoryManageViewModel = new CategoryManageViewModel(myModelViewModel.myCategoryCollection);
            ImportForeignFormatViewModel = new ImportForeignFormatViewModel(myModelViewModel.myCategoryCollection);
            ImportViewModel = new ImportViewModel(myModelViewModel.myCategoryCollection);
            ExportViewModel = new ExportViewModel(myModelViewModel.myCategoryCollection);
            MainWindowViewModel = new MainWindowViewModel(myModelViewModel);

            //myBoxCollectionViewModel = new BoxCollectionViewModel(myModelViewModel.BoxCollection);
            //myCategoryCollectionViewModel = new CategoryCollectionViewModel(myModelViewModel.CategoryCollection);

            BoxViewModel bvm1 = new BoxViewModel(new Box(Boxnumber.Box1));
            BoxViewModel bvm2 = new BoxViewModel(new Box(Boxnumber.Box2));
            BoxViewModel bvm3 = new BoxViewModel(new Box(Boxnumber.Box3));
            BoxViewModel bvm4 = new BoxViewModel(new Box(Boxnumber.Box4));
            BoxViewModel bvm5 = new BoxViewModel(new Box(Boxnumber.Box5));

            CardViewModel cvm1 = new CardViewModel(new Card("what is sky color?", "blue", Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\..\\..\\..\\Data\\myimages\\sky.jpeg"));
            CardViewModel cvm2 = new CardViewModel(new Card("what is year?", "2021", Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\..\\..\\..\\Data\\myimages\\year2021.jpg"));
            CardViewModel cvm3 = new CardViewModel(new Card("is mouse input or output device?", "input", Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\..\\..\\..\\Data\\myimages\\mouse.jpg"));

            cvm1.StatisticCollection = statisticCollectionVM;
            cvm2.StatisticCollection = statisticCollectionVM;
            cvm3.StatisticCollection = statisticCollectionVM;

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
            cardWindowViewModel = new CardWindowViewModel(myModelViewModel.BoxCollectionVM , myModelViewModel.myCategoryCollection);


            //marah
            CardLearningViewModel = new CardLearningViewModel(myModelViewModel.BoxCollectionVM, myModelViewModel.myCategoryCollection);

            StatisticWindowViewModel = new StatisticWindowViewModel(CardLearningViewModel.CardVM.StatisticCollection);
            StatisticWindowViewModel.MakeStatistic();


        }
    }
}
