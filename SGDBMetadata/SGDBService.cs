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
using System.Reflection;
using System.Net.Http;
using System.Threading;

namespace SGDBMetadata
{
    public class SgdbServiceClient
    {
        private readonly string baseUrl = @"https://www.steamgriddb.com/api/v2/";

        private RestClient client;
        private readonly SGDBMetadataSettings settings;

        public SgdbServiceClient(SGDBMetadataSettings settings)
        {
            client = new RestClient(baseUrl);
            client.Authenticator = new JwtAuthenticator(settings.ApiKey);

            this.settings = settings;
        }

        public RestClient RestClient { get; set; }

        public List<T> Execute<T>(RestRequest request, bool pagination = true) where T : new()
        {
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var logger = LogManager.GetLogger();
            var fullUrl = client.BuildUri(request);
            logger.Info(fullUrl.ToString());

            var items = new List<T>();
            int page = 0;

            while(true)
            {
                request.AddOrUpdateParameter("page", page); 
                var response = client.Execute(request);

                if (response.ErrorException != null)
                    throw new Exception("Error retrieving response", response.ErrorException);

                logger.Info(response.Content);

                var json = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseModel<T>>(response.Content);
                items.AddRange(json.data);
                if (json.data.Count > 0 && pagination)
                {
                    page++;
                } else
                {
                    break;
                }

            }

            return items;
        }

        public List<SearchModel> getSGDBGames(string searchName)
        {
            var logger = LogManager.GetLogger();
            logger.Info(searchName);
            var request = new RestRequest("search/autocomplete/{searchName}", Method.GET);
            request.AddParameter("searchName", searchName, ParameterType.UrlSegment);
            return Execute<SearchModel>(request, false);
        }

        public List<GridModel> getSGDBGameGridByAppId(string platform, string gameId)
        {
            var request = new RestRequest("grids/{platform}/{gameId}", Method.GET);
            request.AddParameter("platform", platform, ParameterType.UrlSegment);
            request.AddParameter("gameId", gameId, ParameterType.UrlSegment);
            if (settings.CoverStyle != "any")
            {
                request.AddParameter("styles", settings.CoverStyle, ParameterType.GetOrPost);
            }
            if (settings.CoverDimension != "any")
            {
                request.AddParameter("dimensions", settings.CoverDimension, ParameterType.GetOrPost);
            }
            request.AddParameter("nsfw", settings.CoverNsfw, ParameterType.GetOrPost);
            request.AddParameter("humor", settings.CoverHumor, ParameterType.GetOrPost);

            return Execute<GridModel>(request);
        }

        public List<GridModel> getSGDBGameGridCover(int gameId)
        {
            var request = new RestRequest("grids/game/{id}", Method.GET);
            request.AddParameter("id", gameId, ParameterType.UrlSegment);
            if (settings.CoverStyle != "any")
            {
                request.AddParameter("styles", settings.CoverStyle, ParameterType.GetOrPost);
            }
            if (settings.CoverDimension != "any")
            {
                request.AddParameter("dimensions", settings.CoverDimension, ParameterType.GetOrPost);
            }
            request.AddParameter("nsfw", settings.CoverNsfw, ParameterType.GetOrPost);
            request.AddParameter("humor", settings.CoverHumor, ParameterType.GetOrPost);
            return Execute<GridModel>(request);
        }
        
        public List<HeroModel> getSGDBGameHero(int gameId)
        {
            var logger = LogManager.GetLogger();
            logger.Info("getSGDBGameHero");
            var request = new RestRequest("/heroes/game/{id}", Method.GET);
            request.AddParameter("id", gameId, ParameterType.UrlSegment);
            if (settings.BackgroundStyle != "any")
            {
                request.AddParameter("styles", settings.BackgroundStyle, ParameterType.GetOrPost);
            }
            if (settings.BackgroundDimension != "any")
            {
                request.AddParameter("dimensions", settings.BackgroundDimension, ParameterType.GetOrPost);
            }
            request.AddParameter("nsfw", settings.BackgroundNsfw, ParameterType.GetOrPost);
            request.AddParameter("humor", settings.BackgroundHumor, ParameterType.GetOrPost);

            return Execute<HeroModel>(request);
        }

        public List<HeroModel> getSGDBGameHeroByAppId(string platform, string gameId)
        {
            var logger = LogManager.GetLogger();
            logger.Info("getSGDBGameHeroByAppId");
            var request = new RestRequest("heroes/{platform}/{gameId}", Method.GET);
            request.AddParameter("platform", platform, ParameterType.UrlSegment);
            request.AddParameter("gameId", gameId, ParameterType.UrlSegment);
            if (settings.BackgroundStyle != "any")
            {
                request.AddParameter("styles", settings.BackgroundStyle, ParameterType.GetOrPost);
            }
            if (settings.BackgroundDimension != "any")
            {
                request.AddParameter("dimensions", settings.BackgroundDimension, ParameterType.GetOrPost);
            }
            request.AddParameter("nsfw", settings.BackgroundNsfw, ParameterType.GetOrPost);
            request.AddParameter("humor", settings.BackgroundHumor, ParameterType.GetOrPost);

            return Execute<HeroModel>(request);
        }

