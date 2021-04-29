using System;
using System.Collections.Generic;
using System.Linq;
using Playnite.SDK;
using Playnite.SDK.Metadata;
using Playnite.SDK.Plugins;

namespace SGDBMetadata
{
    public class SGDBMetadataProvider : OnDemandMetadataProvider
    {
        private readonly MetadataRequestOptions options;
        private readonly SGDBMetadata plugin;
        private SgdbServiceClient services;
        private GenericItemOption searchSelection;
        private List<MetadataField> availableFields;
        public override List<MetadataField> AvailableFields
        {
            get
            {
                var logger = LogManager.GetLogger();
                logger.Info("AvailableFields");
                if (availableFields == null)
                {
                    availableFields = GetAvailableFields();
                }

                return availableFields;
            }
        }
        public SGDBMetadataProvider(MetadataRequestOptions options, SGDBMetadata plugin, string apiKey, string dimension, string style, string nsfw, string humor)
        {
            this.options = options;
            this.plugin = plugin;
            services = new SgdbServiceClient(apiKey, dimension, style, nsfw, humor);
            var logger = LogManager.GetLogger();
            logger.Info("SGDB Initialized");
        }

        public string convertPlayniteGamePluginIdToSGDBPlatformEnum(Guid pluginId)
        {
            //check for platform "steam""origin""egs""bnet""uplay"
            switch (BuiltinExtensions.GetExtensionFromId(pluginId))
            {
                case BuiltinExtension.SteamLibrary:
                    return "steam";
                case BuiltinExtension.OriginLibrary:
                    return "origin";
                case BuiltinExtension.EpicLibrary:
                    return "egs";
                case BuiltinExtension.BattleNetLibrary:
                    return "bnet";
                default:
                    return null;
            }
        }

        // Override additional methods based on supported metadata fields.
        public override MetadataFile GetCoverImage()
        {
            var logger = LogManager.GetLogger();
            logger.Info("GetCoverImage");
            if (options.IsBackgroundDownload)
            {
                string gameUrl;
                if (
                    options.GameData.Source != null
                    && options.GameData.PluginId == BuiltinExtensions.GetIdFromExtension(BuiltinExtension.SteamLibrary)
                    && options.GameData.GameId != null
                )
                {
                    gameUrl = services.getCoverImageUrl(
                        options.GameData.Name,
                        convertPlayniteGamePluginIdToSGDBPlatformEnum(options.GameData.PluginId),
                        options.GameData.GameId);
                }
                else
                {
                    gameUrl = services.getCoverImageUrl(options.GameData.Name);
                }
                if (gameUrl == "bad path")
                {
                    return base.GetCoverImage();
                }
                else
                {
                    return new MetadataFile(gameUrl);
                }
            }
            else
            {
                if (AvailableFields.Contains(MetadataField.Name))
                {
                    logger.Info("search Selection" + searchSelection);
                    if (searchSelection != null)
                    {
                        var covers = services.getCoverImages(searchSelection.Name);
                        dynamic selection = null;
                        if (covers != null)
                        {
                            selection = GetCoverManually(covers);
                        }
                        if (selection == null || selection.Path == "nopath")
                        {
                            return base.GetCoverImage();
                        }
                        else
                        {
                            return new MetadataFile(selection.FullRes);
                        }
                    }
                    else
                    {
                        return base.GetCoverImage();
                    }
                }
                else
                {
                    return base.GetCoverImage();
                }
            }
        }

        public override MetadataFile GetBackgroundImage()
        {
            if (options.IsBackgroundDownload)
            {
                string gameUrl;
                if (options.GameData.Source != null && options.GameData.Source.ToString().ToLower() == "steam" && options.GameData.GameId != null)
                {
                    gameUrl = services.getHeroImageUrl(options.GameData.Name, convertPlayniteGamePluginIdToSGDBPlatformEnum(options.GameData.PluginId), options.GameData.GameId);
                }
                else
                {
                    gameUrl = services.getHeroImageUrl(options.GameData.Name);
                }
                if (gameUrl == "bad path")
                {
                    return base.GetBackgroundImage();
                }
                else
                {
                    return new MetadataFile(gameUrl);
                }
            }
            else
            {
                if (AvailableFields.Contains(MetadataField.Name))
                {
                    if (searchSelection != null)
                    {
                        var heroes = services.getHeroImages(searchSelection.Name);
                        dynamic selection = null;
                        if (heroes != null)
                        {
                            selection = GetHeroManually(heroes);
                        }
                        if (selection == null || selection.Path == "nopath")
                        {
                            return base.GetBackgroundImage();
                        }
                        else
                        {
                            return new MetadataFile(selection.FullRes);
                        }
                    }
                    else
                    {
                        return base.GetBackgroundImage();
                    }
                }
                else
                {
                    return base.GetBackgroundImage();
                }
            }
        }

