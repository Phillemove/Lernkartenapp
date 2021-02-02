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

       // private const string CategoryFile = @"..\..\..\Data\Categorys.xml";


        public ModelViewModel()
        {
            this.model = new Model();
            bcvm = new BoxCollectionViewModel(this.model.BoxCollection);
            myCategoryCollection = new CategoryCollectionViewModel();
        }
        /*
        public ModelViewModel(Model model)
        {
            this.model = model;
            bcvm = new BoxCollectionViewModel(this.model.BoxCollection);
            myCategoryCollection = new CategoryCollectionViewModel();
        } */

        public BoxCollectionViewModel BoxCollectionVM
        {
            get
            {
                return bcvm;
            }
        }
    }
}
