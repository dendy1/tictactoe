using System.Collections.Generic;

public static class GameLogic
{
    public static bool CheckWin(Cell[,] cells, int boardSize, Cell lastCell)
    {
        return CheckHorizontal(cells, boardSize, lastCell) || 
               CheckVertical(cells, boardSize, lastCell) || 
               CheckMainDiagonal(cells, boardSize, lastCell) || 
               CheckSideDiagonal(cells, boardSize, lastCell);
    } 
    
    public static bool CheckWin(Board board, Cell lastCell)
    {
        return CheckHorizontal(board, lastCell) || 
               CheckVertical(board, lastCell) || 
               CheckMainDiagonal(board, lastCell) || 
               CheckSideDiagonal(board, lastCell);
    } 
    
    public static bool CheckVertical(Cell[,] cells, int boardSize, Cell lastCell)
    {
        var cellState = lastCell.CellState;
        
        for (var i = 0; i < boardSize; i++)
        {
            if (cells[i, lastCell.Col].CellState != cellState)
                return false;
        }

        return true;
    }

    public static bool CheckVertical(Board board, Cell lastCell)
    {
        return CheckVertical(board.Cells, board.BoardSize, lastCell);
    }

    public static bool CheckHorizontal(Cell[,] cells, int boardSize, Cell lastCell)
    {
        var cellState = lastCell.CellState;
        
        for (var i = 0; i < boardSize; i++)
        {
            if (cells[lastCell.Row, i].CellState != cellState)
                return false;
        }
        
        return true;
    }   
    
    public static bool CheckHorizontal(Board board, Cell lastCell)
    {
        return CheckHorizontal(board.Cells, board.BoardSize, lastCell);
    }
    
    public static bool CheckMainDiagonal(Cell[,] cells, int boardSize, Cell lastCell)
    {
        var cellState = lastCell.CellState;
        
        for (var i = 0; i < boardSize; i++)
        {
            if (cells[i, boardSize - 1 - i].CellState != cellState)
                return false;
        }
        
        return true;
    }
    
    public static bool CheckMainDiagonal(Board board, Cell lastCell)
    {
        return CheckMainDiagonal(board.Cells, board.BoardSize, lastCell);
    }
    
    public static bool CheckSideDiagonal(Cell[,] cells, int boardSize, Cell lastCell)
    {
        var cellState = lastCell.CellState;
        
        for (var i = 0; i < boardSize; i++)
        {
            if (cells[i, i].CellState != cellState)
                return false;
        }

        return true;
    }

    public static bool CheckSideDiagonal(Board board, Cell lastCell)
    {
        return CheckSideDiagonal(board.Cells, board.BoardSize, lastCell);
    }

    public static bool IsDraw(Cell[,] cells, int boardSize)
    {
        for (var i = 0; i < boardSize; i++)
        for (var j = 0; j < boardSize; j++)
            if (cells[i, j].CellState == CellState.Empty)
                return false;

        return true;
    }
    
    public static bool IsDraw(Board board)
    {
        return IsDraw(board.Cells, board.BoardSize);
    }
    
    public static List<Cell> GetEmptyCells(Cell[,] cells, int boardSize)
    {
        var emptyCells = new List<Cell>();

        for (var i = 0; i < boardSize; i++)
        {
            for (var j = 0; j < boardSize; j++)
            {
                if (cells[i, j].CellState == CellState.Empty)
                {
                    emptyCells.Add(cells[i, j]);
                }
            }
        }
        

        return emptyCells;
    }
}
