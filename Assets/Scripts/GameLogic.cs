public class GameLogic
{
    public static bool CheckForWin(Board board, Cell lastCell)
    {
        return CheckHorizontal(board, lastCell) || 
               CheckVertical(board, lastCell) || 
               CheckMainDiagonal(board, lastCell) || 
               CheckSideDiagonal(board, lastCell);
    } 
    
    public static bool CheckForDraw(Board board)
    {
        return false;
    }

    private static bool CheckVertical(Board board, Cell lastCell)
    {
        var cellState = lastCell.CellState;
        var size = board.Cells.GetLength(0);
        
        for (var i = 0; i < size; i++)
        {
            if (board.Cells[i, lastCell.Col].CellState != cellState)
                return false;
        }
        
        return true;
    }
    
    private static bool CheckHorizontal(Board board, Cell lastCell)
    {
        var cellState = lastCell.CellState;
        var size = board.Cells.GetLength(0);
        
        for (var i = 0; i < size; i++)
        {
            if (board.Cells[lastCell.Row, i].CellState != cellState)
                return false;
        }
        
        return true;
    }
    
    private static bool CheckMainDiagonal(Board board, Cell lastCell)
    {
        var cellState = lastCell.CellState;
        var size = board.Cells.GetLength(0);
        
        for (var i = 0; i < size; i++)
        {
            if (board.Cells[i, i].CellState != cellState)
                return false;
        }
        
        return true;
    }
    
    private static bool CheckSideDiagonal(Board board, Cell lastCell)
    {
        var cellState = lastCell.CellState;
        var size = board.Cells.GetLength(0);
        
        for (int i = 0; i < size; i++)
        {
            if (board.Cells[i, size - 1 - i].CellState != cellState)
                return false;
        }
        
        return true;
    }
}
