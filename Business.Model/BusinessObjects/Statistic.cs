using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects
{
    public class Statistic
    {

        public DateTime Timestamp { get; set; }
        public bool SuccessfullAnswer { get; set; }
        public enum CurrentBoxNumber { }
        
        public Statistic()
        {

        }

        public Statistic(DateTime timestamp, bool successfullanswer)
        {
            Timestamp = timestamp;
            SuccessfullAnswer = successfullanswer;
        }
    }
}
