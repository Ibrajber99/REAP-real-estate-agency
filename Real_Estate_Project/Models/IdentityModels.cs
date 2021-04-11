using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.Models.Domain_Models;
using Real_Estate_Project.Models.Domain_Models.Customer_Models;
using Real_Estate_Project.Models.Domain_Models.Listing_Models;
using Real_Estate_Project.Models.Domain_Models.OperatingUser_Models;
using Real_Estate_Project.Models.Domain_Models.Viewing_Models;
using Real_Estate_Project.Models.Listing_Models;

namespace Real_Estate_Project.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public virtual OperatingUser registeredUser { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<OperatingUser> siteUsers { get; set; }

        public DbSet<Listing> Listing { get; set; }

        public DbSet<PropertyFeatures> ListingFeatures { get; set; }

        public DbSet<Heating> HeatingType { get; set; }

        public DbSet<Viewing> Viewings { get; set; }

        public DbSet<CustomerAddress> CustomerAddress { get; set; }

        public DbSet<ListingAddress> ListingAddresses { get; set; }

        public DbSet<ListingImage> ListingImages { get; set; }

        public DbSet<OperatingUserImage> UsersProfileImages { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OperatingUser>()
                 .HasOptional(user => user.UserCreator)
                 .WithMany().HasForeignKey(user => user.UserCreatorId);

            modelBuilder.Entity<OperatingUser>()
                 .HasOptional(user => user.UserUpdator)
                 .WithMany().HasForeignKey(user => user.UserUpdatorId);


            modelBuilder.Entity<Listing>().HasRequired(a => a.Customer)
                .WithMany(c => c.Listings);

            modelBuilder.Entity<Viewing>().HasRequired(l => l.Listing).
                 WithMany(v => v.Viewings).WillCascadeOnDelete(false);

            modelBuilder.Entity<Viewing>().HasRequired(c => c.Customer).
                WithMany(v => v.Viewings).WillCascadeOnDelete(false);

            modelBuilder.Entity<Viewing>().HasRequired(u => u.ViewingHost).
                WithMany(v => v.Viewings).WillCascadeOnDelete(false);

        }
    }
}