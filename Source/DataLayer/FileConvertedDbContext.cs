using Convert.Domain;
using Microsoft.EntityFrameworkCore;

namespace Convert.DataLayer
{
	public class FileConvertedDbContext : DbContext
	{
		public DbSet<FileConverted> FileConverteds { get; set; } = null!;

		public FileConvertedDbContext(DbContextOptions<FileConvertedDbContext> options) 
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
		}
	}
}
