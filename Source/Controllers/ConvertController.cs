using Convert.FilePreparation;
using Convert.Info;
using Microsoft.AspNetCore.Mvc;

namespace Convert.Controllers
{
	public class ConvertInput
	{
		public IFormFile fileHtml { set; get; } = default!;
	}

	[Route("api/[controller]")]
	[ApiController]
	public class ConvertController : Controller
	{
		private readonly IInfoService mInfoService;
		private readonly IFilePreparationService mFilePreparationService;

		public ConvertController(IInfoService infoService, IFilePreparationService filePreparationService)
		{
			mInfoService = infoService;
			mFilePreparationService = filePreparationService;
		}

		[HttpPost("HtmlToPdf")]
		[Consumes("multipart/form-data")]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
		public IActionResult HtmlToPdf([FromForm] ConvertInput input)
		{
			try
			{
				int id = mFilePreparationService.PreparationFile(input.fileHtml);
				return Content(id.ToString());
			}
			catch (Exception exc)
			{
				return BadRequest(exc.ToString());
			}
		}

		[HttpGet("GetInfo")]
		[ProducesResponseType(typeof(String), StatusCodes.Status200OK)]
		public IActionResult GetInfo([FromQuery] int id)
		{
			try
			{
				var result = mInfoService.GetInfo(id);
				return Ok(result);
			}
			catch (Exception exc)
			{
				return BadRequest(exc.ToString());
			}
		}
	}
}
