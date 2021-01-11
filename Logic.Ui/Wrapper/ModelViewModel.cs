using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper
{
    public class ModelViewModel
    {
        public Model model;
        private BoxCollectionViewModel bcvm;


        public ModelViewModel()
        {
            this.model = new Model();
            bcvm = new BoxCollectionViewModel(this.model.BoxCollection);

        }
        public ModelViewModel(Model model)
        {
            this.model = model;
            bcvm = new BoxCollectionViewModel(this.model.BoxCollection);

        }
        public BoxCollectionViewModel BoxCollection
        {
            get
            {
                return bcvm;
            }
        }
    }
}
