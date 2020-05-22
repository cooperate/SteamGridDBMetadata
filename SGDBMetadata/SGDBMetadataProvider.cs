using Playnite.SDK;
using Playnite.SDK.Plugins;
using Playnite.SDK.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGDBMetadata;

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
        public SGDBMetadataProvider(MetadataRequestOptions options, SGDBMetadata plugin, string apiKey, string dimension, string style)
        {
            this.options = options;
            this.plugin = plugin;
            services = new SgdbServiceClient(apiKey, dimension, style);
            var logger = LogManager.GetLogger();
            logger.Info("SGDB Initialized");
        }

        // Override additional methods based on supported metadata fields.
        public override MetadataFile GetCoverImage()
        {
            var logger = LogManager.GetLogger();
            logger.Info("GetCoverImage");
            if (options.IsBackgroundDownload)
            {
                if (options.GameData.Source != null)
                {
                    return new MetadataFile(services.getCoverImageUrl(options.GameData.Name, options.GameData.Source.ToString().ToLower(), options.GameData.GameId));
                }
                else
                {
                    return new MetadataFile(services.getCoverImageUrl(options.GameData.Name));
                }
            } else {

                if (AvailableFields.Contains(MetadataField.Name))
                {
                    var covers = services.getCoverImages(searchSelection.Name);
                    var selection = GetCoverManually(covers);
                    if (selection == null || selection.Path == "nopath")
                    {
                        return base.GetCoverImage();
                    } else {
                        return new MetadataFile(selection.Path);
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
                if (options.GameData.Source != null)
                {
                    return new MetadataFile(services.getHeroImageUrl(options.GameData.Name, options.GameData.Source.ToString().ToLower(), options.GameData.GameId));
                }
                else
                {
                    return new MetadataFile(services.getHeroImageUrl(options.GameData.Name));
                }

            }
            else
            {
                if (AvailableFields.Contains(MetadataField.Name))
                {
                    var heroes = services.getHeroImages(searchSelection.Name);
                    var selection = GetHeroManually(heroes);
                    if (selection == null || selection.Path == "nopath")
                    {
                        return base.GetBackgroundImage();
                    }
                    else
                    {
                        return new MetadataFile(selection.Path);
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
                if (options.GameData.Source != null)
                {
                    return new MetadataFile(services.getLogoImageUrl(options.GameData.Name, options.GameData.Source.ToString().ToLower(), options.GameData.GameId));
                }
                else
                {
                    return new MetadataFile(services.getLogoImageUrl(options.GameData.Name));
                }
            }
            else
            {
                if (AvailableFields.Contains(MetadataField.Name))
                {
                    var icons = services.getLogoImages(searchSelection.Name);
                    var selection = GetIconManually(icons);
                    if (selection == null || selection.Path == "nopath")
                    {
                        return base.GetIcon();
                    }
                    else
                    {
                        return new MetadataFile(selection.Path);
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
            var fields = new List<MetadataField> { MetadataField.Name};
            fields.Add(MetadataField.Icon);
            fields.Add(MetadataField.BackgroundImage);
            fields.Add(MetadataField.CoverImage);
            return fields;
        }

        private void GetSgdbMetadata() {
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
                selection.Add(new ImageFileOption
                {
                    Path = cover.url
                });
            }
            if (selection.Count > 0)
            {
                return plugin.PlayniteApi.Dialogs.ChooseImageFile(
                    selection, "Choose Cover");
            } else
            {
                return new ImageFileOption("nopath");
            }
        }

        private ImageFileOption GetHeroManually(List<HeroModel> possibleHeroes)
        {
            var selection = new List<ImageFileOption>();
            foreach (var hero in possibleHeroes)
            {
                selection.Add(new ImageFileOption
                {
                    Path = hero.url
                });
            }
            if(selection.Count > 0)
            {
                return plugin.PlayniteApi.Dialogs.ChooseImageFile(
                selection, "Choose Background");
            } else
            {
                return new ImageFileOption("nopath");
            }
            
        }

        private ImageFileOption GetIconManually(List<MediaModel> possibleIcons)
        {
            var selection = new List<ImageFileOption>();
            foreach (var icon in possibleIcons)
            {
                selection.Add(new ImageFileOption
                {
                    Path = icon.url
                });
            }
            if(selection.Count > 0) { 
                return plugin.PlayniteApi.Dialogs.ChooseImageFile(
                selection, "Choose Icon");
            } else
            {
                return new ImageFileOption("nopath");
            }
}
    }
}