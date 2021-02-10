﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using
System.Windows
;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    public class CategoryManageViewModel
    {
        public CategoryCollectionViewModel MyCategoryCollection { get; set; }

        public RelayCommand CloseWindow { get; }

        public CategoryManageViewModel(CategoryCollectionViewModel ccvm)
        {
            CloseWindow = new RelayCommand(param => Close(param));
            this.MyCategoryCollection = ccvm;
        }

        // Saves Categorys to FileSystem and Closes the Window
        private void Close(object param)
        {
            MyCategoryCollection.SaveCategorys();
            Window window = (Window)param;
            window.Close();
        }

    }
}
