
namespace Convert.FileStorage
{
    /// <summary>
    /// Управление локальными файлами
    /// </summary>
    public class LocalDirectoryFileStorage : IFileStorage
    {
        private readonly DirectoryInfo mBaseDirectoryInfo;

        public LocalDirectoryFileStorage(IConfiguration configuration)
        {
            mBaseDirectoryInfo = new DirectoryInfo(configuration["FileStorage"]);
            if (mBaseDirectoryInfo == null)
            {
                throw new ArgumentNullException(nameof(mBaseDirectoryInfo));
            }

            if (!mBaseDirectoryInfo.Exists)
                mBaseDirectoryInfo.Create();
        }

		/// <inheritdoc/>
		public async Task UploadAsync(IFormFile fileInput, string fileName, CancellationToken ct)
        {
            await UploadAsync(fileInput.OpenReadStream(), fileName, ct);
		}

		/// <inheritdoc/>
		public async Task UploadAsync(Stream stream, string fileName, CancellationToken ct)
        {
			var uploadPath = Path.Combine(mBaseDirectoryInfo.FullName, fileName);
			await using var fs = new FileStream(uploadPath, FileMode.Create, FileAccess.Write);
			await stream.CopyToAsync(fs, ct);
			await fs.FlushAsync(ct);
		}

		/// <inheritdoc/>
		public async Task<Stream?> GetFileAsync(string fileName)
		{
            var path = Path.Combine(mBaseDirectoryInfo.FullName, fileName);
            if(File.Exists(path))
            {
				return await Task.FromResult(File.OpenRead(path));
			}
            else
            {
                return null;

			}
		}

        /// <inheritdoc/>
        public Task DeleteAsync(string fileName)
        {
			var path = Path.Combine(mBaseDirectoryInfo.FullName, fileName);
			if (File.Exists(path))
			{
                File.Delete(path);
			}

            return Task.CompletedTask;
        }
    }
}