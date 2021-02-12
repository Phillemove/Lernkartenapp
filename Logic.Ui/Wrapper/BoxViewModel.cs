using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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
            public BoxViewModel(Box box)
            {
                this.box = box;
            }
            public Boxnumber Bn
            {
                get
                {
                    return box.Bn;
                }
                set
                {
                    box.Bn = value;
                }
            }
            public void Add(CardViewModel cardvm)
            {
                box.Add(cardvm.card);
                base.Enqueue(cardvm);
            }
            public CardViewModel Remove()
            {
            box.Remove();
            if (this.Any())
            {
                CardViewModel cvm = this.Dequeue();
                return cvm;
            }
            else
            {
                return null;
            }
        }

        public new void Clear()
        {
            box.Clear();
            base.Clear();
        }
    }
}

