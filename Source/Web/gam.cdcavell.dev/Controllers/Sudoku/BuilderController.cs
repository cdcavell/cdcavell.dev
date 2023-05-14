using ClassLibrary.Mvc.Localization;
using ClassLibrary.Mvc.Services.AppSettings;
using gam.cdcavell.dev.Models.Sudoku;
using gam.cdcavell.dev.Services.Sudoku;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace gam.cdcavell.dev.Controllers.Sudoku
{
	/// <summary>
	/// Sudoku Builder controller class
	/// </summary>
	/// <revision>
	/// __Revisions:__~~
	/// | Contributor | Build | Revison Date | Description |~
	/// |-------------|-------|--------------|-------------|~
	/// | Christopher D. Cavell | 1.0.5.0 | 05/13/2023 | Game Development - Sudoku |~ 
	/// </revision>
	[Route("Sudoku/[controller]")]
	public class BuilderController : ApplicationBaseController<BuilderController>
    {
        private readonly IPuzzleSaver puzzleSaver;
        private readonly IPuzzleService puzzleService;

		/// <summary>
		/// Constructor method
		/// </summary>
		/// <param name="logger">ILogger</param>
		/// <param name="webHostEnvironment">IWebHostEnvironment</param>
		/// <param name="httpContextAccessor">IHttpContextAccessor</param>
		/// <param name="appSettingsService">IAppSettingsService</param>
		/// <param name="localizer">IStringLocalizer&lt;T&gt;</param>
		/// <param name="sharedLocalizer">IStringLocalizer&lt;SharedResource&gt;</param>
		/// <param name="saver">IPuzzleSaver</param>
		/// <param name="service">IPuzzleService</param>
		/// <method>
		/// public BuilderController(
		///     ILogger&lt;BuilderController&gt; logger,
		///     IWebHostEnvironment webHostEnvironment,
		///     IHttpContextAccessor httpContextAccessor,
		///     IAppSettingsService appSettingsService,
		///     IStringLocalizer&lt;BuilderController&gt; localizer,
		///     IStringLocalizer&lt;SharedResource&gt; sharedLocalizer,
		///     IPuzzleSaver saver,
		///     IPuzzleService service
		/// ) : base(logger, webHostEnvironment, httpContextAccessor, appSettingsService, localizer, sharedLocalizer)
		/// </method>
		public BuilderController(
            ILogger<BuilderController> logger,
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            IAppSettingsService appSettingsService,
            IStringLocalizer<BuilderController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer,
            IPuzzleSaver saver,
            IPuzzleService service
        ) : base(logger, webHostEnvironment, httpContextAccessor, appSettingsService, localizer, sharedLocalizer)
        {
            puzzleSaver = saver;
            puzzleService = service;
        }

		/// <summary>
		/// Save puzzle setup HttpPost method
		/// </summary>
		/// <param name="board">FullBoard</param>
		/// <returns>ActionResult</returns>
		[AllowAnonymous]
        [HttpPost("SavePuzzleSetup")]
        [ValidateAntiForgeryToken]
        public ActionResult SavePuzzleSetup(FullBoard board)
        {
            int puzzleNumber = board.BoardNumber;
            puzzleSaver.SavePuzzleSetup(board.BoardList, ref puzzleNumber);
            board.BoardNumber = puzzleNumber;
            board.Status = puzzleService.GetPuzzleStatus(board.BoardList);

            // Return the model's updated BoardNumber by redirecting to an HttpGet method
            TempData["Board"] = board;
            return RedirectToAction("SavePuzzleSetup");
        }

        /// <summary>
        /// Save puzzle setup HttpGet method
        /// </summary>
		/// <returns>ActionResult</returns>
        [AllowAnonymous]
        [HttpGet("SavePuzzleSetup")]
        public ActionResult SavePuzzleSetup()
        {
            var fullBoard = new FullBoard();
            return View(Constants.CustomViewPath("BuilderView"), (FullBoard)(TempData["Board"] = fullBoard));
        }
    }
}
