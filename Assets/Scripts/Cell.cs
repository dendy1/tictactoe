using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private XOController crossObject;
    [SerializeField] private XOController noughtObject;

    private CellState _currentCellState = CellState.Empty;
    private Action<Cell> _onCellClicked;
    private Animator _animator;
    private static readonly int Enabled = Animator.StringToHash("enabled");

    private Camera _camera;
    
    public int Row { get; private set; }
    public int Col { get; private set; }
    public CellState CellState
    {
        get => _currentCellState;
        set
        {
            _currentCellState = value;
            
            switch (value)
            {
                case CellState.Cross:
                    crossObject.SetActive(true);
                    break;
                case CellState.Nought:
                    noughtObject.SetActive(true);
                    break;
                case CellState.Empty:
                    if (noughtObject.IsEnabled)
                    {
                        noughtObject.SetActive(false);
                    }
                    else if (crossObject.IsEnabled)
                    {
                        crossObject.SetActive(false);
                    }
                    break;
            }
        }
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _camera = Camera.main;
    }

    private void OnMouseDown()
    {
        _onCellClicked?.Invoke(this);
    }
    
    public void Initialize(int row, int col, Action<Cell> onCellClicked)
    {
        Row = row;
        Col = col;
        
        name = $"Cell[{row}, {col}]";

        _onCellClicked = onCellClicked;
    }

    public void SetActive(bool value)
    {
        _animator.SetBool(Enabled, value);
    }
}
