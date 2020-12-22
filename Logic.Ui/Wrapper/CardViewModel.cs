using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper
{
    class CardViewModel
    {
        public Card card;
        public CardViewModel()
        {
            this.card = new Card();
        }

        public string Question
        {
            get
            {
                return card.Question;
            }
            set
            {
                card.Question = value;
            }
        }
        public string Answer
        {
            get
            {
                return card.Answer;
            }
            set
            {
                card.Answer = value;
            }
        }
    }
}
