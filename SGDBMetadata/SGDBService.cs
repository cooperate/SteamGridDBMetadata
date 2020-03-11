using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using RestSharp;
using RestSharp.Authenticators;
using SGDBMetadata;
using Playnite.SDK;

namespace SGDBMetadata
{
    public class SgdbServiceClient
    {
        private string baseUrl = "https://www.steamgriddb.com/api/v2/";

        private RestClient client;

        public SgdbServiceClient(string bearerToken)
        {
            client = new RestClient(baseUrl);
            client.Authenticator = new JwtAuthenticator(bearerToken);
        }

        public RestClient RestClient { get; set; }

        public T Execute<T>(RestRequest request) where T : new()
        {
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var logger = LogManager.GetLogger();
            var fullUrl = client.BuildUri(request);
            logger.Info(fullUrl.ToString());
            var response = client.Execute(request);
            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var sgdbException = new Exception(message, response.ErrorException);
                throw sgdbException;
            }
            var content = response.Content;
            logger.Info(content);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);
        }

        public SearchModel getSGDBGames(string searchName)
        {
            var logger = LogManager.GetLogger();
            logger.Info(searchName);
            var request = new RestRequest("search/autocomplete/{searchName}", Method.GET);
            request.AddParameter("searchName", searchName, ParameterType.UrlSegment);
            return Execute<SearchModel>(request);
        }

        public GridModel getSGDBGameGrid(int id)
        {
            var request = new RestRequest("grids/game/{id}", Method.GET);
            request.AddParameter("id", id, ParameterType.UrlSegment);
            request.AddParameter("dimensions", "600x900", ParameterType.GetOrPost);
            return Execute<GridModel>(request);
        }

        /*public ResponseModel getSGDBGameHero(UInt64 id)
        {
            var request = new RestRequest("/heroes/game/{id}", Method.GET);
            return this.Execute<ResponseModel>(request);
        }*/

        public SearchSchema getGameSGDBFuzzySearch(string gameTitle)
        {
            var logger = LogManager.GetLogger();
            logger.Info(gameTitle);
            var gameListResponse = getSGDBGames(gameTitle);
            if (gameListResponse.success)
            {
                logger.Info(gameListResponse.data[0].name);
                return gameListResponse.data[0]; //First element of search results, should probably implement fuzzysearchquery based on intentions
            }
            else
            {
                var sgdbException = new Exception("Service failure.");
                throw sgdbException;
            }
        }

        public string getCoverImageUrl(string gameTitle)
        {
            var gameId = getGameSGDBFuzzySearch(gameTitle).id;
            var gameCoverResponse = getSGDBGameGrid(gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
            if (gameCoverResponse.success)
            {
                var logger = LogManager.GetLogger();
                logger.Info(gameCoverResponse.data[0].url);
                return gameCoverResponse.data[0].url; //This request seems to return results in descending order by score, this is ideal.
            }
            else
            {
                return "bad path";
            }
            /*
            Example response for one grid entity.
            {
                "id": 60363,
                "score": 0,
                "style": "alternate",
                "notes": null,
                "language": "en",
                "url": "https://d38w655bqoyvyi.cloudfront.net/grid/01edeb72ae20fe8bd93d126ec0fbaf91.png",
                "thumb": "https://d38w655bqoyvyi.cloudfront.net/thumb/01edeb72ae20fe8bd93d126ec0fbaf91.png",
                "author": {
                    "name": "Dark Seals",
                    "steam64": "76561198040948626",
                    "avatar": "https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/69/69c6707a64b7f31b9b3aa2e900bb1d9f988a56b5.jpg"
                }
            },
            */
        }

        /*public string getHeroImageUrl(string gameTitle)
        {
            var gameListResponse = getSGDBGames(gameTitle);
            if (gameListResponse.success)
            {
                var gameCoverResponse = getSGDBGameHero(gameListResponse.data[0].id); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if (gameCoverResponse.success)
                {
                    return gameCoverResponse.data[0].url; //This request seems to return results in descending order by score, this is ideal.
                } else
                {
                    return "bad path";
                }
                *//*
                Example response for one hero entity.
                 {
                     "id": 3823,
                     "score": 1,
                     "notes": null,
                     "language": "en",
                     "style": "alternate",
                     "url": "https://d38w655bqoyvyi.cloudfront.net/hero/bca382c81484983f2d437f97d1e141f3.png",
                     "thumb": "https://d38w655bqoyvyi.cloudfront.net/hero_thumb/bca382c81484983f2d437f97d1e141f3.png",
                     "author": {
                         "name": "Mac!",
                         "steam64": "76561198069521439",
                         "avatar": "https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/c1/c1261edf193be8d738e1e43d3c23ec8fbf89f32d.jpg"
                     }
                 }
                *//*
            }
            else
            {
                return "bad path";
            }
        }*/
    }
}
