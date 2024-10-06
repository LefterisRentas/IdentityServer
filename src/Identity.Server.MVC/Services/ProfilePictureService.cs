using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Identity.Server.MVC.Data;
using Identity.Server.MVC.Data.User;
using Identity.Server.MVC.Models.Validation;
using Identity.Server.MVC.Services.Abstractions;

namespace Identity.Server.MVC.Services;

public class ProfilePictureService(ApplicationDbContext context) : IProfilePictureService
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public Task<ProfilePicture?> GetProfilePictureAsync(int profilePictureId)
    {
        var profilePicture = _context.ProfilePictures.FirstOrDefault(x => x.Id == profilePictureId);
        return Task.FromResult(profilePicture);
    }

    public async Task<ProfilePicture?> SaveProfilePictureAsync(ProfilePicture profilePicture)
    {
        var validationResult = Validate(profilePicture);
        if (!validationResult.IsValid)
        {
            throw new InvalidOperationException(validationResult.ErrorMessage);
        }
        _context.ProfilePictures.Add(profilePicture);
        await _context.SaveChangesAsync();
        return profilePicture;
    }

    public Task<bool> DeleteProfilePictureAsync(int profilePictureId)
    {
        var profilePicture = _context.ProfilePictures.FirstOrDefault(x => x.Id == profilePictureId);
        if (profilePicture is null)
        {
            return Task.FromResult(false);
        }
        var userWithProfilePicture = _context.Users.FirstOrDefault(x => x.ProfilePictureId == profilePictureId);
        if (userWithProfilePicture is not null)
        {
            userWithProfilePicture.ProfilePictureId = null;
            _context.Users.Update(userWithProfilePicture);
        }
        _context.ProfilePictures.Remove(profilePicture);
        _context.SaveChanges();
        return Task.FromResult(true);
    }
    
    private ValidationResult Validate(ProfilePicture profilePicture)
    {
        var validationResult = new ValidationResult { IsValid = true };
        if (profilePicture.Data != Array.Empty<byte>() && profilePicture.Data.Length != 0) return validationResult;
        validationResult.IsValid = false;
        validationResult.ErrorMessage = "Profile picture is empty.";
        if (ValidateImageValidateContentType(profilePicture.ContentType))
        {
            
        }
        return validationResult;
    }
    
    private bool ValidateImageValidateContentType(string contentType)
    {
        return contentType switch
        {
            MediaTypeNames.Image.Jpeg => true,
            MediaTypeNames.Image.Icon => true,
            MediaTypeNames.Image.Gif => true,
            MediaTypeNames.Image.Webp => true,
            MediaTypeNames.Image.Bmp => true,
            MediaTypeNames.Image.Png => true,
            MediaTypeNames.Image.Tiff => false,
            _ => false
        };
    }
}