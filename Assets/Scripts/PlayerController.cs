using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TMP_Text playerText;

    public string Name { get; set; }
    public PlayerType PlayerType { get; set; }
    public CellState CellState { get; set; }
    public CellState OtherCellState => CellState == CellState.Cross ? CellState.Nought : CellState.Cross;

    private readonly string[] _attackTriggers = {"Attack1", "Attack2", "Attack3", "Attack4"};
    private readonly string[] _deathTriggers = {"Death1", "Death2"};
    
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Initialize(string playerName, PlayerType playerType, CellState cellState)
    {
        Name = playerName;
        PlayerType = playerType;
        CellState = cellState;
        
        SetText(ToString());
    }
    
    public void SetText(string text)
    {
        playerText.text = text;
    }
    
    public override string ToString()
    {
        return $"{Name} [{CellState}] ";
    }

    public void Attack()
    {
        _animator.SetTrigger(_attackTriggers[Random.Range(0, _attackTriggers.Length - 1)]);
    }
    
    public void Death()
    {
        _animator.SetTrigger(_deathTriggers[Random.Range(0, _deathTriggers.Length - 1)]);
    }

    public void Sit()
    {
        _animator.SetTrigger("Sit");
    }

    public void Up()
    {
        _animator.SetTrigger("Up");
    }
}
