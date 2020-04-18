﻿using System;
using System.Collections.Generic;
using RestSharp;
using RestSharp.Authenticators;
using Playnite.SDK;

namespace SGDBMetadata
{
    public class SgdbServiceClient
    {
        private string baseUrl = "https://www.steamgriddb.com/api/v2/";

        private RestClient client;
        private string dimension;
        private string style;

        public SgdbServiceClient(string bearerToken, string dimension, string style)
        {
            client = new RestClient(baseUrl);
            client.Authenticator = new JwtAuthenticator(bearerToken);
            this.dimension = dimension;
            this.style = style;
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
            if (dimension != null || dimension != "any") {
                request.AddParameter("dimensions", dimension, ParameterType.GetOrPost);
            }
            if (dimension != null || dimension != "any") {
                request.AddParameter("styles", style, ParameterType.GetOrPost);
            }
            return Execute<ResponseModel<GridModel>>(request);
        }

        public ResponseModel<GridModel> getSGDBGameGridCover(int gameId)
        {
            var request = new RestRequest("grids/game/{id}", Method.GET);
            request.AddParameter("id", gameId, ParameterType.UrlSegment);
            if (dimension != "any")
            {
                request.AddParameter("dimensions", dimension, ParameterType.GetOrPost);
            }
            if (style != null || style != "any")
            {
                request.AddParameter("styles", style, ParameterType.GetOrPost);
            }
            return Execute<ResponseModel<GridModel>>(request);
        }
        
        public ResponseModel<HeroModel> getSGDBGameHero(int gameId)
        {
            var request = new RestRequest("/heroes/game/{id}", Method.GET);
            request.AddParameter("id", gameId, ParameterType.UrlSegment);
            return this.Execute<ResponseModel<HeroModel>>(request);
        }

        public ResponseModel<HeroModel> getSGDBGameHeroByAppId(string platform, string gameId)
        {
            var request = new RestRequest("heroes/{platform}/{gameId}", Method.GET);
            request.AddParameter("platform", platform, ParameterType.UrlSegment);
            request.AddParameter("gameId", gameId, ParameterType.UrlSegment);
            return Execute<ResponseModel<HeroModel>>(request);
        }

        public ResponseModel<MediaModel> getSGDBGameLogo(int gameId)
        {
            var request = new RestRequest("logos/game/{gameId}", Method.GET);
            request.AddParameter("gameId", gameId, ParameterType.UrlSegment);
            return Execute<ResponseModel<MediaModel>>(request);
        }

        public ResponseModel<MediaModel> getSGDBGameLogoByAppId(string platform, string gameId)
        {
            var request = new RestRequest("logos/{platform}/{gameId}", Method.GET);
            request.AddParameter("platform", platform, ParameterType.UrlSegment);
            request.AddParameter("gameId", gameId, ParameterType.UrlSegment);
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

        public string getCoverImageUrl(string gameName, string platform = null, string gameId = null)
        {
            if (platform != null && gameId != null)
            {
                ResponseModel<GridModel> grid = getSGDBGameGridByAppId(platform, gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if(grid.success)
                {
                    return grid.data[0].url;
                }
            }
            else
            {
                SearchModel gameCoverSearch = getGameSGDBFuzzySearch(gameName);
                ResponseModel<GridModel> grid = getSGDBGameGridCover(gameCoverSearch.id);
                if (grid.success)
                {
                    return grid.data[0].url;
                }
            }
            return "bad path";
        }

        public List<GridModel> getCoverImages(string gameName, string platform = null, string gameId = null)
        {
            if (platform != null && gameId != null)
            {
                ResponseModel<GridModel> grid = getSGDBGameGridByAppId(platform, gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if (grid.success)
                {
                    return grid.data;
                }else
                {
                    var sgdbException = new Exception("Service failure.");
                    throw sgdbException;
                }
            }
            else
            {
                SearchModel gameCoverSearch = getGameSGDBFuzzySearch(gameName);
                ResponseModel<GridModel> grid = getSGDBGameGridCover(gameCoverSearch.id);
                if (grid.success)
                {
                    return grid.data;
                } else
                {
                    var sgdbException = new Exception("Service failure.");
                    throw sgdbException;
                }
            }
        }

        public string getHeroImageUrl(string gameName, string platform = null, string gameId = null)
        {
            if (platform != null && gameId != null)
            {
                ResponseModel<HeroModel> hero = getSGDBGameHeroByAppId(platform, gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if (hero.success)
                {
                    return hero.data[0].url;
                }
            }
            else
            {
                SearchModel gameHeroSearch = getGameSGDBFuzzySearch(gameName);
                ResponseModel<HeroModel> hero= getSGDBGameHero(gameHeroSearch.id);
                if (hero.success)
                {
                    return hero.data[0].url;
                }
            }
            return "bad path";
        }

        public List<HeroModel> getHeroImages(string gameName, string platform = null, string gameId = null)
        {
            if (platform != null && gameId != null)
            {
                ResponseModel<HeroModel> hero = getSGDBGameHeroByAppId(platform, gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if (hero.success)
                {
                    return hero.data;
                } else
                {
                    var sgdbException = new Exception("Service failure.");
                    throw sgdbException;
                }
            }
            else
            {
                SearchModel gameHeroSearch = getGameSGDBFuzzySearch(gameName);
                ResponseModel<HeroModel> hero = getSGDBGameHero(gameHeroSearch.id);
                if (hero.success)
                {
                    return hero.data;
                } else
                {
                    var sgdbException = new Exception("Service failure.");
                    throw sgdbException;
                }
            }
        }

        public string getLogoImageUrl(string gameName, string platform = null, string gameId = null)
        {
            if (platform != null && gameId != null)
            {
                ResponseModel<MediaModel> logo = getSGDBGameLogoByAppId(platform, gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if (logo.success)
                {
                    return logo.data[0].url;
                }
            }
            else
            {
                SearchModel gameLogoSearch = getGameSGDBFuzzySearch(gameName);
                ResponseModel<MediaModel> logo = getSGDBGameLogo(gameLogoSearch.id);
                if (logo.success)
                {
                    return logo.data[0].url;
                }
            }
            return "bad path";
        }

        public List<MediaModel> getLogoImages(string gameName, string platform = null, string gameId = null)
        {
            if (platform != null && gameId != null)
            {
                ResponseModel<MediaModel> logo = getSGDBGameLogoByAppId(platform, gameId); //First element of search results, should probably implement fuzzysearchquery based on intentions
                if (logo.success)
                {
                    return logo.data;
                } else
                {
                    var sgdbException = new Exception("Service failure.");
                    throw sgdbException;
                }
            }
            else
            {
                SearchModel gameLogoSearch = getGameSGDBFuzzySearch(gameName);
                ResponseModel<MediaModel> logo = getSGDBGameLogo(gameLogoSearch.id);
                if (logo.success)
                {
                    return logo.data;
                } else
                {
                    var sgdbException = new Exception("Service failure.");
                    throw sgdbException;
                }
            }
        }
    }
}
