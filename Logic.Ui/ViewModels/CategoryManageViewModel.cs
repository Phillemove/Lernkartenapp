using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    public class CategoryManageViewModel
    {
        public CategoryCollectionViewModel MyCategoryCollection { get; set; }

        public CategoryManageViewModel(CategoryCollectionViewModel ccvm)
        {
            this.MyCategoryCollection = ccvm;
        }

    }
}
