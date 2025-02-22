using CloudinaryDotNet.Actions;

namespace OnlineShop.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
    }
}
