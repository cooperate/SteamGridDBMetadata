using System;

namespace SGDBMetadata
{
    public class SearchModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int release_data { get; set; }
        public string[] types { get; set; }
        public Boolean verified { get; set; }
    }
}
