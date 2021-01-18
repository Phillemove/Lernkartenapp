using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper
{
    public class CardViewModel 
    {


        public Card card;
        public CardViewModel()
        {
            this.card = new Card();
        }
        public CardViewModel(Card c)
        {
            this.card = c;
        
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
        public Category Category
        {
            get
            {
                return card.Category;
            }
            set
            {
                card.Category = value;
            }
        }

        public string QuestionPic
        {
            get
            {
                return card.QuestionPic;
            }
            set
            {
                card.QuestionPic = value;
            }
        }
        public string AnswerPic
        {
            get
            {
                return card.AnswerPic;
            }
            set
            {
                card.AnswerPic = value;
            }
        }

        public StatisticCollection StatisticCollection
        {
            get
            {
                return card.StatisticCollection;
            }
            set
            {
                card.StatisticCollection = value;
            }
        }
    }
}
