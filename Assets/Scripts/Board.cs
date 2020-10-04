using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;

    public const int Rows = 3;
    public const int Cols = 3;

    private Player FirstPlayer { get; set; }
    private Player SecondPlayer { get; set; }
    private Player CurrentPlayer { get; set; }
    
    public Cell[,] Cells { get; private set; }
    
    private void Awake()
    {
        Cells = new Cell[Rows, Cols];
        CreateCells();
        Initialize(true, false);
    }
    
    public void Initialize(bool firstTurn, bool secondPlayerComputer)
    {
        if (secondPlayerComputer)
        {
            if (firstTurn)
            {
                FirstPlayer = new Player("Player", PlayerType.Human, State.Cross);
                SecondPlayer = new Player("Computer", PlayerType.Computer, State.Nought);
            }
            else
            {
                FirstPlayer = new Player("Computer", PlayerType.Computer, State.Cross);
                SecondPlayer = new Player("Player", PlayerType.Human, State.Nought);
            }
        }
        else
        {
            FirstPlayer = new Player("Player 1", PlayerType.Human, State.Cross);
            SecondPlayer = new Player("Player 2", PlayerType.Human, State.Nought);
        }
        
        Debug.Log($"FirstPlayer : {FirstPlayer}");
        Debug.Log($"SecondPlayer : {SecondPlayer}");
    }

    private void CreateCells()
    {
        for (var i = 0; i < Rows; i++)
        {
            for (var j = 0; j < Cols; j++)
            {
                var gap = 0.2f;
                var startX = - Cols * (1 + gap) * 0.5f;
                var startZ = - Rows * (1 + gap) * 0.5f;
                
                var cellPos = new Vector3(j + gap * (j - 1), 0f, i + gap * (i - 1));
                
                var cell = Instantiate(cellPrefab, cellPos, Quaternion.identity).GetComponent<Cell>();
                cell.gameObject.transform.parent = transform;
                cell.Initialize(i, j, OnCellClicked);

                Cells[i, j] = cell;
            }
        }
    }

    private void OnCellClicked(Cell cell)
    {
        if (cell.CellState != State.Empty) 
            return;
        
        cell.CellState = CurrentPlayer.CellState;
        
        // Checking for current player win
        if (GameLogic.CheckForWin(this, cell))
        {
            Debug.Log($"{CurrentPlayer} won!");
            return;
        }

        // Checking for draw
        if (GameLogic.CheckForDraw(this))
        {
            Debug.Log($"Draw!");
            return;
        }
        
        ChangePlayer();
        if (CurrentPlayer.PlayerType == PlayerType.Computer)
        {
            MakeComputerMove();
        }
    }
    
    private void MakeComputerMove()
    {
        // Computer Move
        ChangePlayer();
    }

    private void ChangePlayer()
    {
        CurrentPlayer = CurrentPlayer == FirstPlayer ? SecondPlayer : FirstPlayer;
    }
}
