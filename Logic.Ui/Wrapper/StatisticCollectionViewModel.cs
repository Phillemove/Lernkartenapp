using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using De.HsFlensburg.ClientApp101.Logic.Ui.Support;
using De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper
{
    public class StatisticCollectionViewModel : ObservableCollection<StatisticViewModel>
    {
        public StatisticCollection statisticCollection;
        private bool syncDisabled;

        public StatisticCollectionViewModel()
        {
            statisticCollection = new StatisticCollection();
            this.CollectionChanged += ViewModelCollectionChanged;
            statisticCollection.CollectionChanged += ModelCollectionChanged;
        }

        private void ViewModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (syncDisabled) return;
            syncDisabled = true;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var client in e.NewItems.OfType<StatisticViewModel>().Select(v => v.statistic).OfType<Statistic>())
                        statisticCollection.Add(client);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var client in e.OldItems.OfType<StatisticViewModel>().Select(v => v.statistic).OfType<Statistic>())
                        statisticCollection.Remove(client);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    statisticCollection.Clear();
                    break;
            }
            syncDisabled = false;  
        }

        private void ModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (syncDisabled) return;
            syncDisabled = true;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var statistic in e.NewItems.OfType<Statistic>())
                        Add(new StatisticViewModel(statistic));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var statistic in e.OldItems.OfType<Statistic>())
                        Remove(GetViewModelOfModel(statistic));
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Clear();
                    break;
            }
            syncDisabled = false;
        }

        private StatisticViewModel GetViewModelOfModel(Statistic statistic)
        {
            foreach (StatisticViewModel svm in this)
            {
                if (svm.statistic.Equals(statistic)) return svm;

            }
            return null;
        }
    }
}
