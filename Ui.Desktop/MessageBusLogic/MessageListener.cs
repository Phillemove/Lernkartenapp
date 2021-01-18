using De.HsFlensburg.ClientApp101.Services.MessageBus;
using De.HsFlensburg.ClientApp101.Logic.Ui.MessageBusMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Ui.Desktop.MessageBusLogic
{
   
   public class MessageListener
    {
         public bool BindableProperty => true;
            
        public MessageListener()
        {
            InitMessenger();
        }

        private void InitMessenger()
        {
            ServiceBus.Instance.Register<OpenCardAddMessage>(this, delegate ()
            {
                CardAdd cardAdd = new CardAdd();
                cardAdd.ShowDialog();
            });
            ServiceBus.Instance.Register<OpenManageCardMessage>(this, delegate ()
            {
                ManageCard manageCard = new ManageCard();
                manageCard.ShowDialog();
            });
            ServiceBus.Instance.Register<OpenImportMessage>(this, delegate ()
            {
                Import import = new Import();
                import.ShowDialog();
            });
            ServiceBus.Instance.Register<OpenStatisticMessage>(this, delegate ()
            {
                StatisticWindow statistic = new StatisticWindow();
                statistic.ShowDialog();
            });
            ServiceBus.Instance.Register<OpenCategoryMessage>(this, delegate ()
            {
                CategoryManage catManage = new CategoryManage();
                catManage.ShowDialog();
            });
            ServiceBus.Instance.Register<OpenCategoryAddMessage>(this, delegate ()
            {
                CategoryAdd catAdd = new CategoryAdd();
                catAdd.ShowDialog();
            });
            ServiceBus.Instance.Register<OpenExportMessage>(this, delegate ()
            {
                Export export = new Export();
                export.ShowDialog();
            });
            ServiceBus.Instance.Register<OpenImportForeignFormatMessage>(this, delegate ()
            {
                ImportForeignFormat importForeignFormat = new ImportForeignFormat();
                importForeignFormat.ShowDialog();
            });
            ServiceBus.Instance.Register<OpenCardLearningMessage>(this, delegate ()
            {
                CardLearning cardLearning = new CardLearning();
                cardLearning.ShowDialog();
            });
        }

    }

   
}
