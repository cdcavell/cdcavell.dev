using gam.cdcavell.dev.Models.Sudoku;
using System;
using System.Collections.Generic;

namespace gam.cdcavell.dev.Services.Sudoku
{
    /// <summary>
    /// Description of IPuzzleService.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.5.0 | 05/12/2023 | Game Development - Sudoku |~ 
    /// </revision>
    public interface IPuzzleService
	{
        /// <summary>
        /// Set up board
        /// </summary>
        /// <returns>List&lt;Cell&gt;</returns>
		List<Cell> SetupBoard();

        /// <summary>
        /// Returns puzzle status
        /// </summary>
        /// <param name="cellList">List&lt;Cell&gt;</param>
        /// <returns>PuzzleStatus</returns>
        /// <method>GetPuzzleStatus(List&lt;Cell&gt; cellList)</method>
        PuzzleStatus GetPuzzleStatus(List<Cell> cellList);
	}
}
