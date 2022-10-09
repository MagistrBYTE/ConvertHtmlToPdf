using Microsoft.EntityFrameworkCore;

namespace Convert.DataLayer
{
    public class FileConvertedDbContextFactory : IDbContextFactory<FileConvertedDbContext>
    {
        private readonly IDbContextFactory<FileConvertedDbContext> mDbContextFactory;

        public FileConvertedDbContextFactory(IDbContextFactory<FileConvertedDbContext> dbContextFactory)
        {
            mDbContextFactory = dbContextFactory;
        }

        public FileConvertedDbContext CreateDbContext()
        {
            return mDbContextFactory.CreateDbContext();
        }
    }
}