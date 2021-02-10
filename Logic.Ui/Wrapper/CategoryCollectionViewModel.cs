using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    public class CategoryCollectionViewModel : 
        ObservableCollection<CategoryViewModel>
    {
        public CategoryCollection categoryCollection;
        private bool syncDisabled;
        // Path to Category File - where to look for it/ save it
        private const string CategoryFile = @"..\..\..\Data\Categorys.xml";


        public CategoryCollectionViewModel()
        {
            categoryCollection = new CategoryCollection();
            this.CollectionChanged += ViewModelCollectionChanged;
            categoryCollection.CollectionChanged += ModelCollectionChanged;
            LoadCategorys();
        }

        // Loads Categorys from CategoryFile if it exists
        private void LoadCategorys()
        {
            if (File.Exists(CategoryFile))
            {
                // File exists - load Category Names 
                var reader = System.Xml.XmlReader.Create(CategoryFile);
                while (reader.ReadToFollowing("Category"))
                {
                    // Read Caegorys and create Category 
                    // Object in the Collection
                    this.Add(new CategoryViewModel(new Category(
                        reader.ReadElementContentAsString())));
                }
            }
        }

        // Saves Categorys to CategoryFile
        public void SaveCategorys()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            //settings.IndentChars = "\n";
            XmlWriter writer = XmlWriter.Create(CategoryFile,settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("CategoryList");
            foreach (var category in this)
            {
                if (category.Name != "")
                {
                    writer.WriteStartElement("Category");
                    writer.WriteString(category.Name);
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndDocument();
            writer.Close();
        }

        private void ViewModelCollectionChanged(object sender, 
            NotifyCollectionChangedEventArgs e)
        {
            if (syncDisabled) return;
            syncDisabled = true;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var category in e.NewItems.OfType
                        <CategoryViewModel>
                        ().Select(v => v.category).OfType<Category>())
                        categoryCollection.Add(category);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var category in e.OldItems.OfType
                        <CategoryViewModel>().Select
                        (v => v.category).OfType<Category>())
                        categoryCollection.Remove(category);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    categoryCollection.Clear();
                    break;
            }
            syncDisabled = false;
        }

        private void ModelCollectionChanged(object sender, 
            NotifyCollectionChangedEventArgs e)
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
