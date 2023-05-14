using gam.cdcavell.dev.DataLayer.Sudoku;
using gam.cdcavell.dev.Models.Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace gam.cdcavell.dev.Services.Sudoku
{
	/// <summary>
	/// This class saves a Sudoku puzzle as an Xml Document.
	/// </summary>
	/// <revision>
	/// __Revisions:__~~
	/// | Contributor | Build | Revison Date | Description |~
	/// |-------------|-------|--------------|-------------|~
	/// | Christopher D. Cavell | 1.0.5.0 | 05/12/2023 | Game Development - Sudoku |~ 
	/// </revision>
	public class XDocPuzzleSaver : IPuzzleSaver
	{
		private readonly IPuzzleRepository? puzzleRepository = null;

		/// <summary>
		/// Constructor method
		/// </summary>
		/// <param name="xDocPuzzleRepository">IPuzzleRepository</param>
		public XDocPuzzleSaver(IPuzzleRepository xDocPuzzleRepository)
		{
			puzzleRepository = xDocPuzzleRepository;
		}

		/// <summary>
		/// Save game
		/// </summary>
		/// <param name="cellList">List&lt;Cell&gt;</param>
		/// <param name="puzzleNumber">int</param>
		/// <method>SaveGame(List&lt;Cell&gt; cellList, int puzzleNumber)</method>
		public void SaveGame(List<Cell> cellList, int puzzleNumber)
		{
			if (puzzleRepository != null)
			{
				XDocument savedGameXDoc = InitializeSavedGameXDoc();

				SavePuzzleInXDoc(cellList, savedGameXDoc, puzzleNumber, puzzleRepository.SaveSavedGameXDoc);
			}
		}

		/// <summary>
		/// Save game
		/// </summary>
		/// <param name="cellList">List&lt;Cell&gt;</param>
		/// <param name="puzzleNumber">int</param>
		/// <method>SaveGame(List&lt;Cell&gt; cellList, ref int puzzleNumber)</method>
		public void SavePuzzleSetup(List<Cell> cellList, ref int puzzleNumber)
		{
			if (puzzleRepository != null)
			{
				XDocument? puzzleSetupXDoc = puzzleRepository.LoadPuzzleSetupXDoc();

				bool puzzleExists = (puzzleNumber > 0);
				if (puzzleExists)
				{
					OverwritePuzzleInXDoc(cellList, puzzleSetupXDoc, puzzleNumber);
				}
				else
				{
					puzzleNumber = FindNextAvailablePuzzleNumber(puzzleSetupXDoc);
					SavePuzzleInXDoc(cellList, puzzleSetupXDoc, puzzleNumber, puzzleRepository.SavePuzzleSetupXDoc);
				}
			}
		}
		
		private static XDocument InitializeSavedGameXDoc()
		{
			XDocument savedGameXDoc = new()
			{
				Declaration = new XDeclaration("1.0", "utf-8", "true")
			};
			savedGameXDoc.Add(new XElement("PuzzleSetup"));
			
			return savedGameXDoc;
		}
		
		private void OverwritePuzzleInXDoc(List<Cell> cellList, XDocument? puzzleSetupXDoc, int puzzleNumber)
		{
			if (puzzleSetupXDoc != null)
			{
				XElement? cellsXElement = puzzleSetupXDoc.Descendants("Puzzle").First(b => ((int?)b.Element("Number") ?? 0) == puzzleNumber).Element("Cells");
				cellsXElement?.ReplaceWith(CreateCellsXElement(cellList));

				puzzleRepository?.SavePuzzleSetupXDoc(puzzleSetupXDoc);
			}
		}
		
		private static int FindNextAvailablePuzzleNumber(XDocument? existingPuzzleXDoc)
		{
			return (existingPuzzleXDoc?.Descendants("Puzzle").Max(b => ((int?)b.Element("Number") ?? 0)) + 1) ?? 0;
		}
		
		private static void SavePuzzleInXDoc(List<Cell> cellList, XDocument? saveXDoc, int puzzleNumber,
		                               Action<XDocument> saveMethod)
		{
			if (saveXDoc != null)
			{
				saveXDoc.Element("PuzzleSetup")?.Add(
					new XElement("Puzzle",
						new XElement("Number", puzzleNumber),
						new XElement("Difficulty"),
						CreateCellsXElement(cellList)));

				saveMethod(saveXDoc);
			}
		}
		
		private static XElement CreateCellsXElement(List<Cell> cellList)
		{
			return new XElement("Cells", 
				cellList.Select((c, i) => c.Value.HasValue ?
										  new XElement("Cell", new XAttribute("index", i), new XAttribute("value", c.Value.Value)) :
			                              null));
		}
	}
}
