﻿using ImageStore.Data.DbContexts;
using ImageStore.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageStore.Data.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ImageContext _context;
        private readonly ILogger<ImageRepository> _logger;

        public ImageRepository(ImageContext context, ILogger<ImageRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<Image> GetImages()
        {
            _logger.LogInformation("Getting images...");
            var images = _context.Images.OrderBy(image => image.Name);
            return images;
        }

        public async Task<bool> AddImageAsync(Image image)
        {
            _context.Add(image);
            var changeCount = await _context.SaveChangesAsync();
            return changeCount > 0;
        }
    }
}