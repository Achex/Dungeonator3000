using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TitleMenu : MonoBehaviour
{  
    public GameObject titleMenu;
    public GameObject loadScreen;
    public GameObject playScreen;

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

        if (File.Exists("Assets/Resources/stableDiffusionInside.jpeg")) 
        {
            File.Delete("Assets/Resources/stableDiffusionInside.jpeg");
        }
        if (File.Exists("Assets/Resources/stableDiffusionInside.jpeg.meta")) 
        {
            File.Delete("Assets/Resources/stableDiffusionInside.jpeg.meta");
        }

        if (File.Exists("Assets/Resources/stableDiffusionStreet.jpeg")) 
        {
            File.Delete("Assets/Resources/stableDiffusionStreet.jpeg");
        }
        if (File.Exists("Assets/Resources/stableDiffusionStreet.jpeg.meta")) 
        {
            File.Delete("Assets/Resources/stableDiffusionStreet.jpeg.meta");
        }

        if (File.Exists("Assets/Resources/stableDiffusionTown.jpeg")) 
        {
            File.Delete("Assets/Resources/stableDiffusionTown.jpeg");
        }
        if (File.Exists("Assets/Resources/stableDiffusionTown.jpeg.meta")) 
        {
            File.Delete("Assets/Resources/stableDiffusionTown.jpeg.meta");
        }
    }
     
    public void QuitGame()
    {
        print("Quitting game");
        Application.Quit();
    }

    public void SetPlatformerMode() 
    {
        PlayerPrefs.SetString("GameMode", "Platformer");
    }

    public void SetMazeMode()
    {
        PlayerPrefs.SetString("GameMode", "Maze");
    }

    public void SetPNCMode()
    {
        PlayerPrefs.SetString("GameMode", "PNC");
    }

    public static System.Random random;

    public void TestButton()
    {
        TestTheSequenceAlgorithm();
    }

    public void TestTheSequenceAlgorithm()
    {
        random = new();

        int height = 15;
        int width = 15;
        
        int[][] grid = new int[height][];
        for (int i = 0; i < height; i++)
        {
            grid[i] = new int[width];
            for (int j = 0; j < width; j++)
            {
                //set the floor
                if (i == 14) 
                {
                    grid[i][j] = 1;
                }
                else 
                {
                    grid[i][j] = 0;
                }
            }
        }
        
        List<int> selectedRows = GenerateSequence();

        foreach (int row in selectedRows)
        {
            int platformStart = random.Next(0,13);

            int platformEnd = random.Next(platformStart+1,14);

            if (platformEnd - platformStart > 12)
            {
                platformStart += 2;
                platformEnd -= 2;
            }

            for (int i=platformStart; i<=platformEnd; i++)
            {
                grid[row][i] = 1;
            }
        }

        PrintMaze(grid);
    }

    private static List<int> GenerateSequence() 
    {
        List<int> selectedValues = new();

        int currentRow = 0;

        while (currentRow < 10)
        {
            int gap = random.Next(2,4);

            currentRow += gap;

            selectedValues.Add(currentRow);
        }

        if (selectedValues.Contains(10) && !selectedValues.Contains(12)) 
        {
            selectedValues.Add(12);
        }
        
        return selectedValues;
    }
    
    public static void PrintMaze(int[][] maze)
    {
        string tmp = "\n";
        for (int i = 0; i < maze.Length; i++)
        {
            for (int j = 0; j < maze[i].Length; j++)
            {
                if (maze[i][j] == 1)
                    tmp += "# ";
                else
                    tmp += "X ";
            }
            tmp += "\n";
        }
        print(tmp);
    }
}
