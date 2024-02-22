using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor.VersionControl;
using UnityEditorInternal;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ImageLoader : MonoBehaviour
{
    public GameObject imagePrefab;
    public Transform content;
    public int imagesPerRow = 4;
    public GameObject loadImagesScreen;
    public GameObject titleScreen;

    void OnEnable()
    {
        LoadImages();
    }

    void LoadImages()
    {
        AssetDatabase.Refresh();
        string[] files = Directory.GetFiles(Application.dataPath + "/Resources/Saved/", "*.jpeg");
        int rowIndex = 0;
        int columnIndex = 0;

        foreach (string file in files)
        {
            byte[] imageData = File.ReadAllBytes(file);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            GameObject imageObject = Instantiate(imagePrefab, content);
            imageObject.name = Path.GetFileName(file);
            Image imageComponent = imageObject.GetComponent<Image>();
            imageComponent.sprite = sprite;

            // Calculate position based on row and column index
            float posX = columnIndex * (content.GetComponent<RectTransform>().rect.width / imagesPerRow);
            float posY = -rowIndex * (content.GetComponent<RectTransform>().rect.height / Mathf.Ceil(files.Length / (float)imagesPerRow));
            imageObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);

            columnIndex++;
            if (columnIndex >= imagesPerRow)
            {
                columnIndex = 0;
                rowIndex++;
            }

            // Add EventTrigger component and attach PointerClick event
            EventTrigger trigger = imageObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { OnMazeClicked(file); });
            trigger.triggers.Add(entry);

            imageObject.SetActive(true);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
        imagePrefab.gameObject.SetActive(false);
    }

    private void OnMazeClicked(string file)
    {
        File.Copy(file, "Assets/Resources/stableDiffusionMaze.jpeg");

        string mazeGridInfo = file.Substring(0, file.Length - 4) + ".txt";

        PlayerPrefs.SetString("MazeGrid", File.ReadAllText(mazeGridInfo));

        string[] lines = File.ReadAllLines(file.Substring(0, file.Length - 4) + "_settings.txt");

        PlayerPrefs.SetString("Prompt", lines[0]);
        PlayerPrefs.SetString("NegativePrompt", lines[1]);
        PlayerPrefs.SetString("ModelSetting", lines[2]);
        PlayerPrefs.SetString("SamplerSetting", lines[3]);
        PlayerPrefs.SetFloat("SamplingStepsSlierSetting", float.Parse(lines[4]));
        PlayerPrefs.SetFloat("CFGScaleSetting", float.Parse(lines[5]));
        PlayerPrefs.SetString("UpscalerSetting", lines[6]);
        PlayerPrefs.SetFloat("DenoisingStrengthSetting", float.Parse(lines[7]));
        PlayerPrefs.SetFloat("UpscaleAmountSlider", float.Parse(lines[8]));
        PlayerPrefs.SetString("GameMode", lines[9]);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void BackButton() 
    {
        // Iterate through all child objects under the Content
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            // Destroy each child object
            Destroy(content.GetChild(i).gameObject);
        }
        
        loadImagesScreen.SetActive(false);
        titleScreen.SetActive(true);
    }
}