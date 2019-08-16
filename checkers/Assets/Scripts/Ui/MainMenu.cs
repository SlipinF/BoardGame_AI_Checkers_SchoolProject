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
    public TextMeshProUGUI PlayerNumberText;
    public TextMeshProUGUI Modetext;
    public GameObject LoadButton;
    public static bool startFromLoad;
    public static  bool GameIsSaved = false;
    public static Mode currentMode = Mode.VsAi;


    private void Awake()
    {
      if(GameIsSaved == false)
        {
            LoadButton.GetComponent<Button>().interactable = false;
        }
      else
        {
            LoadButton.GetComponent<Button>().interactable = true;
        }
    }
    private void Start()
    {
        currentMode = Mode.VsAi;
        NumberOfPlayers = 2;
    }

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
        PlayerNumberText.text = NumberOfPlayers.ToString();
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
        PlayerNumberText.text = NumberOfPlayers.ToString();
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
    public static void ChangeSaveStatus()
    {
        GameIsSaved = true;
    }
}

