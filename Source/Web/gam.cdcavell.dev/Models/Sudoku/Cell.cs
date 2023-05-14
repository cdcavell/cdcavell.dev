using gam.cdcavell.dev.Services.Sudoku;
using System.ComponentModel.DataAnnotations;

namespace gam.cdcavell.dev.Models.Sudoku
{
    /// <summary>
    ///  Cell class domain model
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.5.0 | 05/14/2023 | Game Development - Sudoku |~ 
    /// </revision>
    public class Cell
    {
        /// <value>int</value>
        public int XCoordinate { get; set; }
        /// <value>int</value>
        public int YCoordinate { get; set; }
        /// <value>int</value>
        public int BlockNumber { get; set; }
        /// <value>int?</value>
        [Range(1, Constants.BoardSize, ErrorMessage = Constants.CellErrorMessage)]
        public int? Value { get; set; }
		/// <value>int</value>
		public int CellNumber { get; set; }
        /// <value>bool</value>
        public bool CellLocked { get; set; }

        /// <summary>
        /// Constructor method
        /// </summary>
        public Cell()
        {
            Value = null;
        }
    }
}
