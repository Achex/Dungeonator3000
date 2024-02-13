using UnityEngine;
using UnityEditor;
using System.IO;

public class StableDiffusionMazeInserter : MonoBehaviour
{
    private bool detected = false;

    public string imagePath = "stableDiffusionMaze";

    static void SetTextureType()
    {
        string path = "Assets/Resources/stableDiffusionMaze.jpeg"; //SD maze image path
        AssetDatabase.Refresh();
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

        if (textureImporter != null)
        {
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spriteImportMode = SpriteImportMode.Single;

            AssetDatabase.ImportAsset(path);
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError("Texture importer not found. Make sure the path is correct.");
        }
    }

    // void Start() 
    // {
    //     if (File.Exists("Assets/Resources/stableDiffusionMaze.jpeg")) 
    //     {
    //         File.Delete("Assets/Resources/stableDiffusionMaze.jpeg");
    //     }
    //     if (File.Exists("Assets/Resources/stableDiffusionMaze.jpeg.meta")) 
    //     {
    //         File.Delete("Assets/Resources/stableDiffusionMaze.jpeg.meta");
    //     }
    // }

    void Update()
    {
        if (!detected && File.Exists("Assets/Resources/stableDiffusionMaze.jpeg")) 
        {
            detected = true;
            SetTextureType();
            Sprite sprite = Resources.Load<Sprite>(imagePath);

            if (sprite != null)
            {
                //set the sprite to the SpriteRenderer component
                GetComponent<SpriteRenderer>().sprite = sprite;
            }
            else
            {
                AssetDatabase.Refresh();
                detected = false;
            }
        }
    }
}
