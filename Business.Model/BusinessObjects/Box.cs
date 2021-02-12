using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
            this.bn = Boxnumber.Box1;
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
        public void Add(Card card)
        {
            base.Enqueue(card);
        }
        public Card Remove()
        {
            if (this.Any())
            {
                Card c = this.Dequeue();
                return c;
            }
            else
            {
                return null;
            }
        }
        public new void Clear()
        {
            base.Clear();
        }
    }
}

