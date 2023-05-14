using gam.cdcavell.dev.Models.Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gam.cdcavell.dev.Services.Sudoku
{
    /// <summary>
    /// Puzzle status enumerations
    /// </summary>
    public enum PuzzleStatus
	{
		/// <summary>Normal status</summary>
		Normal,
        /// <summary>Invalid status</summary>
        Invalid,
        /// <summary>Complete status</summary>
        Complete
    }

    /// <summary>
    /// This class provides all the methods needed to play a game of Sudoku.
    /// It initializes a Sudoku board, and determines the status of a puzzle.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.5.0 | 05/13/2023 | Game Development - Sudoku |~ 
    /// </revision>
    public class PuzzleService : IPuzzleService
	{
        /// <summary>
        /// Setup board
        /// </summary>
		/// <returns>List&lt;Cell&gt;</returns>
		public List<Cell> SetupBoard()
		{
			List<Cell> board = new();
			int cellNumber = 0;

			for (int x = 0; x < Constants.BoardSize; x++)
			{
				for (int y = 0; y < Constants.BoardSize; y++)
				{
					Cell newCell = new() 
					{ 
						XCoordinate = x + 1, 
						YCoordinate = y + 1,
						BlockNumber = Constants.BlockSize * (x / Constants.BlockSize) + (y / Constants.BlockSize) + 1,
						CellNumber = cellNumber
					};
					board.Add(newCell);
					cellNumber++;
				}
			}
			
			return board;
		}

        /// <summary>
        /// Return puzzle status
        /// </summary>
        /// <param name="cellList">List&lt;Cell&gt;</param>
		/// <returns>PuzzleStatus</returns>
        /// <method>GetPuzzleStatus(List&lt;Cell&gt; cellList)</method>
        public PuzzleStatus GetPuzzleStatus(List<Cell> cellList)
		{
			PuzzleStatus status;
			
			if (!IsPuzzleValid(cellList))
			{
				status = PuzzleStatus.Invalid;
			}
			else if (IsPuzzleComplete(cellList))
			{
				status = PuzzleStatus.Complete;
			}
			else
			{
				status = PuzzleStatus.Normal;
			}
			
			return status;
		}
		
		private static bool IsPuzzleValid(List<Cell> cellList)
		{
			bool isValid = AreRowsValid(cellList);
			isValid &= AreColumnsValid(cellList);
			isValid &= AreBlocksValid(cellList);
			
			return isValid;
		}
		
		private static bool AreRowsValid(List<Cell> cellList)
		{
			bool isValid = true;
			
			cellList.GroupBy(c => c.XCoordinate).Select(g => g.ToList()).ToList().ForEach(s => isValid &= IsValueUniqueInSet(s));
			
			return isValid;
		}
		
		private static bool AreColumnsValid(List<Cell> cellList)
		{
			bool isValid = true;
			
			cellList.GroupBy(c => c.YCoordinate).Select(g => g.ToList()).ToList().ForEach(s => isValid &= IsValueUniqueInSet(s));
			
			return isValid;
		}
		
		private static bool AreBlocksValid(List<Cell> cellList)
		{
			bool isValid = true;
			
			cellList.GroupBy(c => c.BlockNumber).Select(g => g.ToList()).ToList().ForEach(s => isValid &= IsValueUniqueInSet(s));

			return isValid;
		}
		
		private static bool IsValueUniqueInSet(List<Cell> cellGroup)
		{
			// Validate that each non-NULL value in this group is unique.  Ignore NULL values.
			return cellGroup.Where(c => c.Value.HasValue).GroupBy(c => (c.Value ?? 0)).All(g => g.Count() <= 1);
		}

		// Must be called after IsBoardValid().  A board can be completely filled in, but invalid.
		private static bool IsPuzzleComplete(List<Cell> cellList)
		{
			return cellList.All(c => c.Value.HasValue);
		}
	}
}
