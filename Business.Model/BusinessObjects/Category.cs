using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects
{
    public class Category
    {
        public String Name { get; set; }

        public Category()
        {

        }
        public Category(String name)
        {
            Name = name;
        }
    }
}
