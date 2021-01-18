using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper
{
    public class ModelViewModel
    {
        public Model model;
        private BoxCollectionViewModel bcvm;
        public CategoryCollectionViewModel myCategoryCollection;


        private const string CategoryFile = "\\..\\..\\..\\Data\\Categorys.xml";


        public ModelViewModel()
        {
            this.model = new Model();
            bcvm = new BoxCollectionViewModel(this.model.BoxCollection);
            myCategoryCollection = new CategoryCollectionViewModel(this.model.CategoryCollection);
            LoadCategorys();

        }
        public ModelViewModel(Model model)
        {
            this.model = model;
            bcvm = new BoxCollectionViewModel(this.model.BoxCollection);
            myCategoryCollection = new CategoryCollectionViewModel(this.model.CategoryCollection);


            LoadCategorys();

        }


        private void LoadCategorys()
        {

            // Should be Ui.Desktop/bin/Debug or Ui.Desktop/bin/Release
            var appRoot = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            if (File.Exists(appRoot + CategoryFile))
            {
                // File exists - load Category Names 
                var reader = XmlReader.Create(appRoot + CategoryFile);
                while (reader.ReadToFollowing("Category"))
                {
                    // Read Caegorys and create Category Object in the Collection
                    myCategoryCollection.Add(new CategoryViewModel(new Category(reader.ReadElementContentAsString())));
                }
            }
        }





        public BoxCollectionViewModel BoxCollectionVM
        {
            get
            {
                return bcvm;
            }
        }
    }
}
