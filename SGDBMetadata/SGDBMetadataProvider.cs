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
        public override List<MetadataField> AvailableFields
        {
            get
            {
                return SupportedFields;
            }
        }


        public List<MetadataField> SupportedFields { get; } = new List<MetadataField>
        {
            MetadataField.CoverImage
        };
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
                return new MetadataFile(services.getCoverImageUrl(options.GameData.Name));
            } else {
                var sgdbException = new Exception("Service failure.");
                throw sgdbException;
            }
        }
/*
        public override string GetBackgroundImage()
        {
            if (options.IsBackgroundDownload)
            {
                return new MetadataFile(services.GetHeroImageUrl(options.Game.Name));
            } else {
                return false;
            }
        }*/
    }
}