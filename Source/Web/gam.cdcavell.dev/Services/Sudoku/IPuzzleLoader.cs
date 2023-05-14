using gam.cdcavell.dev.Models.Sudoku;
using System;
using System.Collections.Generic;

namespace gam.cdcavell.dev.Services.Sudoku
{
    /// <summary>
    /// Description of IPuzzleLoader.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.5.0 | 05/12/2023 | Game Development - Sudoku |~ 
    /// </revision>
    public interface IPuzzleLoader
	{
        /// <summary>
        /// Load new puzzle
        /// </summary>
        /// <param name="cellList">List&lt;Cell&gt;</param>
        /// <param name="puzzleNumber">int</param>
        /// <method>LoadPuzzle(List&lt;Cell&gt; cellList, int puzzleNumber)</method>
		void LoadNewPuzzle(List<Cell> cellList, out int puzzleNumber);

        /// <summary>
        /// Reload puzzle
        /// </summary>
        /// <param name="cellList">List&lt;Cell&gt;</param>
        /// <param name="puzzleNumber">int</param>
        /// <method>ReloadPuzzle(List&lt;Cell&gt; cellList, int puzzleNumber)</method>
        void ReloadPuzzle(List<Cell> cellList, int puzzleNumber);

        /// <summary>
        /// Load saved puzzle
        /// </summary>
        /// <param name="cellList">List&lt;Cell&gt;</param>
        /// <param name="puzzleNumber">int</param>
        /// <method>LoadSavedPuzzle(List&lt;Cell&gt; cellList, int puzzleNumber)</method>
		void LoadSavedPuzzle(List<Cell> cellList, out int puzzleNumber);
	}
}
