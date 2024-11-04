using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Identity.Server.Extended.Services;

public class ExtendedUserManager<TUser, TIdentityDbConext> : UserManager<TUser> where TUser : IdentityUser where TIdentityDbConext : IdentityDbContext<TUser>
{
    private readonly TIdentityDbConext _context;
    
    public ExtendedUserManager(IUserStore<TUser> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<TUser> passwordHasher,
        IEnumerable<IUserValidator<TUser>> userValidators,
        IEnumerable<IPasswordValidator<TUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<TUser>> logger,
        TIdentityDbConext context) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        _context = context;
    }

    public override async Task<IdentityResult> RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims)
    {
        // Load all claims for the user into memory
        var userClaims = await _context.UserClaims
            .Where(x => x.UserId == user.Id)
            .ToListAsync();

        await CleanupUserDuplicateClaims(userClaims);

        // Proceed with removing specified claims as usual
        var claimsToRemove = userClaims
            .Where(x => claims.Any(c => c.Type == x.ClaimType && c.Value == x.ClaimValue))
            .ToList();

        _context.UserClaims.RemoveRange(claimsToRemove); // Remove specified claims in bulk
        _context.SaveChanges();

        return await base.RemoveClaimsAsync(user, claims);
    }


    public override async Task<IdentityResult> AddClaimAsync(TUser user, Claim claim)
    {
        var result = await base.AddClaimAsync(user, claim);
        var userClaims = await _context.UserClaims
            .Where(x => x.UserId == user.Id)
            .ToListAsync();
        await CleanupUserDuplicateClaims(userClaims);
        return result;
    }

    private async Task CleanupUserDuplicateClaims(List<IdentityUserClaim<string>> userClaims)
    {
        // Identify duplicate claims for cleanup
        var duplicatesToRemove = userClaims
            .GroupBy(x => new { x.ClaimType, x.ClaimValue })
            .SelectMany(g => g.Skip(1)) // Keep only one instance, mark duplicates for removal
            .ToList();

        if (duplicatesToRemove.Count == 0)
        {
            return;
        }
        // Remove the duplicates from the database
        _context.UserClaims.RemoveRange(duplicatesToRemove);
        //Using the synchronous SaveChanges method to avoid 
        await _context.SaveChangesAsync();
    }
}