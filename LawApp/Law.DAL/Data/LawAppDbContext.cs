using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;
using Microsoft.EntityFrameworkCore;

namespace Law.DAL.Data
{
    public class LawAppDbContext:DbContext
    {
        public DbSet<Issue> issues {  get; set; }
        public DbSet<Court> courts { get; set; }
        public DbSet<Center> centers { get; set; }  
        public DbSet<Client> clients { get; set; }
        public DbSet<Party> parties { get; set; }
        public DbSet<IssueFile> issuesFiles { get; set; }
        public DbSet<CourtSession> courtsSessions { get; set; }
        public DbSet<Receipt> receipts { get; set; }
        public DbSet<Payment> payments { get; set; }    
        public LawAppDbContext(DbContextOptions<LawAppDbContext> options):base(options) 
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Issue>()
                .HasIndex(n => n.Code)
          .IsUnique();
            modelBuilder.Entity<Court>()
                .HasIndex(n => n.Code)
          .IsUnique();
            modelBuilder.Entity<Center>()
               .HasIndex(n => n.Code)
         .IsUnique();
            modelBuilder.Entity<Client>()
            .HasIndex(n => n.Code)
      .IsUnique();
            modelBuilder.Entity<Party>()
           .HasIndex(n => n.Code)
     .IsUnique();
            modelBuilder.Entity<IssueFile>()
          .HasIndex(n => n.Code)
    .IsUnique();
            modelBuilder.Entity<CourtSession>()
        .HasIndex(n => n.Code)
  .IsUnique();
            modelBuilder.Entity<Receipt>()
       .HasIndex(n => n.Code)
 .IsUnique();
            modelBuilder.Entity<Payment>()
      .HasIndex(n => n.Code)
.IsUnique();
            base.OnModelCreating(modelBuilder);
        }
    }
}
