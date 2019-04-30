using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TShirtMe
{
    public class EntryEntity
    {
        [Key]
        public string EntryCode { get; set; }
        public string PhoneNumber { get; set; }

        public bool Claimed { get; set; }
        
    }

    public class TShirtMeContext : DbContext
    {
        public TShirtMeContext(DbContextOptions<TShirtMeContext> options)
            : base(options)
        {
        }

        public DbSet<EntryEntity> Entries { get; set; }
    }
}