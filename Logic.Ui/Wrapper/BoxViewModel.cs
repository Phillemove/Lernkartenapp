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
            public void add(CardViewModel cardvm)
            {
                box.add(cardvm.card);
                this.Enqueue(cardvm);
            }
            public CardViewModel remove()
            {
                this.Dequeue();
                return new CardViewModel(box.remove());
            }
        }
    }

