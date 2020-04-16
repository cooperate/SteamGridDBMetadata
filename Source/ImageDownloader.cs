using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;

namespace SGDBMetadata
{
    class ImageDownloader
    {
        private bool _disposed;
        private readonly HttpClient _httpClient;

        public ImageDownloader()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Downloads an image asynchronously from the <paramref name="uri"/> and places it in the specified <paramref name="directoryPath"/> with the specified <paramref name="fileName"/>.
        /// </summary>
        /// <param name="directoryPath">The relative or absolute path to the directory to place the image in.</param>
        /// <param name="fileName">The name of the file without the file extension.</param>
        /// <param name="uri">The URI for the image to download.</param>
        public async Task DownloadImage(string directoryPath, string fileName, Uri uri)
        {
            if (_disposed) { throw new ObjectDisposedException(GetType().FullName); }

            // Get the file extension
            var uriWithoutQuery = uri.GetLeftPart(UriPartial.Path);
            var fileExtension = Path.GetExtension(uriWithoutQuery);

            // Create file path and ensure directory exists
            var path = Path.Combine(directoryPath, $"{fileName}{fileExtension}");
            Directory.CreateDirectory(directoryPath);

            // Download the image and write to the file
            var imageBytes = await _httpClient.GetByteArrayAsync(uri);
            File.WriteAllBytes(path, imageBytes);
        }

        public void Dispose()
        {
            if (_disposed) { return; }
            _httpClient.Dispose();
            GC.SuppressFinalize(this);
            _disposed = true;
        }
    }
}
