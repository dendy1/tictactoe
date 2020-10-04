using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private GameObject crossObject;
    [SerializeField] private GameObject noughtObject;

    private CellState _currentCellState;
    private Action<Cell> _onCellClicked;

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
                    noughtObject.SetActive(false);
                    crossObject.SetActive(true);
                    break;
                case CellState.Nought:
                    noughtObject.SetActive(true);
                    crossObject.SetActive(false);
                    break;
                case CellState.Empty:
                    noughtObject.SetActive(false);
                    crossObject.SetActive(false);
                    break;
            }
        }
    }

    public void Initialize(int row, int col, Action<Cell> onCellClicked)
    {
        Row = row;
        Col = col;
        CellState = CellState.Empty;
        
        name = $"Cell[{row}, {col}]";

        _onCellClicked = onCellClicked;
    }
    
    
    private void Awake()
    {
        _camera = Camera.main;
    }
    
    private void OnMouseDown()
    {
        _onCellClicked?.Invoke(this);
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            var ray = Camera.allCameras[0].ScreenPointToRay(Input.GetTouch(0).position);
            
            if (Physics.Raycast(ray, out var hit))
            {
                hit.transform.gameObject.SendMessage("OnMouseDown");
            }
        }
    }
}
