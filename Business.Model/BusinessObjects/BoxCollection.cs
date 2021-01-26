using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects
{
    public class BoxCollection : ObservableCollection<Box>
    {
        // Übergabe Parameter enum Boxnumber erstmal weg gelassen, 
        // da er so zu Fehlermeldungen führt. Muss noch implementiert werden
        public Card giveCard(Boxnumber bn)
        {
            foreach (Box box in this)
            {
                if (box.Bn == bn)
                {
                    return box.Remove();
                }
            }
            return null;
        }
        public void storeCard(Card card, Boxnumber bn)
        {
            foreach (Box box in this)
            {
                if (box.Bn == bn)
                {
                    box.Add(card);
                }
            }
        }
    }
}

