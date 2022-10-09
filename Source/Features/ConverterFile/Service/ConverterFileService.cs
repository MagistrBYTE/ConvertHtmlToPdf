using Convert.DataLayer;
using Convert.Domain;
using Convert.FileStorage;
using Hangfire;
using PuppeteerSharp;

namespace Converter.ConverterFile
{
	public class ConverterFileService : IConverterFileService
	{
		private readonly FileConvertedDbContextFactory mContextFactory;
		private readonly IFileStorage mFileStorage;

		public ConverterFileService(FileConvertedDbContextFactory contextFactory, IFileStorage fileStorage)
		{
			mContextFactory = contextFactory;
			mFileStorage = fileStorage;
		}

		public async Task ConvertAsync(int id)
		{
			var context = mContextFactory.CreateDbContext();
			var file = context.FileConverteds.Where(x => x.Id == id).FirstOrDefault();

			if (file is null)
			{
				throw new NullReferenceException($"���� �� ���������� id={id} ����������");
			}

			if (file.IsLoaded == false)
			{
				throw new Exception($"���� �� ��������, id={id}");
			}

			var stream = await mFileStorage.GetFileAsync(file.GetUniqueFileNameOriginal());
			if (stream is null)
			{
				throw new Exception($"�� ������� �������� ����� ��� �����, id={id}");
			}

			var reader = new StreamReader(stream);
			var htmlcontent = reader.ReadToEnd();

			var browserFetcher = new BrowserFetcher();
			await browserFetcher.DownloadAsync();

			await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });

			await using var page = await browser.NewPageAsync();
			await page.SetContentAsync(htmlcontent);

			Stream outStream = await page.PdfStreamAsync();

			if (outStream is null)
			{
				throw new Exception($"�� ��������������� ���� � PDF, id={id}");
			}

			await mFileStorage.UploadAsync(outStream, file.GetUniqueFileNameConverted(), CancellationToken.None);

			file.IsConverted = true;
			context.FileConverteds.Update(file);
			context.SaveChanges();
		}
	}
}