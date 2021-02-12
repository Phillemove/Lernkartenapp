﻿using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
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
        public CategoryCollection catCollection;
        private bool syncDisabled;
        


        public CategoryCollectionViewModel()
        {
            catCollection = new CategoryCollection();
            this.CollectionChanged += ViewModelCollectionChanged;
            catCollection.CollectionChanged += ModelCollectionChanged;
            LoadCategorys();
        }

        // Loads Categorys from CategoryFile if it exists
        private void LoadCategorys()
        {
            if (File.Exists(ModelViewModel.categoryFile))
            {
                // File exists - load Category Names 
                var reader = System.Xml.XmlReader.Create(ModelViewModel.categoryFile);
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
            XmlWriter writer = XmlWriter.Create(ModelViewModel.categoryFile, settings);
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
                        catCollection.Add(category);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var category in e.OldItems.OfType
                        <CategoryViewModel>().Select
                        (v => v.category).OfType<Category>())
                        catCollection.Remove(category);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    catCollection.Clear();
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
