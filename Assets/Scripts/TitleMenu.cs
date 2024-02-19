using System.IO;
using UnityEngine;

public class TitleMenu : MonoBehaviour
{  
    public GameObject titleMenu;
    public GameObject loadScreen;
    public PlayMenu playMenu;
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

        if (PlayerPrefs.GetString("State").Equals("Repeat"))
        {
            PlayerPrefs.SetString("State", "");
            titleMenu.SetActive(false);
            loadScreen.SetActive(true);
            playMenu.PlayGame();
        }
    }
     
    public void QuitGame()
    {
        print("Quitting game");
        Application.Quit();
    }
}
