using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Models
{
    public class BaseDto
    {

        private int _ID;
        private DateTime _CreateDate;
        private DateTime _UpdateDate;


        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public DateTime CreateDate
        {
            get { return _CreateDate; }
            set { _CreateDate = value; }
        }
        public DateTime UpdateDate
        {
            get { return _UpdateDate; }
            set { _UpdateDate = value; }
        }






    }
}