        public override MetadataFile GetIcon()
        {
            if (options.IsBackgroundDownload)
            {
                var logger = LogManager.GetLogger();
                logger.Info("SGDBMetadataProvider GetIcon options " + options.GameData.ToString());
                string gameUrl;
                if (options.GameData.Source != null && options.GameData.Source.ToString().ToLower() == "steam" && options.GameData.GameId != null)
                {
                    gameUrl = services.getIconImageUrl(options.GameData.Name, convertPlayniteGamePluginIdToSGDBPlatformEnum(options.GameData.PluginId), options.GameData.GameId);
                }
                else
                {
                    gameUrl = services.getIconImageUrl(options.GameData.Name);
                }
                if (gameUrl == "bad path")
                {
                    return base.GetIcon();
                }
                else
                {
                    return new MetadataFile(gameUrl);
                }
            }
            else
            {
                if (AvailableFields.Contains(MetadataField.Name))
                {
                    if (searchSelection != null)
                    {
                        var icons = services.getIconImages(searchSelection.Name);
                        dynamic selection = null;
                        if (icons != null)
                        {
                            selection = GetIconManually(icons);
                        }
                        if (selection == null || selection.Path == "nopath")
                        {
                            return base.GetIcon();
                        }
                        else
                        {
                            return new MetadataFile(selection.FullRes);
                        }
                    }
                    else
                    {
                        return base.GetIcon();
                    }
                }
                else
                {
                    return base.GetIcon();
                }
            }
        }

        private void GetGame(List<GenericItemOption> gameList, string caption)
        {
            var item = plugin.PlayniteApi.Dialogs.ChooseItemWithSearch(gameList, (a) =>
            {
                try
                {
                    return new List<GenericItemOption>(services.getGameListSGDB(a).Select(game => new GenericItemOption(game.name, game.id.ToString())));

                }
                catch
                {
                    var sgdbException = new Exception("Service failure.");
                    throw sgdbException;
                }
            }, options.GameData.Name, caption);
            searchSelection = item;
        }
        private List<MetadataField> GetAvailableFields()
        {
            var logger = LogManager.GetLogger();
            logger.Info("GetAvailableFields");

            if (searchSelection == null)
            {
                GetSgdbMetadata();
            }
            var fields = new List<MetadataField> { MetadataField.Name };
            fields.Add(MetadataField.Icon);
            fields.Add(MetadataField.BackgroundImage);
            fields.Add(MetadataField.CoverImage);
            return fields;
        }

        private void GetSgdbMetadata()
        {
            if (!options.IsBackgroundDownload)
            {
                var logger = LogManager.GetLogger();
                logger.Info("GetSgdbMetadata");
                var gameList = new List<GenericItemOption>(services.getGameListSGDB(options.GameData.Name).Select(game => new GenericItemOption(game.name, game.id.ToString())));
                GetGame(gameList, "Choose Game");
            }
        }
        private ImageFileOption GetCoverManually(List<GridModel> possibleCovers)
        {
            var selection = new List<ImageFileOption>();
            foreach (var cover in possibleCovers)
            {
                selection.Add(new ThumbFileOption { Path = cover.thumb, FullRes = cover.url });
            }
            if (selection.Count > 0)
            {
                return plugin.PlayniteApi.Dialogs.ChooseImageFile(
                    selection, "Choose Cover");
            }
            else
            {
                return new ImageFileOption("nopath");
            }
        }

        private ImageFileOption GetHeroManually(List<HeroModel> possibleHeroes)
        {
            var selection = new List<ImageFileOption>();
            foreach (var hero in possibleHeroes)
            {
                selection.Add(new ThumbFileOption { Path = hero.thumb, FullRes = hero.url });
            }
            if (selection.Count > 0)
            {
                return plugin.PlayniteApi.Dialogs.ChooseImageFile(
                selection, "Choose Background");
            }
            else
            {
                return new ImageFileOption("nopath");
            }

        }

        private ImageFileOption GetIconManually(List<MediaModel> possibleIcons)
        {
            var selection = new List<ImageFileOption>();
            foreach (var icon in possibleIcons)
            {
                selection.Add(new ThumbFileOption
                {
                    Path = icon.thumb,
                    FullRes = icon.url
                });
            }
            if (selection.Count > 0)
            {
                return plugin.PlayniteApi.Dialogs.ChooseImageFile(
                selection, "Choose Icon");
            }
            else
            {
                return new ImageFileOption("nopath");
            }
        }
    }
}
