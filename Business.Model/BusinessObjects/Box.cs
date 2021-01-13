using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects
{

    public class Box : Queue<Card>
    {

        Boxnumber bn;
        public Box()
        {
            this.bn = Boxnumber.None;
        }
        public Box(Boxnumber bn)
        {
            this.bn = bn;
        }
        public Boxnumber Bn
        {
            get
            {
                return bn;
            }
            set
            {
                bn = value;
            }
        }

        public void add(Card card)
        {
            this.Enqueue(card);
        }
        public Card remove()
        {
            return this.Dequeue();
        }
    }
}

