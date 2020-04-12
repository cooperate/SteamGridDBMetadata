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
                if (availableFields == null)
                {
                    availableFields = GetAvailableFields();
                }

                return availableFields;
            }
        }
        public SGDBMetadataProvider(MetadataRequestOptions options, SGDBMetadata plugin, string apiKey)
        {
            this.options = options;
            this.plugin = plugin;
            services = new SgdbServiceClient(apiKey);
        }

        // Override additional methods based on supported metadata fields.
        public override MetadataFile GetCoverImage()
        {
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
                var gameList = new List<GenericItemOption>(services.getGameListSGDB(options.GameData.Name).Select(game => new GenericItemOption(game.name, game.id.ToString())));
                GetGame(gameList, "Choose Game For Cover");

                if (searchSelection != null)
                {
                    var covers = services.getCoverImages(searchSelection.Name);
                    var selection = GetCoverManually(covers);
                    return new MetadataFile(selection.Path);
                }
                else
                {
                    var sgdbException = new Exception("Service failure.");
                    throw sgdbException;
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
                var gameList = new List<GenericItemOption>(services.getGameListSGDB(options.GameData.Name).Select(game => new GenericItemOption(game.name, game.id.ToString())));
                GetGame(gameList, "Choose Game For Background");

                if (searchSelection != null)
                {
                    var heroes = services.getHeroImages(searchSelection.Name);
                    var selection = GetHeroManually(heroes);
                    return new MetadataFile(selection.Path);
                }
                else
                {
                    var sgdbException = new Exception("Service failure.");
                    throw sgdbException;
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
                var gameList = new List<GenericItemOption>(services.getGameListSGDB(options.GameData.Name).Select(game => new GenericItemOption(game.name, game.id.ToString())));
                GetGame(gameList, "Choose Game For Icon");

                if (searchSelection != null)
                {
                    var icons = services.getLogoImages(searchSelection.Name);
                    var selection = GetIconManually(icons);
                    return new MetadataFile(selection.Path);
                }
                else
                {
                    var sgdbException = new Exception("Service failure.");
                    throw sgdbException;
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
            this.searchSelection = item;
        }

        private List<MetadataField> GetAvailableFields()
        {
            var fields = new List<MetadataField> { MetadataField.CoverImage };
            fields.Add(MetadataField.Icon);
            fields.Add(MetadataField.BackgroundImage);
            return fields;
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
            return plugin.PlayniteApi.Dialogs.ChooseImageFile(
                selection, "Choose Cover");
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
            return plugin.PlayniteApi.Dialogs.ChooseImageFile(
                selection, "Choose Background");
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
            return plugin.PlayniteApi.Dialogs.ChooseImageFile(
                selection, "Choose Icon");
        }
    }
}