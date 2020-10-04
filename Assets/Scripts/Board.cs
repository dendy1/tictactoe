using System.Collections;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("Cell Prefab & Settings")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private float cellsGap = 0.2f;
    [SerializeField] private GameObject playerPrefab;
    
    [Header("Settings Canvas Controller")]
    [SerializeField] private SettingsController settingsController; 

    [Header("Game Settings")]
    [SerializeField] private int boardSize = 3;

    public PlayerController FirstPlayer { get; set; }
    public PlayerController SecondPlayer { get; set; }
    private PlayerController CurrentPlayer { get; set; }
    
    private GameState GameState { get; set; } = GameState.Stop;
    
    public Cell[,] Cells { get; private set; }
    public int BoardSize => boardSize;

    private bool _blockCells;
    private bool _creatingCells;
    private GameObject _cellsContainer;
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        Cells = new Cell[boardSize, boardSize];
    }

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.startWidth = _lineRenderer.endWidth = 0.04f;
        _lineRenderer.enabled = false;
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
            
            CurrentPlayer.Sit();
            if (CurrentPlayer == FirstPlayer)
                SecondPlayer.Death();
            else
                FirstPlayer.Death();
            
            return;
        }

        if (GameLogic.IsDraw(Cells, boardSize))
        { 
            Draw();   
            FirstPlayer.Sit();
            SecondPlayer.Sit();
            return;
        }
        
        CurrentPlayer.Attack();
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
        settingsController.SetActive(true);
        GameState = GameState.Win;
    }

    private void Draw()
    {
        Debug.Log($"Draw!");
        settingsController.SetHeaderText($"Draw!");
        settingsController.SetActive(true);
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
    
    public void CreateCells()
    {
        if (_cellsContainer)
            return;
        
        _creatingCells = true;
        StartCoroutine(CreateCellsEnumerator());
    }
    
    private IEnumerator CreateCellsEnumerator()
    {
        _cellsContainer = new GameObject("Cells Container");
        _cellsContainer.transform.parent = transform;
        _cellsContainer.transform.localScale = Vector3.one;
        
        var startPos = (boardSize + (boardSize - 1) * cellsGap - 1) * -0.5f;
        _cellsContainer.transform.localPosition = new Vector3(startPos, 0.01f, startPos);
        
        for (var i = 0; i < boardSize; i++)
        {
            for (var j = 0; j < boardSize; j++)
            {
                var cellPos = new Vector3(j * (cellsGap + 1), 0f, i * (cellsGap + 1));
                var cell = Instantiate(cellPrefab).GetComponent<Cell>();
                
                var cellTransform = cell.gameObject.transform;
                cellTransform.parent = _cellsContainer.transform;
                cellTransform.localPosition = cellPos;
                cellTransform.localScale = Vector3.one;
                
                cell.Initialize(i, j, OnCellClicked);
                cell.SetActive(true);
                
                Cells[i, j] = cell;
                
                yield return new WaitForSeconds(0.35f / boardSize);
            }
        }

        _creatingCells = false;
    }
    
    public void InitializePlayers(bool firstPlayerHuman, bool secondPlayerHuman)
    {
        if (!FirstPlayer)
            CreateFirstPlayer();
        if (!SecondPlayer)
            CreateSecondPlayer();
        
        if (firstPlayerHuman)
        {
            FirstPlayer.Initialize("Player 1", PlayerType.Human, CellState.Cross);
        }
        else
        {
            FirstPlayer.Initialize("Computer 1", PlayerType.Computer, CellState.Cross);
        }
        
        if (secondPlayerHuman)
        {
            SecondPlayer.Initialize("Player 2", PlayerType.Human, CellState.Nought);
        }
        else
        {
            SecondPlayer.Initialize("Computer 2", PlayerType.Computer, CellState.Nought);
        }
        
        CurrentPlayer = FirstPlayer;
        
        GameState = GameState.Stop;
        
        Debug.Log($"FirstPlayer : {FirstPlayer}");
        Debug.Log($"SecondPlayer : {SecondPlayer}");
    }
    
    public void StartNewGame()
    {
        _lineRenderer.enabled = false;
        StartCoroutine(StartNewGameCoroutine());
    }

    private IEnumerator StartNewGameCoroutine()
    {
        while (_creatingCells)
        {
            yield return new WaitForEndOfFrame();
        }
        
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

    private void CreateFirstPlayer()
    {
        var xStart = (boardSize + (boardSize - 1) * cellsGap - 1) * -0.5f;
        
        FirstPlayer = Instantiate(playerPrefab).GetComponent<PlayerController>();
        var firstPlayerTransform = FirstPlayer.transform;
        firstPlayerTransform.parent = transform;
        firstPlayerTransform.localPosition = new Vector3(xStart - 2.5f, -transform.localPosition.y, 0f);
        firstPlayerTransform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        firstPlayerTransform.localScale = Vector3.one;
    }
    
    private void CreateSecondPlayer()
    {
        var xStart = (boardSize + (boardSize - 1) * cellsGap - 1) * -0.5f;
        
        SecondPlayer = Instantiate(playerPrefab).GetComponent<PlayerController>();
        var secondPlayerTransform = SecondPlayer.transform;
        secondPlayerTransform.parent = transform;
        secondPlayerTransform.localPosition = new Vector3(-xStart + 2.5f, -transform.localPosition.y, 0f);
        secondPlayerTransform.localRotation = Quaternion.Euler(0f, -90f, 0f);
        secondPlayerTransform.localScale = Vector3.one;
    }

    #region Computer Moves
    
    private void MakeComputerMove()
    {
        StartCoroutine(ComputerMove());
    }

    private IEnumerator ComputerMove()
    {
        yield return new WaitForSeconds(0.5f);

        var selectedMove = AI.SelectNextMove(Cells, BoardSize, CurrentPlayer);
        OnCellClicked(selectedMove);
        
        _blockCells = false;
    }

    #endregion

    #region Game Logic

    private bool CurrentPlayerWins(Cell lastCell)
    {
        if (GameLogic.CheckVertical(this, lastCell))
        {
            DrawLine(Cells[0, lastCell.Col].transform.position, Cells[BoardSize - 1, lastCell.Col].transform.position, lastCell);
            return true;
        }

        if (GameLogic.CheckHorizontal(this, lastCell))
        {
            DrawLine(Cells[lastCell.Row, 0].transform.position, Cells[lastCell.Row, BoardSize - 1].transform.position, lastCell);
            return true;
        }

        if (GameLogic.CheckMainDiagonal(this, lastCell))
        {
            DrawLine(Cells[0, BoardSize - 1].transform.position, Cells[BoardSize - 1, 0].transform.position, lastCell);
            return true;
        }

        if (GameLogic.CheckSideDiagonal(this, lastCell))
        {
            DrawLine(Cells[0, 0].transform.position, Cells[BoardSize - 1, BoardSize - 1].transform.position, lastCell);
            return true;
        }

        return false;
    } 

    private void DrawLine(Vector3 origin, Vector3 destination, Cell lastCell)
    {
        _lineRenderer.enabled = true;
        _lineRenderer.startColor = _lineRenderer.endColor = lastCell.CellState == CellState.Cross ? Color.green : Color.red;
        _lineRenderer.SetPosition(0, origin);
        _lineRenderer.SetPosition(1, destination);
    }

    #endregion
}
