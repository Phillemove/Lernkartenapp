using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects
{
    public class Box: Queue<Card>
    {
        enum Boxnumber { }

        public void add(Card card)
        {

        }
        public Card remove(Card card)
        {
            return card;
        }
    }
}