        public List<MediaModel> getSGDBGameLogo(int gameId)
        {
            var request = new RestRequest("logos/game/{gameId}", Method.GET);
            request.AddParameter("gameId", gameId, ParameterType.UrlSegment);
            request.AddParameter("nsfw", settings.IconNsfw, ParameterType.GetOrPost);
            request.AddParameter("humor", settings.IconHumor, ParameterType.GetOrPost);

            return Execute<MediaModel>(request);
        }

        public List<MediaModel> getSGDBGameLogoByAppId(string platform, string gameId)
        {
            var request = new RestRequest("logos/{platform}/{gameId}", Method.GET);
            request.AddParameter("platform", platform, ParameterType.UrlSegment);
            request.AddParameter("gameId", gameId, ParameterType.UrlSegment);
            request.AddParameter("nsfw", settings.IconNsfw, ParameterType.GetOrPost);
            request.AddParameter("humor", settings.IconHumor, ParameterType.GetOrPost);

            return Execute<MediaModel>(request);
        }

        public List<MediaModel> getSGDBGameIcon(int gameId)
        {
            var request = new RestRequest("icons/game/{gameId}", Method.GET);
            request.AddParameter("gameId", gameId, ParameterType.UrlSegment);
            request.AddParameter("nsfw", settings.IconNsfw, ParameterType.GetOrPost);
            request.AddParameter("humor", settings.IconHumor, ParameterType.GetOrPost);

            return Execute<MediaModel>(request);
        }

        public List<MediaModel> getSGDBGameIconByAppId(string platform, string gameId)
        {
            var request = new RestRequest("icons/{platform}/{gameId}", Method.GET);
            request.AddParameter("platform", platform, ParameterType.UrlSegment);
            request.AddParameter("gameId", gameId, ParameterType.UrlSegment);
            request.AddParameter("nsfw", settings.IconNsfw, ParameterType.GetOrPost);
            request.AddParameter("humor", settings.IconHumor, ParameterType.GetOrPost);

            return Execute<MediaModel>(request);
        }

        public SearchModel getGameSGDBFuzzySearch(string gameTitle)
        {
            var logger = LogManager.GetLogger();
            logger.Info(gameTitle);
            var gameListResponse = getSGDBGames(gameTitle);
            if (gameListResponse.Count > 0)
            {
                logger.Info(gameListResponse[0].name);
                return gameListResponse[0]; //First element of search results, should probably implement fuzzysearchquery based on intentions
            }
            else
            {
                var sgdbException = new Exception("Service failure during getGameSGDBFuzzySearch");
                throw sgdbException;
            }
        }

        public List<SearchModel> getGameListSGDB(string gameTitle)
        {
            var logger = LogManager.GetLogger();
            logger.Info(gameTitle);
            var gameListResponse = getSGDBGames(gameTitle);
            if (gameListResponse.Count > 0)
            {
                logger.Info(gameListResponse.ToString());
                return gameListResponse; //First element of search results, should probably implement fuzzysearchquery based on intentions
            }
            else
            {
                var sgdbException = new Exception("Service failure during getGameListSGDB");
                throw sgdbException;
            }
        }

