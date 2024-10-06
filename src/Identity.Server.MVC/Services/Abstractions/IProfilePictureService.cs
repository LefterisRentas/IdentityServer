using System.Threading.Tasks;
using Identity.Server.MVC.Data.User;

namespace Identity.Server.MVC.Services.Abstractions;

public interface IProfilePictureService
{
    Task<ProfilePicture?> GetProfilePictureAsync(int profilePictureId);
    Task<ProfilePicture?> SaveProfilePictureAsync(ProfilePicture profilePicture);
    Task<bool> DeleteProfilePictureAsync(int profilePictureId);
}