﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects
{
    public class Card
    {
        public String Question { get; set; }
        public String Answer { get; set; }
        public String QuestionPic { get; set; }
        public String AnswerPic { get; set; }
        public Category Category { get; set; }
        public StatisticCollection StatisticCollection { get; set; }

        public Card()
        {
            Question = null;
            Answer = null;
            QuestionPic = null;
            AnswerPic = null;
            Category = null;
            StatisticCollection = null;
        }
        public Card(String question, String answer)
        {
            Question = question;
            Answer = answer;
            QuestionPic = null;
            AnswerPic = null;
            Category = null;
            StatisticCollection = null;
        }
        public Card(String question, String answer, String questionPic, String answerPic,
                                     Category category,StatisticCollection statisticCollectio)
        {
            Question = question;
            Answer=answer;
            QuestionPic = questionPic;
            AnswerPic = answerPic;
            Category = category;
            StatisticCollection = statisticCollectio;
        }
        public Card(Category category)
        {
            Question = null;
            Answer = null;
            QuestionPic = null;
            AnswerPic = null;
            Category = category;
            StatisticCollection = null;
        }
    }
}
