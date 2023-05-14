using System;
using System.IO;
using System.Xml.Linq;

namespace gam.cdcavell.dev.DataLayer.Sudoku
{
	/// <summary>
	/// Description of XmlFilePuzzleRepository.
	/// </summary>
	/// <revision>
	/// __Revisions:__~~
	/// | Contributor | Build | Revison Date | Description |~
	/// |-------------|-------|--------------|-------------|~
	/// | Christopher D. Cavell | 1.0.5.0 | 05/12/2023 | Game Development - Sudoku |~ 
	/// </revision>
	public class XmlFilePuzzleRepository : IPuzzleRepository
	{
		private const string puzzleSetupXmlString = "PuzzleSetup.xml";
		private const string savedGameXmlString = "SavedGame.xml";
		
		readonly private string puzzleSetupXmlPath = string.Empty;
		readonly private string savedGameXmlPath = string.Empty;

		/// <summary>
		/// Constructor method
		/// </summary>
		public XmlFilePuzzleRepository()
		{
			puzzleSetupXmlPath = Path.Combine(AppDomain.CurrentDomain.GetData("APPBASE")?.ToString() ?? string.Empty, puzzleSetupXmlString);
			savedGameXmlPath = Path.Combine(AppDomain.CurrentDomain.GetData("APPBASE")?.ToString() ?? string.Empty, savedGameXmlString);
		}

		/// <summary>
		/// load puzzle setup
		/// </summary>
		public XDocument LoadPuzzleSetupXDoc()
		{
			return XDocument.Load(puzzleSetupXmlPath);
		}

		/// <summary>
		/// Load saved game
		/// </summary>
		public XDocument LoadSavedGameXDoc()
		{
			return XDocument.Load(savedGameXmlPath);
		}

		/// <summary>
		/// Save puzzle setup
		/// </summary>
		/// <param name="xDoc">XDocument</param>
		public void SavePuzzleSetupXDoc(XDocument xDoc)
		{
			xDoc.Save(puzzleSetupXmlPath);
		}

		/// <summary>
		/// Save saved game
		/// </summary>
		/// <param name="xDoc">XDocument</param>
		public void SaveSavedGameXDoc(XDocument xDoc)
		{
			xDoc.Save(savedGameXmlPath);
		}
	}
}
