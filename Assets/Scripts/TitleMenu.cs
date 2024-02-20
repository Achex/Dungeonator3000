using System.IO;
using UnityEngine;

public class TitleMenu : MonoBehaviour
{  
    public GameObject titleMenu;
    public GameObject loadScreen;
    public GameObject playScreen;
    private PlayMenu playMenu;
    void Start() 
    {
        if (File.Exists("Assets/Resources/stableDiffusionMaze.jpeg")) 
        {
            File.Delete("Assets/Resources/stableDiffusionMaze.jpeg");
        }
        if (File.Exists("Assets/Resources/stableDiffusionMaze.jpeg.meta")) 
        {
            File.Delete("Assets/Resources/stableDiffusionMaze.jpeg.meta");
        }

        // if (PlayerPrefs.GetString("State").Equals("Repeat"))
        // {
        //     PlayerPrefs.SetString("State", "");
        //     titleMenu.SetActive(false);
        //     playScreen.SetActive(true);
        //     playMenu = playScreen.GetComponent<PlayMenu>();
        //     GameObject.Find("ConfirmButton").SetActive(false);
        //     GameObject.Find("PromptInput").SetActive(false);
        //     GameObject.Find("AdvancedOptionsButton").SetActive(false);
        //     GameObject.Find("PlayBackButton").SetActive(false);
        //     loadScreen.SetActive(true);
        //     playMenu.PlayGame();
        // }
    }
     
    public void QuitGame()
    {
        print("Quitting game");
        Application.Quit();
    }
}
