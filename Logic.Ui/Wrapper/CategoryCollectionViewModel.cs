using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    public class CategoryCollectionViewModel : ObservableCollection<CategoryViewModel>
    {

        public CategoryCollection categoryCollection;
        private bool syncDisabled;

        public CategoryCollectionViewModel(CategoryCollection cc)
        {
            categoryCollection = cc;
            this.CollectionChanged += ViewModelCollectionChanged;
            categoryCollection.CollectionChanged += ModelCollectionChanged;
        }

        private void ViewModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (syncDisabled) return;
            syncDisabled = true;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var category in e.NewItems.OfType<CategoryViewModel>().Select(v => v.category).OfType<Category>())
                        categoryCollection.Add(category);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var category in e.OldItems.OfType<CategoryViewModel>().Select(v => v.category).OfType<Category>())
                        categoryCollection.Remove(category);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    categoryCollection.Clear();
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
                    foreach (var category in e.NewItems.OfType<Category>())
                        Add(new CategoryViewModel(category));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var category in e.OldItems.OfType<Category>())
                        Remove(GetViewModelOfModel(category));
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Clear();
                    break;
            }
            syncDisabled = false;
        }

        private CategoryViewModel GetViewModelOfModel(Category category)
        {
            foreach (CategoryViewModel cvm in this)
            {
                if (cvm.category.Equals(category)) return cvm;

            }
            return null;
        }




    }
}
