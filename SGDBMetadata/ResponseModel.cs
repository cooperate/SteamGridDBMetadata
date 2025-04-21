using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace SGDBMetadata
{
    public class ResponseModel <T>
    {
        public Boolean success { get; set; }
        public int page { get; set; }
        public int total { get; set; }
        public int limit { get; set; }
        public List<T> data;

        public List<T> NewList
        {
            get { return data; }
            set { data = value; }
        }
    }
}
