using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper
{
    public class BoxViewModel : Queue<CardViewModel> //, INotifyCollectionChanged
    {
        public Box box;
        //public event NotifyCollectionChangedEventHandler CollectionChanged;
        public BoxViewModel()
            {
                this.box = new Box();
            }
            public BoxViewModel(Box box)
            {
                this.box = box;
            }
            public Boxnumber Bn
            {
                get
                {
                    return box.Bn;
                }
                set
                {
                    box.Bn = value;
                }
            }

        
            public void Add(CardViewModel cardvm)
            {
                box.Add(cardvm.card);
                base.Enqueue(cardvm);
                //if (this.CollectionChanged != null)
                //  CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
            }
            public CardViewModel Remove()
            {
            //CardViewModel item = base.Dequeue();
            //if (this.CollectionChanged != null)
            //   CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
            //return item;
            box.Remove();
            if (this.Any())
            {
                CardViewModel cvm = this.Dequeue();
                //this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, cvm));
                return cvm;
            }
            else
            {
                //this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
                return null;
            }
        }

        public new void Clear()
        {
            box.Clear();
            base.Clear();
            //if (this.CollectionChanged != null)
            //    CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        //=================================================================
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

