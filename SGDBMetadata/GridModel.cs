using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGDBMetadata
{
    public class GridModel : MediaModel
    {
        /*
         * "id": 65001,
            "score": 3,
            "style": "alternate",
            "notes": null,
            "language": "en",
            "url": "https://d38w655bqoyvyi.cloudfront.net/grid/f40e1eff5a6bcca01d27694dacf49c78.png",
            "thumb": "https://d38w655bqoyvyi.cloudfront.net/thumb/f40e1eff5a6bcca01d27694dacf49c78.png",
            "author": {
                "name": "Mac!",
                "steam64": "76561198069521439",
                "avatar": "https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/c1/c1261edf193be8d738e1e43d3c23ec8fbf89f32d.jpg"
            }
         */
        public string style { get; set; }
    }
}
