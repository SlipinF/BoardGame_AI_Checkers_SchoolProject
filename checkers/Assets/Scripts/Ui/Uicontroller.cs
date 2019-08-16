using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Uicontroller : MonoBehaviour
{
    public List<Sprite> sprites;
    public TextMeshProUGUI ColorText;
    public TextMeshProUGUI EndScreenName;
    public Image imageToChange;
    public GameObject gamebackground;
    public GameObject endScreen;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamebackground.activeSelf)
            {
                gamebackground.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                gamebackground.SetActive(true);
                Time.timeScale = 0.1f;
            }
        }
    }


    public void ChangeThePlayerColorIcon(int currentPlayerId)
    {
        imageToChange.sprite = sprites[currentPlayerId];
        ChangeText(currentPlayerId, ColorText);
    }

    public void QuitGameButton()
    {
        SceneManager.LoadScene(0);
    }

    public  void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }


    public void ChangeText(int currentplayer, TextMeshProUGUI ColorText)
    {
        switch (currentplayer)
        {
            case 2:
                ColorText.text = "RED";
                break;
            case 3:
                ColorText.text = "BLUE";
                break;
            case 4:
                ColorText.text = "YELLOW";
                break;
            case 5:
                ColorText.text = "ORANGE";
                break;
            case 6:
                ColorText.text = "GREEN";
                break;
            case 7:
                ColorText.text = "PURPLE";
                break;
            default:
                break;
        }
    }

    public void DisplayEndScreen(Player p)
    {
        ChangeText((int)p.id, EndScreenName);
        endScreen.SetActive(true);
    }
}
