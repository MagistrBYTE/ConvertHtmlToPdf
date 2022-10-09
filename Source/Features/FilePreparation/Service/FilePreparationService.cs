using Convert.DataLayer;
using Convert.Domain;
using Convert.FileStorage;
using Converter.ConverterFile;
using Hangfire;
using PuppeteerSharp;

namespace Convert.FilePreparation
{
	public class FilePreparationService : IFilePreparationService
	{
		private readonly FileConvertedDbContextFactory mContextFactory;
		private readonly IFileStorage mFileStorage;

		public FilePreparationService(FileConvertedDbContextFactory contextFactory, IFileStorage fileStorage)
		{
			mContextFactory = contextFactory;
			mFileStorage = fileStorage;
		}

		public int PreparationFile(IFormFile formFile)
		{
			var context = mContextFactory.CreateDbContext();

			FileConverted file = new FileConverted();
			file.Name = formFile.FileName;

			context.FileConverteds.Add(file);
			context.SaveChanges();

			mFileStorage.UploadAsync(formFile, file.GetUniqueFileNameOriginal(), CancellationToken.None);

			file.IsLoaded = true;
			context.FileConverteds.Update(file);
			context.SaveChanges();

			BackgroundJob.Enqueue<IConverterFileService>(x => x.ConvertAsync(file.Id));

			return file.Id;
		}
	}
}