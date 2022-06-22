using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ImageCachingMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IMemoryCache _cache;
        private static string _cachePath = $"{AppContext.BaseDirectory}";
        private static TimeSpan _slidingExpiration;
        private static readonly TimeSpan _DefaultslidingExpiration = TimeSpan.FromSeconds(60);

        private static readonly string _defaultCacheDirectory = "ImageCache";

        public ImageCachingMiddleware(RequestDelegate next, string path, int maxItems, TimeSpan? slidingExpiration = null)
        {
            _next = next;

            _cache = new MemoryCache(new MemoryCacheOptions()
            {
                SizeLimit = maxItems
            });
            _cachePath = Path.Combine(_cachePath, path ?? _defaultCacheDirectory);
            _slidingExpiration = slidingExpiration ?? _DefaultslidingExpiration;

            Directory.CreateDirectory(_cachePath);
        }

        public async Task Invoke(HttpContext httpContext)
        {
            Stream originalBody = httpContext.Response.Body;

            if (_cache.TryGetValue(httpContext.Request.Path, out var cachedImageName))
            {
                httpContext.Response.ContentType = "image/jpg";
                using var cachedImageStream = File.OpenRead($"{_cachePath}/{cachedImageName}");
                await cachedImageStream.CopyToAsync(httpContext.Response.Body);
                return;
            }

            try
            {
                using var memStream = new MemoryStream();
                httpContext.Response.Body = memStream;

                await _next(httpContext);

                memStream.Position = 0;
                await memStream.CopyToAsync(originalBody);

                if (IsImage(httpContext.Response.ContentType))
                {
                    var fileName = $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}.jpg";

                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetSize(1)
                        .SetSlidingExpiration(_slidingExpiration)
                        .RegisterPostEvictionCallback(OnCachedImageExpired, this);
                    _cache.Set(httpContext.Request.Path, fileName, cacheOptions);

                    using var fileStream = File.Create($"{_cachePath}/{fileName}");
                    memStream.Position = 0;
                    await memStream.CopyToAsync(fileStream);
                }
            }
            finally
            {
                httpContext.Response.Body = originalBody;
            }
        }

        private static bool IsImage(string contentType)
        {
            return contentType != null &&
                (contentType.ToLower() == "image/jpg" ||
                contentType.ToLower() == "image/jpeg" ||
                contentType.ToLower() == "image/pjpeg" ||
                contentType.ToLower() == "image/gif" ||
                contentType.ToLower() == "image/x-png" ||
                contentType.ToLower() == "image/png");
        }

        private static void OnCachedImageExpired(object key, object value, EvictionReason reason, object state)
        {
            var imagePath = Path.Combine(_cachePath, value.ToString());

            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ImageCachingMiddlewareExtensions
    {
        public static IApplicationBuilder UseImageCachingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageCachingMiddleware>();
        }
    }
}
