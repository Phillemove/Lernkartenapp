﻿using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper
{
    public class StatisticViewModel
    {
        public Statistic statistic;

        public StatisticViewModel()
        {
            this.statistic = new Statistic();
        }

        public DateTime Timestamp
        {
            get
            {
                return statistic.Timestamp;
            }

            set
            {
                statistic.Timestamp = value;
            }
        }

        public bool SuccessfulAnswer
        {
            get
            {
                return statistic.SuccessfullAnswer;
            }

            set
            {
                statistic.SuccessfullAnswer = value;
            }
        }

        public enum CurrentBoxNumber { }
    }
}
