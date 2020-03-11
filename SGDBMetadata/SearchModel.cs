using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGDBMetadata
{
    public class SearchModel : ResponseModel
    {
        public SearchSchema[] data { get; set; }
    }
}
