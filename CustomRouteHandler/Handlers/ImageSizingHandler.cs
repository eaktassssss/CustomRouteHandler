using ImageMagick;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomRouteHandler.Handlers
{
    public class ImageSizingHandler
    {
        public RequestDelegate Handler(string filePath)
        {
            return async context =>
            {
                FileInfo file = new FileInfo($"{filePath}\\{context.Request.RouteValues["imageName"].ToString()}");
                using (MagickImage magick = new MagickImage(file))
                {
                    int width = 0; int height = 0;
                    width = magick.Width; height = magick.Height;
                    if (!String.IsNullOrEmpty(context.Request.Query["w"].ToString()))
                    {
                        width = int.Parse(context.Request.Query["w"].ToString());
                    }
                    if (!String.IsNullOrEmpty(context.Request.Query["h"].ToString()))
                    {
                        height = int.Parse(context.Request.Query["h"].ToString());
                    }
                    magick.Resize(width, height);
                    var buffer = magick.ToByteArray();
                    context.Response.Clear();
                    context.Response.ContentType = String.Concat($"image/{file.Extension.Replace(".", "")}");
                    await context.Response.Body.WriteAsync(buffer, 0, buffer.Length);
                    await context.Response.WriteAsync(filePath);
                }
            };
        }
    }
}
