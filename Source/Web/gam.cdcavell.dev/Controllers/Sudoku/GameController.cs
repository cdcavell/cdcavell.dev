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
	/// Sudoku Game controller class
	/// </summary>
	/// <revision>
	/// __Revisions:__~~
	/// | Contributor | Build | Revison Date | Description |~
	/// |-------------|-------|--------------|-------------|~
	/// | Christopher D. Cavell | 1.0.5.0 | 05/14/2023 | Game Development - Sudoku |~ 
	/// </revision>
	[Route("Sudoku/[controller]")]
	public class GameController : ApplicationBaseController<GameController>
    {
        private readonly IPuzzleLoader puzzleLoader;
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
		/// <param name="loader">IPuzzleLoader</param>
		/// <param name="saver">IPuzzleSaver</param>
		/// <param name="service">IPuzzleService</param>
		/// <method>
		/// public GameController(
		///     ILogger&lt;GameController&gt; logger,
		///     IWebHostEnvironment webHostEnvironment,
		///     IHttpContextAccessor httpContextAccessor,
		///     IAppSettingsService appSettingsService,
		///     IStringLocalizer&lt;GameController&gt; localizer,
		///     IStringLocalizer&lt;SharedResource&gt; sharedLocalizer,
		///     IPuzzleLoader loader,
		///     IPuzzleSaver saver, 
		///     IPuzzleService service
		/// ) : base(logger, webHostEnvironment, httpContextAccessor, appSettingsService, localizer, sharedLocalizer)
		/// </method>
		public GameController(
            ILogger<GameController> logger,
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            IAppSettingsService appSettingsService,
            IStringLocalizer<GameController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer,
            IPuzzleLoader loader, 
            IPuzzleSaver saver, 
            IPuzzleService service
        ) : base(logger, webHostEnvironment, httpContextAccessor, appSettingsService, localizer, sharedLocalizer)
        {
            puzzleLoader = loader;
            puzzleSaver = saver;
            puzzleService = service;
        }

		/// <summary>
		/// New Game
		/// </summary>
		/// <returns>ActionResult</returns>
		[AllowAnonymous]
        [HttpGet("NewGame")]
		public ActionResult NewGame()
        {
            FullBoard board = new() { BoardList = puzzleService.SetupBoard() };
            puzzleLoader.LoadNewPuzzle(board.BoardList, out int puzzleNumber);
            board.BoardNumber = puzzleNumber;

            board.BoardList
                .Where(x => x.Value != null)
                .ToList()
                .ForEach(x => x.CellLocked = true);

            return View(Constants.CustomViewPath("GameView"), board);
        }

        /// <summary>
        /// Update Game
        /// </summary>
        /// <param name="board">FullBoard</param>
		/// <returns>ActionResult</returns>
        [AllowAnonymous]
        [HttpPost("UpdateGame")]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGame(FullBoard board)
        {
            board.Status = puzzleService.GetPuzzleStatus(board.BoardList);
            return View(Constants.CustomViewPath("GameView"), board);
        }

        /// <summary>
        /// Reset Game
        /// </summary>
        /// <param name="board">FullBoard</param>
		/// <returns>ActionResult</returns>
        [AllowAnonymous]
        [HttpPost("ResetGame")]
		[ValidateAntiForgeryToken]
		public ActionResult ResetGame(FullBoard board)
        {
            board.BoardList = puzzleService.SetupBoard();
            puzzleLoader.ReloadPuzzle(board.BoardList, board.BoardNumber);

			board.BoardList
				.Where(x => x.Value != null)
				.ToList()
				.ForEach(x => x.CellLocked = true);
			
            return View(Constants.CustomViewPath("GameView"), board);
        }

        /// <summary>
        /// Save Game
        /// </summary>
        /// <param name="board">FullBoard</param>
		/// <returns>ActionResult</returns>
        [AllowAnonymous]
        [HttpPost("SaveGame")]
		[ValidateAntiForgeryToken]
		public ActionResult SaveGame(FullBoard board)
        {
            puzzleSaver.SaveGame(board.BoardList, board.BoardNumber);
            board.Status = puzzleService.GetPuzzleStatus(board.BoardList);
            return View(Constants.CustomViewPath("GameView"), board);
        }

        /// <summary>
        /// Load Game
        /// </summary>
		/// <returns>ActionResult</returns>
        [AllowAnonymous]
        [HttpGet("LoadGame")]
        public ActionResult LoadGame()
        {
            FullBoard board = new() { BoardList = puzzleService.SetupBoard() };
            puzzleLoader.LoadSavedPuzzle(board.BoardList, out int puzzleNumber);
            board.Status = puzzleService.GetPuzzleStatus(board.BoardList);
            board.BoardNumber = puzzleNumber;
            return View(Constants.CustomViewPath("GameView"), board);
        }
    }
}
