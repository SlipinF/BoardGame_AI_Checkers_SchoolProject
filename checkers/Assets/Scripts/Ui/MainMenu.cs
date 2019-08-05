using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public enum Mode {VsAi,VsPlayer}

public class MainMenu : MonoBehaviour
{
    public static int NumberOfPlayers = 2;
    public TextMeshProUGUI PlayerAmountText;
    public TextMeshProUGUI Modetext;
    public static bool startFromLoad;
    public static Mode currentMode = Mode.VsAi;



    public void PlayGame()
    {
        startFromLoad = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void NumberOfPlayerSelectionRight()
    {
        if (NumberOfPlayers == 2 || NumberOfPlayers == 3)
        {
            NumberOfPlayers++;
        }
        else if (NumberOfPlayers == 4)
        {
            NumberOfPlayers += 2;
        }
        else
        {
            NumberOfPlayers = 2;
        }
        PlayerAmountText.text = NumberOfPlayers.ToString();
    }



    public void NumberOfPlayerSelectionLeft()
    {
        if (NumberOfPlayers == 4 || NumberOfPlayers == 3)
        {
            NumberOfPlayers--;
        }
        else if (NumberOfPlayers == 6)
        {
            NumberOfPlayers -= 2;
        }
        else
        {
            NumberOfPlayers = 6;
        }
        PlayerAmountText.text = NumberOfPlayers.ToString();
    }
    public void StartGameFromLoad()
    {
        startFromLoad = true;
        PlayerData data = SaveSystem.LoadPlayer();
        NumberOfPlayers = data.numberOfPlayers;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void RightModeSelection()
    {
        if(currentMode == Mode.VsAi)
        {
            currentMode = Mode.VsPlayer;
            Modetext.text = "VS  Player";
        }
        else
        {
            currentMode = Mode.VsAi;
            Modetext.text = "Vs Ai";
        }
    }
}

