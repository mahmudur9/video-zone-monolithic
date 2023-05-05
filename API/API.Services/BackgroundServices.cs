using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Services
{
    public class BackgroundServices : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string images = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            string videos = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos");
            string thumbnails = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/thumbnails");
            if (!Directory.Exists(images))
            {
                Directory.CreateDirectory(images);
            }
            if (!Directory.Exists(videos))
            {
                Directory.CreateDirectory(videos);
            }
            if (!Directory.Exists(thumbnails))
            {
                Directory.CreateDirectory(thumbnails);
            }

            return Task.CompletedTask;
        }
    }
}
