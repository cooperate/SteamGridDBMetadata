﻿namespace SGDBMetadata
{
    public class MediaModel
    {
        public int id { get; set; }
        public int score { get; set; }
        public string notes { get; set; }
        public string language { get; set; }
        public string url { get; set; }
        public string thumb { get; set; }
        public AuthorSchema author { get; set; }
    }
}
