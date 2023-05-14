using System;

namespace gam.cdcavell.dev.Services.Sudoku
{
    /// <summary>
    /// Description of Constants.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.5.0 | 05/12/2023 | Game Development - Sudoku |~ 
    /// </revision>
    internal static class Constants
	{
		internal const int BlockSize = 3;
		internal const int BoardSize = 9;
		internal const string CellErrorMessage = "Choose a number 1 through 9";

		internal static string CustomViewPath(string name, string extension = "cshtml")
		{
			return "~/Views/Sudoku/" + name.Trim() + "." + extension.Trim();
		}

		internal static string CustomFormActionPath(string controller, string action)
		{
			return "/Sudoku/" + controller.Trim() + "/" + action.Trim();
		}
	}
}
