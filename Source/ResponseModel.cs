using System;
using System.Collections.Generic;

namespace SGDBMetadata
{
    public class ResponseModel <T>
    {
        public Boolean success { get; set; }
        public List<T> data;

        public List<T> NewList
        {
            get { return data; }
            set { data = value; }
        }
    }
}
