
using Convert.DataLayer;
using Convert.Domain;

namespace Convert.Info
{
	public class InfoService : IInfoService
	{
		private readonly FileConvertedDbContextFactory mContextFactory;
		private readonly IConfiguration mConfiguration;
		private string mPath;

		public InfoService(FileConvertedDbContextFactory contextFactory, IConfiguration configuration)
		{
			mContextFactory = contextFactory;
			mConfiguration = configuration;
			mPath = Path.Combine(mConfiguration.GetValue<string>("UrlApi"), mConfiguration.GetValue<string>("FileStorage"));
			mPath = mPath.Replace("wwwroot/", "");
		}

		public string GetInfo(int id)
		{
			var context = mContextFactory.CreateDbContext();

			var file = context.FileConverteds.Where(x => x.Id == id).FirstOrDefault();
			if(file is null)
			{
				return "<p>Идентификатор запроса не найден</p>";
			}

			if(file.IsLoaded == false)
			{
				return "<p>Файл загружается для обработки</p>";
			}

			if (file.IsLoaded && file.IsConverted == false)
			{
				return "<p>Файл обрабатывается</p>";
			}

			if (file.IsLoaded && file.IsConverted)
			{
				string url = Path.Combine(mPath, file.GetUniqueFileNameConverted());
				string name = Path.ChangeExtension(file.Name, ".pdf");

				return $"<p>Конвертация файла выполнена</p><p>Скачать файл <a href='{url}' download>{name}</a></p>";
			}

			return string.Empty;
		}
	}
}