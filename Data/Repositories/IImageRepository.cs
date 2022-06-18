using ImageStore.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageStore.Data.Repositories
{
    public interface IImageRepository
    {
        Task<bool> AddImageAsync(Image image);
        Task<bool> DeleteImageByIdAsync(int id);
        Image GetImageById(int id);
        IEnumerable<Image> GetImages();
        Task<bool> UpdateCaptionAsync(int id, string value);
    }
}