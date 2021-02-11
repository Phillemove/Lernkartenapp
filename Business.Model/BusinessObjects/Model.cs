using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects
{
    public class Model
    {
        public CategoryCollection CategoryCollection { get; set; }
        public BoxCollection BoxCollection { get; set; }



        public Model()
        {
            BoxCollection = new BoxCollection();
            CategoryCollection = new CategoryCollection();
        }
    }
}
