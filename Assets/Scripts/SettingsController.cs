using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("UI Components")] 
    [SerializeField] private TMP_Text headerText; 
    
    [Header("UI Toggles")] 
    [SerializeField] private Toggle playerOneHuman;
    [SerializeField] private Toggle playerTwoHuman;
    
    [Header("Board")] 
    [SerializeField] private Board board;

    private Animator _animator;
    private static readonly int Enabled = Animator.StringToHash("enabled");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        SetActive(true);
    }

    public void OnPlayButtonPressed()
    {
        board.CreateCells();
        board.InitializePlayers(playerOneHuman.isOn, playerTwoHuman.isOn);
        board.StartNewGame();
        
        board.FirstPlayer.Up();
        board.SecondPlayer.Up();
        
        SetActive(false);
    }

    public void SetHeaderText(string text)
    {
        headerText.text = text;
    }

    public void SetActive(bool value)
    { 
        _animator.SetBool(Enabled, value);
    }
}
