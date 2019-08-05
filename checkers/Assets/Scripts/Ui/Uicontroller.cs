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
        ChangeText(currentPlayerId);
    }

    public void QuitGameButton()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }


    public void ChangeText(int currentplayer)
    {
        switch (currentplayer)
        {
            case 3:
                ColorText.text = "red";
                break;
            case 4:
                ColorText.text = "blue";
                break;
            case 5:
                ColorText.text = "Yellow";
                break;
            case 6:
                ColorText.text = "orange";
                break;
            case 7:
                ColorText.text = "green";
                break;
            case 8:
                ColorText.text = "purple";
                break;

            default:
                break;
        }
    }

    public void DisplayEndScreen(Player p)
    {
        endScreen.SetActive(true);
    }
}
