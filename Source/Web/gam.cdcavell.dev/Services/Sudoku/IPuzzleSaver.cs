using gam.cdcavell.dev.Models.Sudoku;
using System;
using System.Collections.Generic;

namespace gam.cdcavell.dev.Services.Sudoku
{
    /// <summary>
    /// Description of IPuzzleSaver.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.5.0 | 05/12/2023 | Game Development - Sudoku |~ 
    /// </revision>
    public interface IPuzzleSaver
	{
        /// <summary>
        /// Save game
        /// </summary>
        /// <param name="cellList">List&lt;Cell&gt;</param>
        /// <param name="puzzleNumber">int</param>
        /// <method>SaveGame(List&lt;Cell&gt; cellList, int puzzleNumber)</method>
		void SaveGame(List<Cell> cellList, int puzzleNumber);

        /// <summary>
        /// Load puzzle setup
        /// </summary>
        /// <param name="cellList">List&lt;Cell&gt;</param>
        /// <param name="puzzleNumber">int</param>
        /// <method>SavePuzzleSetup(List&lt;Cell&gt; cellList, int puzzleNumber)</method>
        void SavePuzzleSetup(List<Cell> cellList, ref int puzzleNumber);
	}
}
