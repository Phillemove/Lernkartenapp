using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects
{
    public class Model
    {
        public CategoryCollection CategoryCollection { get; set; }
        public BoxCollection BoxCollection { get; set; }

        private const String CategoryFile = "../../Data/Categorys.xml"; // Pfad zur Kategoriedatei

        public Model()
        {
            // Load Category Collection
            LoadCategorys();
        }

        private void LoadCategorys()
        {
            // TODO
            /**
            if (File.Exists(CategoryFile))
            {
                Console.WriteLine("its there");
            }
    **/
        }
    }
}
