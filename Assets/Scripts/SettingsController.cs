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

    public void OnPlayButtonPressed()
    {
        board.InitializePlayers(playerOneHuman.isOn, playerTwoHuman.isOn);
        board.StartNewGame();
        gameObject.SetActive(false);
    }

    public void SetHeaderText(string text)
    {
        headerText.text = text;
    }
}