        public string getCoverImageUrl(SearchModel gameSearchItem, string platform, string gameId)
        {
            if (platform != null && gameId != null)
            {
                List<GridModel> grid = getSGDBGameGridByAppId(platform, gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if(grid.Count > 0)
                {
                    return grid[0].url;
                }
            }
            else if (gameSearchItem != null)
            {
                List<GridModel> grid = getSGDBGameGridCover(gameSearchItem.id);
                if (grid.Count > 0)
                {
                    return grid[0].url;
                }
            }
            return "bad path";
        }

        public List<GridModel> getCoverImages(GenericItemOption searchSelection, string platform, string gameId)
        {
            if (platform != null && gameId != null)
            {
                List<GridModel> grid = getSGDBGameGridByAppId(platform, gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if (grid.Count > 0)
                {
                    return grid;
                }
                else if (grid.Count == 0)
                {
                    return null;
                }
                else
                {
                    var sgdbException = new Exception("Service failure during getCoverImages.");
                    throw sgdbException;
                }
            }
            else if (searchSelection != null)
            {
                List<GridModel> grid = getSGDBGameGridCover(int.Parse(searchSelection.Description));
                if (grid.Count > 0)
                {
                    return grid;
                }
                else if (grid.Count == 0)
                {
                    return null;
                }
                else
                {
                    var sgdbException = new Exception("Service failure during getCoverImages.");
                    throw sgdbException;
                }
            }
            else
            {
                return null;
            }
        }

        public string getHeroImageUrl(SearchModel gameSearchItem, string platform, string gameId)
        {
            var logger = LogManager.GetLogger();
            logger.Info("getHeroImageUrl");
            logger.Info(gameSearchItem?.name);
            logger.Info(platform);
            logger.Info(gameId);
            if (platform != null && gameId != null)
            {
                List<HeroModel> hero = getSGDBGameHeroByAppId(platform, gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if (hero.Count > 0)
                {
                    return hero[0].url;
                }
            }
            else if (gameSearchItem != null)
            {
                List<HeroModel> hero = getSGDBGameHero(gameSearchItem.id);
                if (hero.Count > 0)
                {
                    return hero[0].url;
                }
            }
            return "bad path";
        }

        public List<HeroModel> getHeroImages(GenericItemOption searchSelection, string platform, string gameId)
        {
            var logger = LogManager.GetLogger();
            logger.Info("getHeroImages");
            logger.Info(searchSelection?.Name);
            logger.Info(platform);
            logger.Info(gameId);
            if (platform != null && gameId != null)
            {
                List<HeroModel> hero = getSGDBGameHeroByAppId(platform, gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if (hero.Count > 0)
                {
                    return hero;
                }
                else if (hero.Count == 0)
                {
                    return null;
                }
                else
                {
                    var sgdbException = new Exception("Service failure during getHeroImages");
                    throw sgdbException;
                }
            }
            else if (searchSelection != null)
            {
                List<HeroModel> hero = getSGDBGameHero(int.Parse(searchSelection.Description));
                if (hero.Count > 0)
                {
                    return hero;
                }
                else if (hero.Count == 0)
                {
                    return null;
                }
                else
                {
                    var sgdbException = new Exception("Service failure during getHeroImages");
                    throw sgdbException;
                }
            }
            else
            {
                return null;
            }
        }

        public string getLogoImageUrl(SearchModel gameSearchItem, string platform, string gameId)
        {
            if (platform != null && gameId != null)
            {
                List<MediaModel> logo = getSGDBGameLogoByAppId(platform, gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if (logo.Count > 0)
                {
                    return logo[0].url;
                }
            }
            else if (gameSearchItem != null)
            {
                List<MediaModel> logo = getSGDBGameLogo(gameSearchItem.id);
                if (logo.Count > 0)
                {
                    return logo[0].url;
                }
            }
            return "bad path";
        }

        public List<MediaModel> getLogoImages(GenericItemOption searchSelection, string platform, string gameId)
        {
            if (platform != null && gameId != null)
            {
                List<MediaModel> logo = getSGDBGameLogoByAppId(platform, gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if (logo.Count > 0)
                {
                    return logo;
                }
                else if (logo.Count == 0)
                {
                    return null;
                }
                else
                {
                    var sgdbException = new Exception("Service failure during getLogoImages");
                    throw sgdbException;
                }
            }
            else if (searchSelection != null)
            {
                List<MediaModel> logo = getSGDBGameLogo(int.Parse(searchSelection.Description));
                if (logo.Count > 0)
                {
                    return logo;
                }
                else if (logo.Count == 0)
                {
                    return null;
                }
                else
                {
                    var sgdbException = new Exception("Service failure during getLogoImages");
                    throw sgdbException;
                }
            }
            else
            {
                return null;
            }
        }

        public string getIconImageUrl(SearchModel gameSearchItem, string platform, string gameId)
        {
            if (platform != null && gameId != null)
            {
                List<MediaModel> icon = getSGDBGameIconByAppId(platform, gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if (icon.Count > 0)
                {
                    return icon[0].url;
                }
            }
            else if (gameSearchItem != null)
            {
                List<MediaModel> icon = getSGDBGameIcon(gameSearchItem.id);
                if (icon.Count > 0)
                {
                    return icon[0].url;
                }
            }
            return "bad path";
        }

        public List<MediaModel> getIconImages(GenericItemOption searchSelection, string platform, string gameId)
        {
            if (platform != null && gameId != null)
            {
                List<MediaModel> icon = getSGDBGameIconByAppId(platform, gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if (icon.Count > 0)
                {
                    return icon;
                }
                else if (icon.Count == 0)
                {
                    return null;
                }
                else
                {
                    var sgdbException = new Exception("Service failure during getIconImages");
                    throw sgdbException;
                }
            }
            else if (searchSelection != null)
            {
                List<MediaModel> icon = getSGDBGameIcon(int.Parse(searchSelection.Description));
                if (icon.Count > 0)
                {
                    return icon;
                }
                else if (icon.Count == 0)
                {
                    return null;
                }
                else
                {
                    var sgdbException = new Exception("Service failure during getIconImages");
                    throw sgdbException;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
