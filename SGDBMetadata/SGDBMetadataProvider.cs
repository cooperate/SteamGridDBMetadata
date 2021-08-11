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
        private string gamePlatformEnum;
        private List<MetadataField> availableFields;
        private string iconAssetSelection;
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
        public SGDBMetadataProvider(MetadataRequestOptions options, SGDBMetadata plugin, SGDBMetadataSettings settings)
        {
            this.options = options;
            this.plugin = plugin;
            this.iconAssetSelection = settings.IconAssetSelection;
            services = new SgdbServiceClient(settings);
            var logger = LogManager.GetLogger();
            logger.Info("SGDB Initialized");
        }

        public void convertPlayniteGamePluginIdToSGDBPlatformEnum(Guid pluginId)
        {
            //check for platform "steam""origin""egs""bnet""uplay"
            // only steam is reliable enough on sgdb
            // most games are not linked to other platforms
            switch (BuiltinExtensions.GetExtensionFromId(pluginId))
            {
                case BuiltinExtension.SteamLibrary:
                    gamePlatformEnum = "steam";
                    return;
                case BuiltinExtension.OriginLibrary:
                    //gamePlatformEnum = "origin";
                    return;
                case BuiltinExtension.EpicLibrary:
                    //gamePlatformEnum = "egs";
                    return;
                case BuiltinExtension.BattleNetLibrary:
                    //gamePlatformEnum = "bnet";
                    return;
                default:
                    return;
            }
        }

        // Override additional methods based on supported metadata fields.
        public override MetadataFile GetCoverImage()
        {
            var logger = LogManager.GetLogger();
            logger.Info("GetCoverImage");
            if (options.IsBackgroundDownload)
            {
                string gameUrl = services.getCoverImageUrl(options.GameData.Name, gamePlatformEnum ?? null, options.GameData.GameId ?? null);
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
                    if (string.IsNullOrEmpty(gamePlatformEnum) == false || searchSelection != null)
                    {
                        var covers = services.getCoverImages(searchSelection ?? null, gamePlatformEnum ?? null, options.GameData.GameId ?? null);
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
                string gameUrl = services.getHeroImageUrl(options.GameData.Name, gamePlatformEnum ?? null, options.GameData.GameId ?? null);
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
                    if (string.IsNullOrEmpty(gamePlatformEnum) == false || searchSelection != null)
                    {
                        var heroes = services.getHeroImages(searchSelection ?? null, gamePlatformEnum ?? null, options.GameData.GameId ?? null);
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
                string gameUrl;
                if (iconAssetSelection == "logos")
                {
                    gameUrl = services.getLogoImageUrl(options.GameData.Name, gamePlatformEnum ?? null, options.GameData.GameId ?? null);
                }
                else
                {
                    gameUrl = services.getIconImageUrl(options.GameData.Name, gamePlatformEnum ?? null, options.GameData.GameId ?? null);
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
                    if (string.IsNullOrEmpty(gamePlatformEnum) == false || searchSelection != null)
                    {
                        List<MediaModel> icons;
                        if (iconAssetSelection == "logos")
                        {
                            icons = services.getLogoImages(searchSelection ?? null, gamePlatformEnum ?? null, options.GameData.GameId ?? null);
                        }
                        else
                        {
                            icons = services.getIconImages(searchSelection ?? null, gamePlatformEnum ?? null, options.GameData.GameId ?? null);
                        }
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
            logger.Info("SGDBMetadataProvider GameData:" + options.GameData.ToString());
            if (searchSelection == null || gamePlatformEnum == null)
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
            convertPlayniteGamePluginIdToSGDBPlatformEnum(options.GameData.PluginId);

            if (!options.IsBackgroundDownload)
            {
                var logger = LogManager.GetLogger();
                logger.Info("GetSgdbMetadata");

                if (string.IsNullOrEmpty(gamePlatformEnum))
                {
                    var gameList = new List<GenericItemOption>(services.getGameListSGDB(options.GameData.Name).Select(game => new GenericItemOption(game.name, game.id.ToString())));
                    GetGame(gameList, "Choose Game");
                }
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
