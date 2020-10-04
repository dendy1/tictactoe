using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject crossObject;
    [SerializeField] private GameObject noughtObject;

    private State _currentState;
    private Action<Cell> _onCellClicked;
        
    public int Row { get; private set; }
    public int Col { get; private set; }
    public State CellState
    {
        get => _currentState;
        set
        {
            _currentState = value;
            
            switch (value)
            {
                case State.Cross:
                    noughtObject.SetActive(false);
                    crossObject.SetActive(true);
                    break;
                case State.Nought:
                    noughtObject.SetActive(true);
                    crossObject.SetActive(false);
                    break;
                case State.Empty:
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
        CellState = State.Empty;
        
        name = $"Cell [{row}, {col}]";

        _onCellClicked = onCellClicked;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Cell [{Row}, {Col}] clicked");
        _onCellClicked?.Invoke(this);
    }
    
    public void OnMouseDown()
    {
        Debug.Log($"Cell [{Row}, {Col}] clicked");
        _onCellClicked?.Invoke(this);
    }
}
