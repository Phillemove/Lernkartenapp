using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    public class ImportForeignFormatViewModel
    {
        public CategoryCollectionViewModel MyCategoryCollection { get; set; }

        public ImportForeignFormatViewModel(CategoryCollectionViewModel ccvm)
        { 
            this.MyCategoryCollection = ccvm;
        }

    }
}
