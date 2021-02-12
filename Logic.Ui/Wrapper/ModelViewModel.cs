﻿using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
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
        public CategoryCollectionViewModel myCatCollection;
        public BoxCollectionViewModel BoxCollectionVM { get; set; }

        public static readonly string saveDirectory =
            @"..\..\..\Lernkarten\";
        public static readonly string savePicDirectory =
            @"..\..\..\Lernkarten\content\";
        public static readonly string categoryFile =
            @"..\..\..\Data\Categorys.xml";

        public ModelViewModel()
        {
            this.model = new Model();
            BoxCollectionVM = new BoxCollectionViewModel();
            // Creating Boxes
            BoxCollectionVM.Add(new BoxViewModel(new Box(Boxnumber.Box1)));
            BoxCollectionVM.Add(new BoxViewModel(new Box(Boxnumber.Box2)));
            BoxCollectionVM.Add(new BoxViewModel(new Box(Boxnumber.Box3)));
            BoxCollectionVM.Add(new BoxViewModel(new Box(Boxnumber.Box4)));
            BoxCollectionVM.Add(new BoxViewModel(new Box(Boxnumber.Box5)));

            myCatCollection = new CategoryCollectionViewModel();
        }
    }
}
