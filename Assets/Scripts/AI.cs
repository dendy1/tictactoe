using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    private class AICell
    {
        public int Row { get; private set; }
        public int Col { get; private set; }
        public CellState CellState { get; set; }

        public AICell(int row, int col, CellState currentCellState)
        {
            Row = row;
            Col = col;
            CellState = currentCellState;
        }

        public AICell(Cell cell) : this(cell.Row, cell.Col, cell.CellState) { }
    }
    
    private static bool CheckWin(AICell[,] cells, int boardSize, AICell lastCell)
    {
        return CheckHorizontal(cells, boardSize, lastCell) || 
               CheckVertical(cells, boardSize, lastCell) || 
               CheckMainDiagonal(cells, boardSize, lastCell) || 
               CheckSideDiagonal(cells, boardSize, lastCell);
    } 
    
    private static bool CheckVertical(AICell[,] cells, int boardSize, AICell lastCell)
    {
        var cellState = lastCell.CellState;
        
        for (var i = 0; i < boardSize; i++)
        {
            if (cells[i, lastCell.Col].CellState != cellState)
                return false;
        }

        return true;
    }

    private static bool CheckHorizontal(AICell[,] cells, int boardSize, AICell lastCell)
    {
        var cellState = lastCell.CellState;
        
        for (var i = 0; i < boardSize; i++)
        {
            if (cells[lastCell.Row, i].CellState != cellState)
                return false;
        }
        
        return true;
    }   
    
    private static bool CheckMainDiagonal(AICell[,] cells, int boardSize, AICell lastCell)
    {
        var cellState = lastCell.CellState;
        
        for (var i = 0; i < boardSize; i++)
        {
            if (cells[i, boardSize - 1 - i].CellState != cellState)
                return false;
        }
        
        return true;
    }
    
    private static bool CheckSideDiagonal(AICell[,] cells, int boardSize, AICell lastCell)
    {
        var cellState = lastCell.CellState;
        
        for (var i = 0; i < boardSize; i++)
        {
            if (cells[i, i].CellState != cellState)
                return false;
        }

        return true;
    }
    
    private static List<AICell> GetEmptyCells(AICell[,] cells, int boardSize)
    {
        var emptyCells = new List<AICell>();
            
        for (var i = 0; i < boardSize; i++)
        for (var j = 0; j < boardSize; j++)
            if (cells[i, j].CellState == CellState.Empty)
                emptyCells.Add(cells[i, j]);

        return emptyCells;
    }
    
    public static Cell SelectNextMove(Cell[,] cells, int boardSize, PlayerController currentPlayer)
    {
        var firstCandidates = GameLogic.GetEmptyCells(cells, boardSize);
        foreach (var firstCell in firstCandidates)
        {
            var row = firstCell.Row;
            var col = firstCell.Col;
            
            var boardCopy = new AICell[boardSize, boardSize];
            for (var i = 0; i < boardSize; i++)
            {
                for (var j = 0; j < boardSize; j++)
                {
                    boardCopy[i, j] = new AICell(cells[i,j]);
                }
            }

            boardCopy[row, col].CellState = currentPlayer.CellState;
            var lastCell = boardCopy[row, col];
            if (CheckWin(boardCopy, boardSize, lastCell))
            {
                return cells[lastCell.Row, lastCell.Col];
            }
            
            var secondCandidates = GetEmptyCells(boardCopy, boardSize);
            foreach (var secondCell in secondCandidates)
            {
                row = secondCell.Row;
                col = secondCell.Col;
            
                var boardCopyCopy = new AICell[boardSize, boardSize];
                for (var i = 0; i < boardSize; i++)
                {
                    for (var j = 0; j < boardSize; j++)
                    {
                        boardCopyCopy[i, j] = boardCopy[i, j];
                    }
                }

                boardCopyCopy[row, col].CellState = currentPlayer.OtherCellState;
                lastCell = boardCopyCopy[row, col];
                if (CheckWin(boardCopyCopy, boardSize, lastCell))
                {
                    return cells[lastCell.Row, lastCell.Col];
                }
            }
        }

        return firstCandidates[Random.Range(0, firstCandidates.Count - 1)];
    }
}
