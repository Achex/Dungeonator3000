using System;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour
{
    public Canvas escapeMenu;
    public TMP_Text saveErrorMessage;
    public TMP_Text saveSuccessMessage;
    public Button saveMazeButton;

    public void Start()
    {
        // Call the method to check for escape key press
        CheckForEscapeKeyPress();
        escapeMenu.enabled = false;
    }

    public void Update()
    {
        // Call the method to check for escape key press
        CheckForEscapeKeyPress();
    }

    private void CheckForEscapeKeyPress()
    {
        // Check for the Escape key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle the menu visibility
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        // Toggle the active state of the menu UI
        escapeMenu.enabled = !escapeMenu.enabled;
        if (saveErrorMessage.gameObject.activeSelf)
        {
            saveErrorMessage.gameObject.SetActive(false);
        }
        if (saveSuccessMessage.gameObject.activeSelf)
        {
            saveSuccessMessage.gameObject.SetActive(false);
        }
    }

    public void MainMenu() 
    {
        File.Delete("Assets/Resources/stableDiffusionMaze.jpeg");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void SaveMaze() 
    {
        saveMazeButton.gameObject.SetActive(false);

        string sourceFilePath = "Assets/Resources/stableDiffusionMaze.jpeg"; // Path to the source image file
        string destinationFolderPath = "Assets/Resources/Saved/"; // Path to the destination folder

        // Ensure the destination folder exists, create it if it doesn't
        Directory.CreateDirectory(destinationFolderPath);

        DateTime currentDate = DateTime.Now;
        string currentDateString = currentDate.ToString("dd-MM-yy");

        int index = 1;
        string uniqueFileName = currentDateString + "_" + index.ToString() + ".jpeg";
        while (File.Exists(Path.Combine(destinationFolderPath, uniqueFileName)))
        {
            // If a file with the same name exists, append a numerical index to the filename
            index++;
            uniqueFileName = $"{currentDateString}_{index}.jpeg";
        }

        // Combine the destination folder path with the filename to get the full destination path
        string destinationFilePath = Path.Combine(destinationFolderPath, uniqueFileName);

        try
        {
            // Copy the image file to the destination folder
            File.Copy(sourceFilePath, destinationFilePath);

            saveSuccessMessage.gameObject.SetActive(true);

            SaveMetaInfo(destinationFilePath);
            AssetDatabase.Refresh();
        }
        catch (FileNotFoundException)
        {
            string errorMessage = "Error: Source image not found.";
            Debug.Log(errorMessage);
            saveErrorMessage.gameObject.SetActive(true);
            saveErrorMessage.text = errorMessage;
        }
        catch (Exception ex)
        {
            Debug.Log($"Error: {ex.Message}");
            saveErrorMessage.gameObject.SetActive(true);
            saveErrorMessage.text = ex.Message;
        }


    }

    private void SaveMetaInfo(string destinationFilePath) 
    {
        string flattenedString = PlayerPrefs.GetString("MazeGrid");

        // string[] rowStrings = flattenedString.Split(';');

        // int[][] mazeGrid = new int[rowStrings.Length][];
        // for (int i = 0; i < rowStrings.Length; i++)
        // {
        //     string[] stringValues = rowStrings[i].Split(',');
        //     mazeGrid[i] = new int[stringValues.Length];

        //     for (int j = 0; j < stringValues.Length; j++)
        //     {
        //         mazeGrid[i][j] = int.Parse(stringValues[j]);
        //     }
        // }

        string destinationFilePathMeta = destinationFilePath.Substring(0, destinationFilePath.Length - 4) + ".txt";

        File.WriteAllText(destinationFilePathMeta, flattenedString);
    }
}
    