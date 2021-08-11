﻿using System;
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

        public ResponseModel<SearchModel> getSGDBGames(string searchName)
        {
            var logger = LogManager.GetLogger();
            logger.Info(searchName);
            var request = new RestRequest("search/autocomplete/{searchName}", Method.GET);
            request.AddParameter("searchName", searchName, ParameterType.UrlSegment);
            return Execute<ResponseModel<SearchModel>>(request);
        }

        public ResponseModel<GridModel> getSGDBGameGridByAppId(string platform, string gameId)
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

            return Execute<ResponseModel<GridModel>>(request);
        }

        public ResponseModel<GridModel> getSGDBGameGridCover(int gameId)
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
            return Execute<ResponseModel<GridModel>>(request);
        }
        
        public ResponseModel<HeroModel> getSGDBGameHero(int gameId)
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

            return Execute<ResponseModel<HeroModel>>(request);
        }

        public ResponseModel<HeroModel> getSGDBGameHeroByAppId(string platform, string gameId)
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

            return Execute<ResponseModel<HeroModel>>(request);
        }

        public ResponseModel<MediaModel> getSGDBGameLogo(int gameId)
        {
            var request = new RestRequest("logos/game/{gameId}", Method.GET);
            request.AddParameter("gameId", gameId, ParameterType.UrlSegment);
            request.AddParameter("nsfw", settings.IconNsfw, ParameterType.GetOrPost);
            request.AddParameter("humor", settings.IconHumor, ParameterType.GetOrPost);

            return Execute<ResponseModel<MediaModel>>(request);
        }

        public ResponseModel<MediaModel> getSGDBGameLogoByAppId(string platform, string gameId)
        {
            var request = new RestRequest("logos/{platform}/{gameId}", Method.GET);
            request.AddParameter("platform", platform, ParameterType.UrlSegment);
            request.AddParameter("gameId", gameId, ParameterType.UrlSegment);
            request.AddParameter("nsfw", settings.IconNsfw, ParameterType.GetOrPost);
            request.AddParameter("humor", settings.IconHumor, ParameterType.GetOrPost);

            return Execute<ResponseModel<MediaModel>>(request);
        }

        public ResponseModel<MediaModel> getSGDBGameIcon(int gameId)
        {
            var request = new RestRequest("icons/game/{gameId}", Method.GET);
            request.AddParameter("gameId", gameId, ParameterType.UrlSegment);
            request.AddParameter("nsfw", settings.IconNsfw, ParameterType.GetOrPost);
            request.AddParameter("humor", settings.IconHumor, ParameterType.GetOrPost);

            return Execute<ResponseModel<MediaModel>>(request);
        }

        public ResponseModel<MediaModel> getSGDBGameIconByAppId(string platform, string gameId)
        {
            var request = new RestRequest("icons/{platform}/{gameId}", Method.GET);
            request.AddParameter("platform", platform, ParameterType.UrlSegment);
            request.AddParameter("gameId", gameId, ParameterType.UrlSegment);
            request.AddParameter("nsfw", settings.IconNsfw, ParameterType.GetOrPost);
            request.AddParameter("humor", settings.IconHumor, ParameterType.GetOrPost);

            return Execute<ResponseModel<MediaModel>>(request);
        }

        public SearchModel getGameSGDBFuzzySearch(string gameTitle)
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

        public List<SearchModel> getGameListSGDB(string gameTitle)
        {
            var logger = LogManager.GetLogger();
            logger.Info(gameTitle);
            var gameListResponse = getSGDBGames(gameTitle);
            if (gameListResponse.success)
            {
                logger.Info(gameListResponse.data.ToString());
                return gameListResponse.data; //First element of search results, should probably implement fuzzysearchquery based on intentions
            }
            else
            {
                var sgdbException = new Exception("Service failure.");
                throw sgdbException;
            }
        }

        public string getCoverImageUrl(string gameName, string platform, string gameId)
        {
            if (platform != null && gameId != null)
            {
                ResponseModel<GridModel> grid = getSGDBGameGridByAppId(platform, gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if(grid.success && grid.data.Count > 0)
                {
                    return grid.data[0].url;
                }
            }
            else
            {
                SearchModel gameCoverSearch = getGameSGDBFuzzySearch(gameName);
                ResponseModel<GridModel> grid = getSGDBGameGridCover(gameCoverSearch.id);
                if (grid.success && grid.data.Count > 0)
                {
                    return grid.data[0].url;
                }
            }
            return "bad path";
        }

        public List<GridModel> getCoverImages(string gameName, string platform, string gameId)
        {
            if (platform != null && gameId != null)
            {
                ResponseModel<GridModel> grid = getSGDBGameGridByAppId(platform, gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if (grid.success && grid.data.Count > 0)
                {
                    return grid.data;
                }
                else if (grid.success && grid.data.Count == 0)
                {
                    return null;
                }
                else
                {
                    var sgdbException = new Exception("Service failure.");
                    throw sgdbException;
                }
            }
            else
            {
                SearchModel gameCoverSearch = getGameSGDBFuzzySearch(gameName);
                ResponseModel<GridModel> grid = getSGDBGameGridCover(gameCoverSearch.id);
                if (grid.success && grid.data.Count > 0)
                {
                    return grid.data;
                }
                else if (grid.success && grid.data.Count == 0)
                {
                    return null;
                }
                else
                {
                    var sgdbException = new Exception("Service failure.");
                    throw sgdbException;
                }
            }
        }

        public string getHeroImageUrl(string gameName, string platform, string gameId)
        {
            var logger = LogManager.GetLogger();
            logger.Info("getHeroImageUrl");
            logger.Info(gameName);
            logger.Info(platform);
            logger.Info(gameId);
            if (platform != null && gameId != null)
            {
                ResponseModel<HeroModel> hero = getSGDBGameHeroByAppId(platform, gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if (hero.success && hero.data.Count > 0)
                {
                    return hero.data[0].url;
                }
            }
            else
            {
                SearchModel gameHeroSearch = getGameSGDBFuzzySearch(gameName);
                ResponseModel<HeroModel> hero = getSGDBGameHero(gameHeroSearch.id);
                if (hero.success && hero.data.Count > 0)
                {
                    return hero.data[0].url;
                }
            }
            return "bad path";
        }

        public List<HeroModel> getHeroImages(string gameName, string platform, string gameId)
        {
            var logger = LogManager.GetLogger();
            logger.Info("getHeroImages");
            logger.Info(gameName);
            logger.Info(platform);
            logger.Info(gameId);
            if (platform != null && gameId != null)
            {
                ResponseModel<HeroModel> hero = getSGDBGameHeroByAppId(platform, gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if (hero.success && hero.data.Count > 0)
                {
                    return hero.data;
                }
                else if (hero.success && hero.data.Count == 0)
                {
                    return null;
                }
                else
                {
                    var sgdbException = new Exception("Service failure.");
                    throw sgdbException;
                }
            }
            else
            {
                SearchModel gameHeroSearch = getGameSGDBFuzzySearch(gameName);
                ResponseModel<HeroModel> hero = getSGDBGameHero(gameHeroSearch.id);
                if (hero.success && hero.data.Count > 0)
                {
                    return hero.data;
                }
                else if (hero.success && hero.data.Count == 0)
                {
                    return null;
                }
                else
                {
                    var sgdbException = new Exception("Service failure.");
                    throw sgdbException;
                }
            }
        }

        public string getLogoImageUrl(string gameName, string platform, string gameId)
        {
            if (platform != null && gameId != null)
            {
                ResponseModel<MediaModel> logo = getSGDBGameLogoByAppId(platform, gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if (logo.success && logo.data.Count > 0)
                {
                    return logo.data[0].url;
                }
            }
            else
            {
                SearchModel gameLogoSearch = getGameSGDBFuzzySearch(gameName);
                ResponseModel<MediaModel> logo = getSGDBGameLogo(gameLogoSearch.id);
                if (logo.success && logo.data.Count > 0)
                {
                    return logo.data[0].url;
                }
            }
            return "bad path";
        }

        public List<MediaModel> getLogoImages(string gameName, string platform, string gameId)
        {
            if (platform != null && gameId != null)
            {
                ResponseModel<MediaModel> logo = getSGDBGameLogoByAppId(platform, gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if (logo.success && logo.data.Count > 0)
                {
                    return logo.data;
                }
                else if (logo.success && logo.data.Count == 0)
                {
                    return null;
                }
                else
                {
                    var sgdbException = new Exception("Service failure.");
                    throw sgdbException;
                }
            }
            else
            {
                SearchModel gameLogoSearch = getGameSGDBFuzzySearch(gameName);
                ResponseModel<MediaModel> logo = getSGDBGameLogo(gameLogoSearch.id);
                if (logo.success && logo.data.Count > 0)
                {
                    return logo.data;
                }
                else if (logo.success && logo.data.Count == 0)
                {
                    return null;
                }
                else
                {
                    var sgdbException = new Exception("Service failure.");
                    throw sgdbException;
                }
            }
        }

        public string getIconImageUrl(string gameName, string platform, string gameId)
        {
            if (platform != null && gameId != null)
            {
                ResponseModel<MediaModel> icon = getSGDBGameIconByAppId(platform, gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if (icon.success && icon.data.Count > 0)
                {
                    return icon.data[0].url;
                }
            }
            else
            {
                SearchModel gameIconSearch = getGameSGDBFuzzySearch(gameName);
                ResponseModel<MediaModel> icon = getSGDBGameIcon(gameIconSearch.id);
                if (icon.success && icon.data.Count > 0)
                {
                    return icon.data[0].url;
                }
            }
            return "bad path";
        }

        public List<MediaModel> getIconImages(string gameName, string platform, string gameId)
        {
            if (platform != null && gameId != null)
            {
                ResponseModel<MediaModel> icon = getSGDBGameIconByAppId(platform, gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if (icon.success && icon.data.Count > 0)
                {
                    return icon.data;
                }
                else if (icon.success && icon.data.Count == 0)
                {
                    return null;
                }
                else
                {
                    var sgdbException = new Exception("Service failure.");
                    throw sgdbException;
                }
            }
            else
            {
                SearchModel gameIconSearch = getGameSGDBFuzzySearch(gameName);
                ResponseModel<MediaModel> icon = getSGDBGameIcon(gameIconSearch.id);
                if (icon.success && icon.data.Count > 0)
                {
                    return icon.data;
                }
                else if (icon.success && icon.data.Count == 0)
                {
                    return null;
                }
                else
                {
                    var sgdbException = new Exception("Service failure.");
                    throw sgdbException;
                }
            }
        }
    }
}
