using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper
{

    public class BoxCollectionViewModel : ObservableCollection<BoxViewModel>
    {
        public BoxCollection boxCollection;
        private bool syncDisabled;
        public BoxCollectionViewModel()
        {
            boxCollection = new BoxCollection();
            this.CollectionChanged += ViewModelCollectionChanged;
            boxCollection.CollectionChanged += ModelCollectionChanged;
        }
       public BoxCollectionViewModel(BoxCollection bc)
        {
            boxCollection = bc;
        this.CollectionChanged += ViewModelCollectionChanged;
          boxCollection.CollectionChanged += ModelCollectionChanged;
          }

        public CardViewModel giveCard(Boxnumber bn)
        {
            foreach (BoxViewModel boxvm in this)
            {
                if (boxvm.Bn == bn)
                {
                    return boxvm.remove();
                }
            }
            return null;
        }
        public void storeCard(CardViewModel cardvm, Boxnumber bn)
        {
            foreach (BoxViewModel boxvm in this)
            {
                if (boxvm.Bn == bn)
                {
                    boxvm.add(cardvm);
                }
            }
            foreach (Box box in this.boxCollection)
            {
                if (box.Bn == bn)
                {
                    box.add(cardvm.card);
                }
            }
        }
        private void ViewModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (syncDisabled) return;
            syncDisabled = true;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var box in e.NewItems.OfType<BoxViewModel>().Select(v => v.box).OfType<Box>())
                        boxCollection.Add(box);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var box in e.NewItems.OfType<BoxViewModel>().Select(v => v.box).OfType<Box>())
                        boxCollection.Remove(box);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    boxCollection.Clear();
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
                    foreach (var box in e.NewItems.OfType<Box>())
                        this.Add(new BoxViewModel(box));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var box in e.NewItems.OfType<Box>())
                        this.Remove(GetViewModelOfModel(box));
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Clear();
                    break;
            }
            syncDisabled = false;
        }
        private BoxViewModel GetViewModelOfModel(Box box)
        {
            foreach (BoxViewModel bvm in this)
            {
                if (bvm.box.Equals(box)) return bvm;
            }
            return null;
        }

    }

    }

