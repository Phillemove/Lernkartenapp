using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
         
    public class CategoryViewModel
    {
        public Category category;
        public String Name
        {
            get
            {
                return category.Name;
            }

            set
            {
                category.Name = value;
            }
        }

        public CategoryViewModel(Category category)
        {
            this.category = category;
        }

        // Needed for Creation of CategoryViewModels in Datagrid
        public CategoryViewModel()
        {
            this.category = new Category("");
        }

    }
}
