namespace Convert.FileStorage
{
	public interface IFileStorage
	{
		/// <summary>
		/// Загрузить файл
		/// </summary>
		/// <param name="fileInput">Загружаемый файл</param>
		/// <param name="fileName">Имя файла</param>
		/// <param name="ct">Токен отмены</param>
		/// <returns>Task</returns>
		Task UploadAsync(IFormFile fileInput, string fileName, CancellationToken ct);

		/// <summary>
		/// Загрузить из потока
		/// </summary>
		/// <param name="stream">Поток</param>
		/// <param name="fileName">Имя файла</param>
		/// <param name="ct">Токен отмены</param>
		/// <returns>Task</returns>
		Task UploadAsync(Stream stream, string fileName, CancellationToken ct);

		/// <summary>
		/// Получить поток файла по имени файла
		/// </summary>
		/// <param name="fileName">Имя файла</param>
		/// <returns>Поток файла</returns>
		Task<Stream?> GetFileAsync(string fileName);

		/// <summary>
		/// Удалить загруженный файл
		/// </summary>
		/// <param name="fileName">Имя файла</param>
		/// <returns>Task</returns>
		Task DeleteAsync(string fileName);
	}
}