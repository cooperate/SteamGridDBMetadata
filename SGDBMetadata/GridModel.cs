using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGDBMetadata
{
    public class GridModel : ResponseModel
    {
        public GridSchema[] data { get; set; }
    }
}
