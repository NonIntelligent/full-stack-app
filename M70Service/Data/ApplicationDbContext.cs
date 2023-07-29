using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace M70Service.Data
{
    // Create models from data structures to insert into database tables
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Operation> operationModel { get; set; }
        public DbSet<EquipmentUsed> equipmentModel { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }

        // Create default models and for operation records
        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<Operation>().ToTable("OperationHistory");
            builder.Entity<EquipmentUsed>().ToTable("EquipmentLog");
        }
    }
}