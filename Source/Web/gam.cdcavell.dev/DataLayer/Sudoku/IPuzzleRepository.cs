using System;
using System.Xml.Linq;

namespace gam.cdcavell.dev.DataLayer.Sudoku
{
	/// <summary>
	/// Description of IPuzzleRepository.
	/// </summary>
	/// <revision>
	/// __Revisions:__~~
	/// | Contributor | Build | Revison Date | Description |~
	/// |-------------|-------|--------------|-------------|~
	/// | Christopher D. Cavell | 1.0.5.0 | 05/12/2023 | Game Development - Sudoku |~ 
	/// </revision>
	public interface IPuzzleRepository
	{
		/// <summary>
		/// Load puzzle setup
		/// </summary>
		XDocument LoadPuzzleSetupXDoc();

		/// <summary>
		/// Load saved game
		/// </summary>
		XDocument LoadSavedGameXDoc();

		/// <summary>
		/// Load puzzle setup XDoc
		/// </summary>
		void SavePuzzleSetupXDoc(XDocument xDoc);

		/// <summary>
		/// Save saved game XDoc
		/// </summary>
		void SaveSavedGameXDoc(XDocument xDoc);
	}
}
