using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects
{

    public class Box : Queue<Card> , INotifyCollectionChanged
    {
        Boxnumber bn;
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public Box()
        {
            this.bn = Boxnumber.None;
        }
        public Box(Boxnumber bn)
        {
            this.bn = bn;
        }
        public Boxnumber Bn
        {
            get
            {
                return bn;
            }
            set
            {
                bn = value;
            }
        }
        public void Add(Card card)
        {
            base.Enqueue(card);
            if (this.CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
        }
        public Card Remove()
        {
            Card item = base.Dequeue();
            if (this.CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
            return item;
            //if (this.Any())
            //{
            //    Card c = this.Dequeue();
            //    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, c));
            //    return c;
            //}
            //else
            //{
            //    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
            //    return null;
            //}
        }
        public new void Clear()
        {
            base.Clear();
            if (this.CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        //private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (syncDisabled) return;
        //    syncDisabled = true;
        //    switch (e.Action)
        //    {
        //        case NotifyCollectionChangedAction.Add:
        //            foreach (var card in e.NewItems.OfType<Card>())
        //                this.Add(card);
        //            break;
        //        case NotifyCollectionChangedAction.Remove:
        //            foreach (var card in e.NewItems.OfType<Card>())
        //                this.Remove();
        //            break;
        //        case NotifyCollectionChangedAction.Reset:
        //            this.Clear();
        //            break;
        //    }
        //    syncDisabled = false;
        //}
        //public virtual event NotifyCollectionChangedEventHandler CollectionChanged;


        //protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        //{
        //    this.RaiseCollectionChanged(e);
        //}

        //protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        //{
        //    this.RaisePropertyChanged(e);
        //}


        //protected virtual event PropertyChangedEventHandler PropertyChanged;


        //private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
        //{
        //    if (this.CollectionChanged != null)
        //        this.CollectionChanged(this, e);
        //}

        //private void RaisePropertyChanged(PropertyChangedEventArgs e)
        //{
        //    if (this.PropertyChanged != null)
        //        this.PropertyChanged(this, e);
        //}


        //event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        //{
        //    add { this.PropertyChanged += value; }
        //    remove { this.PropertyChanged -= value; }
        //}
    }
}

