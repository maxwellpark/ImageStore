using ImageStore.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageStore.Data.Repositories
{
    public interface IImageRepository
    {
        Task<bool> AddImageAsync(Image image);
        IEnumerable<Image> GetImages();
    }
}