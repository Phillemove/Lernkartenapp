﻿using De.HsFlensburg.ClientApp101.Logic.Ui.MessageBusMessages;
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
        public RelayCommand OpenImportForeignFormatWindow { get; }
        public RelayCommand OpenManageCardsWindow { get; }
        public MainWindowViewModel()
        {
            OpenCardAddWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenCardAddMessage()));
            OpenCardLearningWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenCardLearningMessage()));
            OpenImportWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenImportMessage()));
            OpenExportWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenExportMessage()));
            OpenStatisticsWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenStatisticMessage()));
            OpenCategoryWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenCategorieMessage()));
            OpenImportForeignFormatWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenImportForeignFormatMessage()));
            OpenManageCardsWindow = new RelayCommand(() => ServiceBus.Instance.Send(new OpenManageCardMessage()));
        }

        private void OpenCardAddWindowMethod()
        {
            ServiceBus.Instance.Send(new OpenCardAddMessage());
        }
    }
}