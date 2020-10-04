using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("Cell Prefab & Settings")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private float cellsGap = 0.2f;
    
    [Header("Settings Canvas Controller")]
    [SerializeField] private SettingsController settingsController; 

    [Header("Game Settings")]
    [SerializeField] private int boardSize = 3;

    private Player FirstPlayer { get; set; }
    private Player SecondPlayer { get; set; }
    private Player CurrentPlayer { get; set; }

    private GameState GameState { get; set; } = GameState.Stop;
    
    public Cell[,] Cells { get; private set; }

    private bool _blockCells;

    private void Awake()
    {
        Cells = new Cell[boardSize, boardSize];
        CreateCells();
    }
    
    public void CreateCells()
    {
        var cellsContainer = new GameObject("Cells Container");
        cellsContainer.transform.parent = transform;
        
        for (var i = 0; i < boardSize; i++)
        {
            for (var j = 0; j < boardSize; j++)
            {
                var cellPos = new Vector3(j * (cellsGap + 1), 0f, i * (cellsGap + 1));
                var cell = Instantiate(cellPrefab, cellPos, Quaternion.identity).GetComponent<Cell>();
                cell.gameObject.transform.parent = cellsContainer.transform;
                cell.Initialize(i, j, OnCellClicked);

                Cells[i, j] = cell;
            }
        }
        
        var startPos = (boardSize + (boardSize - 1) * cellsGap - 1) * -0.5f;
        cellsContainer.transform.localPosition = new Vector3(startPos, 0f, startPos);
        cellsContainer.transform.localScale = Vector3.one;
    }

    private void OnCellClicked(Cell cell)
    {
        if (GameState != GameState.Running || _blockCells || cell.CellState != CellState.Empty) 
            return;

        if (CurrentPlayer.PlayerType == PlayerType.Computer)
            _blockCells = true;
        
        PlaceMark(cell);
        
        if (CurrentPlayerWins(cell))
        {
            Win();
            return;
        }

        if (IsDraw())
        { 
            Draw(); 
            return;
        }
        
        ChangePlayer();
    }

    private void PlaceMark(Cell cell)
    {
        Debug.Log($"{CurrentPlayer.CellState} placed on {cell.name}");
        cell.CellState = CurrentPlayer.CellState;
    }

    private void Win()
    {
        Debug.Log($"{CurrentPlayer} won!");
        settingsController.SetHeaderText($"{CurrentPlayer} won!");
        settingsController.gameObject.SetActive(true);
        GameState = GameState.Win;
    }

    private void Draw()
    {
        Debug.Log($"Draw!");
        settingsController.SetHeaderText($"Draw!");
        settingsController.gameObject.SetActive(true);
        GameState = GameState.Draw;
    }

    private void ChangePlayer()
    {
        CurrentPlayer = CurrentPlayer == FirstPlayer ? SecondPlayer : FirstPlayer;
        
        if (CurrentPlayer.PlayerType == PlayerType.Computer)
        {
            MakeComputerMove();
        }
    }

    public void InitializePlayers(bool firstPlayerHuman, bool secondPlayerHuman)
    {
        CurrentPlayer = FirstPlayer = firstPlayerHuman ? new Player("Player 1", PlayerType.Human, CellState.Cross) : new Player("Computer 1", PlayerType.Computer, CellState.Cross);
        SecondPlayer = secondPlayerHuman ? new Player("Player 2", PlayerType.Human, CellState.Nought) : new Player("Computer 2", PlayerType.Computer, CellState.Nought);
        
        GameState = GameState.Stop;
        
        Debug.Log($"FirstPlayer : {FirstPlayer}");
        Debug.Log($"SecondPlayer : {SecondPlayer}");
    }
    
    public void StartNewGame()
    {
        for (var i = 0; i < boardSize; i++)
        {
            for (var j = 0; j < boardSize; j++)
            {
               Cells[i, j].CellState = CellState.Empty;
            }
        }
        
        GameState = GameState.Running;
        
        if (CurrentPlayer.PlayerType == PlayerType.Computer)
        {
            MakeComputerMove();
        }
    }

    #region AI
    
    private void MakeComputerMove()
    {
        StartCoroutine(ComputerMove());
    }

    private IEnumerator ComputerMove()
    {
        yield return new WaitForSeconds(0.2f);
        
        var emptyCells = EmptyCells;
        var cell = emptyCells[Random.Range(0, emptyCells.Count - 1)];
        
        OnCellClicked(cell);
        _blockCells = false;
    }

    private List<Cell> EmptyCells
    {
        get
        {
            var emptyCells = new List<Cell>();
            
            for (var i = 0; i < boardSize; i++)
            for (var j = 0; j < boardSize; j++)
                if (Cells[i, j].CellState == CellState.Empty)
                    emptyCells.Add(Cells[i, j]);

            return emptyCells;
        }
    }

    #endregion

    #region Game Logic

    private bool CurrentPlayerWins(Cell lastCell)
    {
        return CheckHorizontal(lastCell) || 
               CheckVertical(lastCell) || 
               CheckMainDiagonal(lastCell) || 
               CheckSideDiagonal(lastCell);
    } 

    private bool CheckVertical(Cell lastCell)
    {
        var cellState = lastCell.CellState;
        var size = Cells.GetLength(0);
        
        for (var i = 0; i < size; i++)
        {
            if (Cells[i, lastCell.Col].CellState != cellState)
                return false;
        }
        
        return true;
    }
    
    private bool CheckHorizontal(Cell lastCell)
    {
        var cellState = lastCell.CellState;
        var size = Cells.GetLength(0);
        
        for (var i = 0; i < size; i++)
        {
            if (Cells[lastCell.Row, i].CellState != cellState)
                return false;
        }
        
        return true;
    }
    
    private bool CheckMainDiagonal(Cell lastCell)
    {
        var cellState = lastCell.CellState;
        var size = Cells.GetLength(0);
        
        for (var i = 0; i < size; i++)
        {
            if (Cells[i, i].CellState != cellState)
                return false;
        }
        
        return true;
    }
    
    private bool CheckSideDiagonal(Cell lastCell)
    {
        var cellState = lastCell.CellState;
        var size = Cells.GetLength(0);
        
        for (int i = 0; i < size; i++)
        {
            if (Cells[i, size - 1 - i].CellState != cellState)
                return false;
        }
        
        return true;
    }
    
    private bool IsDraw()
    {
        for (var i = 0; i < boardSize; i++)
        for (var j = 0; j < boardSize; j++)
            if (Cells[i, j].CellState == CellState.Empty)
                return false;

        return true;
    }

    #endregion
}
