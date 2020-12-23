using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper
{
    public class BoxViewModel : Queue<CardViewModel>
    {
        public Box box;

        public BoxViewModel()
        {
            this.box = new Box();
        }
    }
}
