using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Identity.Server.MVC.Models;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Identity.Server.MVC.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser> //, IClientStore, IResourceStore
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}