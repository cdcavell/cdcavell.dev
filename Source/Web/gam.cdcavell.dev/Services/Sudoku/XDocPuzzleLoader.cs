using gam.cdcavell.dev.DataLayer.Sudoku;
using gam.cdcavell.dev.Models.Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace gam.cdcavell.dev.Services.Sudoku
{
	/// <summary>
	/// This class loads SuDoku puzzles that have been saved as Xml Documents.
	/// </summary>
	/// <revision>
	/// __Revisions:__~~
	/// | Contributor | Build | Revison Date | Description |~
	/// |-------------|-------|--------------|-------------|~
	/// | Christopher D. Cavell | 1.0.5.0 | 05/12/2023 | Game Development - Sudoku |~ 
	/// </revision>
	public class XDocPuzzleLoader : IPuzzleLoader
	{
		private readonly Random random = new();
		private readonly IPuzzleRepository? xDocPuzzleRepository = null;

		/// <summary>
		/// Constructor method
		/// </summary>
		/// <param name="puzzleRepository">IPuzzleRepository</param>
		public XDocPuzzleLoader(IPuzzleRepository puzzleRepository)
		{
			xDocPuzzleRepository = puzzleRepository;
		}

		/// <summary>
		/// Load new puzzle
		/// </summary>
		/// <param name="cellList">List&lt;Cell&gt;</param>
		/// <param name="puzzleNumber">int</param>
		/// <method>LoadNewPuzzle(List&lt;Cell&gt; cellList, out int puzzleNumber)</method>
		public void LoadNewPuzzle(List<Cell> cellList, out int puzzleNumber)
		{
			puzzleNumber = GetRandomPuzzleNumber();
			LoadPuzzleFromSetupXDoc(puzzleNumber, cellList);
		}

		/// <summary>
		/// Reload puzzle
		/// </summary>
		/// <param name="cellList">List&lt;Cell&gt;</param>
		/// <param name="puzzleNumber">int</param>
		/// <method>ReloadPuzzle(List&lt;Cell&gt; cellList, int puzzleNumber)</method>
		public void ReloadPuzzle(List<Cell> cellList, int puzzleNumber)
		{
			LoadPuzzleFromSetupXDoc(puzzleNumber, cellList);
		}

		/// <summary>
		/// Load saved puzzle
		/// </summary>
		/// <param name="cellList">List&lt;Cell&gt;</param>
		/// <param name="puzzleNumber">int</param>
		/// <method>LoadSavedPuzzle(List&lt;Cell&gt; cellList, out int puzzleNumber)</method>
		public void LoadSavedPuzzle(List<Cell> cellList, out int puzzleNumber)
		{
			XDocument? savedGameDoc = xDocPuzzleRepository?.LoadSavedGameXDoc();
			XElement? x = savedGameDoc?.Descendants("Puzzle").FirstOrDefault();
			puzzleNumber = (int?)x?.Element("Number") ?? 0;
			LoadCellListFromPuzzleXElement(x, cellList);
		}
		
		private int GetRandomPuzzleNumber()
		{
			XDocument? puzzleSetupDoc = xDocPuzzleRepository?.LoadPuzzleSetupXDoc();
			
			var puzzleNumberList = puzzleSetupDoc?.Descendants("Puzzle").Select(b => ((int?)b.Element("Number") ?? 0)).ToList();
			int puzzleConfigCount = (puzzleNumberList?.Count ?? 0);
			int randomPuzzleIndex = random.Next(puzzleConfigCount);

			if (puzzleNumberList != null)
				return puzzleNumberList[randomPuzzleIndex];

			return 0;
		}
		
		private void LoadPuzzleFromSetupXDoc(int puzzleNumber, List<Cell> cellList)
		{
			XDocument? puzzleSetupXDoc = xDocPuzzleRepository?.LoadPuzzleSetupXDoc();
			
			XElement? x = puzzleSetupXDoc?.Descendants("Puzzle").First(b => ((int?)b.Element("Number") ?? 0) == puzzleNumber);
			LoadCellListFromPuzzleXElement(x, cellList);
		}
		
		private static void LoadCellListFromPuzzleXElement(XElement? puzzleXElement, List<Cell> cellList)
		{
			var y = puzzleXElement?.Descendants("Cells").Descendants("Cell").ToList();
			y?.ForEach(c => cellList[(int?)c.Attribute("index") ?? 0].Value = (int?)c.Attribute("value"));
		}
	}
}
