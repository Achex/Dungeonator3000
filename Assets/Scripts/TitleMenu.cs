using System.IO;
using UnityEngine;

public class TitleMenu : MonoBehaviour
{  
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
    }
     
    public void QuitGame()
    {
        print("Quitting game");
        Application.Quit();
    }
}
