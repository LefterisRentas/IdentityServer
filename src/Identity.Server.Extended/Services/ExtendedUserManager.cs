using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Identity.Server.Extended.Services;

/// <summary>
/// Extended user manager to add functionality for managing user claims, including duplicate cleanup.
/// </summary>
/// <typeparam name="TUser">The type of user entity.</typeparam>
/// <typeparam name="TIdentityDbContext">The type of Identity DbContext.</typeparam>
public class ExtendedUserManager<TUser, TIdentityDbContext> : UserManager<TUser> 
    where TUser : IdentityUser 
    where TIdentityDbContext : IdentityDbContext<TUser>
{
    private readonly TIdentityDbContext _context;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ExtendedUserManager{TUser, TIdentityDbContext}"/> class.
    /// </summary>
    public ExtendedUserManager(
        IUserStore<TUser> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<TUser> passwordHasher,
        IEnumerable<IUserValidator<TUser>> userValidators,
        IEnumerable<IPasswordValidator<TUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<TUser>> logger,
        TIdentityDbContext context) 
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        _context = context;
    }

    /// <summary>
    /// Removes specified claims from a user, ensuring duplicates are cleaned up.
    /// </summary>
    /// <param name="user">The user from whom claims are to be removed.</param>
    /// <param name="claims">The claims to be removed from the user.</param>
    /// <returns>An <see cref="IdentityResult"/> indicating the success or failure of the operation.</returns>
    public override async Task<IdentityResult> RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims)
    {
        // Retrieve only matching claims for removal to minimize data load
        var claimsToRemove = await _context.UserClaims
            .Where(x => x.UserId == user.Id && claims.Any(c => c.Type == x.ClaimType && c.Value == x.ClaimValue))
            .ToListAsync();

        if (claimsToRemove.Any())
        {
            _context.UserClaims.RemoveRange(claimsToRemove);
            await _context.SaveChangesAsync();
        }

        return await base.RemoveClaimsAsync(user, claims);
    }

    /// <summary>
    /// Adds or updates a claim for a user, then removes any duplicate claims.
    /// </summary>
    /// <param name="user">The user to whom the claim is to be added or updated.</param>
    /// <param name="claim">The claim to add or update for the user.</param>
    /// <returns>An <see cref="IdentityResult"/> indicating the success or failure of the operation.</returns>
    public override async Task<IdentityResult> AddClaimAsync(TUser user, Claim claim)
    {
        // Use a single query to load only relevant claims
        var existingClaim = await _context.UserClaims
            .FirstOrDefaultAsync(x => x.UserId == user.Id && x.ClaimType == claim.Type);

        if (existingClaim != null)
        {
            if (existingClaim.ClaimValue != claim.Value)
            {
                existingClaim.ClaimValue = claim.Value;
            }
        }
        else
        {
            _context.UserClaims.Add(new IdentityUserClaim<string>
            {
                UserId = user.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            });
        }

        await _context.SaveChangesAsync();
        await CleanupUserDuplicateClaims(user.Id);

        return IdentityResult.Success;
    }

    /// <summary>
    /// Adds or updates claims for a user, then removes any duplicate claims.
    /// </summary>
    /// <param name="user">The user to whom the claims are to be added or updated.</param>
    /// <param name="claims">The claims to add or update for the user.</param>
    /// <returns>An <see cref="IdentityResult"/> indicating the success or failure of the operation.</returns>
    public override async Task<IdentityResult> AddClaimsAsync(TUser user, IEnumerable<Claim> claims)
    {
        // Load existing claims once and process all updates/additions in-memory
        var userClaims = await _context.UserClaims
            .Where(x => x.UserId == user.Id)
            .ToListAsync();

        foreach (var claim in claims)
        {
            var existingClaim = userClaims.FirstOrDefault(x => x.ClaimType == claim.Type);

            if (existingClaim != null)
            {
                if (existingClaim.ClaimValue != claim.Value)
                {
                    existingClaim.ClaimValue = claim.Value;
                }
            }
            else
            {
                _context.UserClaims.Add(new IdentityUserClaim<string>
                {
                    UserId = user.Id,
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                });
            }
        }

        await _context.SaveChangesAsync();
        await CleanupUserDuplicateClaims(user.Id);

        return IdentityResult.Success;
    }

    /// <summary>
    /// Cleans up duplicate claims for a user, leaving only one instance of each unique claim type and value.
    /// </summary>
    /// <param name="userId">The ID of the user whose claims to check for duplicates.</param>
    private async Task CleanupUserDuplicateClaims(string userId)
    {
        // Load all claims for the user from the database with tracking
        var userClaims = await _context.UserClaims
            .Where(x => x.UserId == userId)
            .ToListAsync();

        // Identify duplicates on the client side
        var duplicatesToRemove = userClaims
            .GroupBy(x => new { x.ClaimType })
            .SelectMany(g => g.Skip(1)) // Retain only the first instance and mark the rest for removal
            .ToList();
        
        if (duplicatesToRemove.Count > 0)
        {
            _context.UserClaims.RemoveRange(duplicatesToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
