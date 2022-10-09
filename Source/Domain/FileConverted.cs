using Hangfire;
using Hangfire.PostgreSql;
using ICSharpCode.SharpZipLib.Tar;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PuppeteerSharp;
using System;
using System.ComponentModel.DataAnnotations;

namespace Convert.Domain
{
	public class FileConverted
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(256)]
		public string Name { get; set; } = default!;

		public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

		public bool IsLoaded { get; set; }

		public bool IsConverted { get; set; }

		public string? Error { get; set; }


		public string GetUniqueFileNameOriginal()
		{
			return Id.ToString("D8") + "_" + Name;
		}

		public string GetUniqueFileNameConverted()
		{
			return Path.ChangeExtension(Id.ToString("D8") + "_" + Name, ".pdf");
		}
	}
}
