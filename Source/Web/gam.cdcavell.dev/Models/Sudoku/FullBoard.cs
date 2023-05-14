using gam.cdcavell.dev.Services.Sudoku;

namespace gam.cdcavell.dev.Models.Sudoku
{
    /// <summary>
    /// FullBoard view model
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.5.0 | 05/13/2023 | Game Development - Sudoku |~ 
    /// </revision>
    public class FullBoard
    {
        /// <value>List&lt;Cell&gt;</value>
        public List<Cell> BoardList { get; set; }
        /// <value>int</value>
        public int BoardNumber { get; set; }
        /// <value>PuzzleStatus</value>
        public PuzzleStatus Status { get; set; }
		/// <value>int</value>
		public int BoardSize { get; set; }
		/// <value>int</value>
		public int BlockSize { get; set; }
        /// <value>string?</value>
        public string? CellFocus { get; set; }

        /// <summary>
        /// Constructor method
        /// </summary>
        public FullBoard()
        {
            BoardList = new List<Cell>();
            Status = PuzzleStatus.Normal;
            BoardSize = Constants.BoardSize; 
            BlockSize = Constants.BlockSize;
		}
    }
}
