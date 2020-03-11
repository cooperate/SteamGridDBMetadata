using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGDBMetadata
{
    public class SearchSchema
    {
        /*
         * "id": 1673,
            "name": "The Legend of Zelda: Four Swords Adventures",
            "release_date": 0,
            "types": [],
            "verified": true
         */
        public int id { get; set; }
        public string name { get; set; }
        public int release_data { get; set; }
        public string[] types { get; set; }
        public Boolean verified { get; set; }
    }
}
