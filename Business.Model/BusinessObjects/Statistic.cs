using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects
{
    public enum CurrentBoxNumber
    {
        None,
        Box1,
        Box2,
        Box3,
        Box4,
        Box5
    }
    public class Statistic
    {
        
        public DateTime Timestamp { get; set; }
        public bool SuccessfullAnswer { get; set; }
        public CurrentBoxNumber CurrentBoxNumber { get; set; }

       
        
        public Statistic()
        {
            CurrentBoxNumber = CurrentBoxNumber.None;
        }

        public Statistic(DateTime timestamp, bool successfullanswer, CurrentBoxNumber cbn)
        {
            Timestamp = timestamp;
            SuccessfullAnswer = successfullanswer;
            CurrentBoxNumber = cbn;
        }
    }
}
