using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using Newtonsoft.Json;
using static MazeGen;
using static InstallWalls;
using UnityEngine.SceneManagement;


public class Repeat : MonoBehaviour
{    
    string FlattenJaggedArray(int[][] array)
    {
        string[] rows = new string[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            rows[i] = string.Join(",", array[i]);
        }

        return string.Join(";", rows);
    }

    private int[][] mazeGrid;

    public void Update() 
    {
        if (PlayerPrefs.GetString("State").Equals("Repeat"))
        {
            cube.SetActive(false);
            PlayerPrefs.SetString("State", "Load");
            MakeNewMaze();
        }
    }

    public void MakeNewMaze() 
    {
        // GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();

        // foreach (GameObject obj in allGameObjects)
        // {
        //     if (obj.name.Contains("Wall") || obj.name.Contains("Left Cap") || obj.name.Contains("Right Cap") || obj.name.Contains("Goal"))
        //     {
        //         Destroy(obj.gameObject);
        //     }
        // }

        // 1: CALL MAZE GEN
        mazeGrid = NewMaze();

        //Flatten the 2D array into a 1D array
        string flattenedString = FlattenJaggedArray(mazeGrid);
        PlayerPrefs.SetString("MazeGrid", flattenedString);
        PlayerPrefs.Save();
        
        PrintMaze(mazeGrid);
        
        Install(mazeGrid, PlayerPrefs.GetString("GameMode"));

        // 2: CALL STABLE DIFFUSION
        StartCoroutine(ProcessImages());

    }

    public GameObject loadScreen;
    public GameObject cube;

    private string urlCode;
    private readonly string controlnetMaze = "Assets/Resources/mazeImage.png";
    private readonly string controlnetPreprocessor = "lineart_standard (from white bg & black line)";
    private readonly string controlnetModel = "control_v11p_sd15_seg_fp16 [ab613144]";
    private readonly string outputFileName = "Assets/Resources/stableDiffusionMaze.jpeg";

    //STABLE DIFFUSION CALLER
    IEnumerator ProcessImages()
    {
        urlCode = PlayerPrefs.GetString("StableDiffusionLink");

        //encode image to base64 for json request
        byte[] imageBytes = File.ReadAllBytes(controlnetMaze);
        string encodedImage = Convert.ToBase64String(imageBytes);

        print("Encoded image: " + encodedImage);

        ControlnetRequest controlnetJsonData = new()
        {
            controlnet_module = controlnetPreprocessor,
            controlnet_input_images = new List<string> {encodedImage},
            controlnet_processor_res = 512,
            controlnet_threshold_a = 64,
            controlnet_threshold_b = 64
        };


        UnityWebRequest controlnetRequest = UnityWebRequest.Post(urlCode + "/controlnet/detect", JsonUtility.ToJson(controlnetJsonData), "application/json");

        print("check 1, json sending is: " + JsonUtility.ToJson(controlnetJsonData));

        yield return controlnetRequest.SendWebRequest();

        print("check 2");

        string controlnetResponseJson = controlnetRequest.downloadHandler.text;

        print("Controlnet response: " + controlnetResponseJson);

        string encodedPreprocessedImage = JsonUtility.FromJson<ImageResponse>(controlnetResponseJson).images[0];

        byte[] decodedBytes = Convert.FromBase64String(encodedPreprocessedImage);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(decodedBytes);

        print("---------REPEAT------------");

        string promptSetting = PlayerPrefs.GetString("Prompt");
        string negativePromptSetting = PlayerPrefs.GetString("NegativePrompt");
        string modelSetting = PlayerPrefs.GetString("ModelSetting");
        string samplerSetting = PlayerPrefs.GetString("SamplerSetting");
        float samplingStepsSliderSetting = PlayerPrefs.GetFloat("SamplingStepsSlierSetting");
        float CFGScaleSetting = PlayerPrefs.GetFloat("CFGScaleSetting");
        string upscalerSetting = PlayerPrefs.GetString("UpscalerSetting");
        float denoisingStrengthSetting = PlayerPrefs.GetFloat("DenoisingStrengthSetting");
        float upscaleAmountSlider = PlayerPrefs.GetFloat("UpscaleAmountSlider");

        print(promptSetting);
        print(negativePromptSetting);
        print(modelSetting);
        print(samplerSetting);
        print(samplingStepsSliderSetting);
        print(CFGScaleSetting);
        print(upscalerSetting);
        print(denoisingStrengthSetting);
        print(upscaleAmountSlider);

        print("----------------------------");

        
        StableDiffusionRequest jsonData = new()
        {
            sd_model_checkpoint = modelSetting,
            prompt = promptSetting,
            negative_prompt = negativePromptSetting,
            batch_size = 1,
            steps = Mathf.RoundToInt(samplingStepsSliderSetting),
            cfg_scale = CFGScaleSetting,
            enable_hr = true,
            hr_scale = upscaleAmountSlider,
            hr_upscaler = upscalerSetting,
            hr_second_pass_steps = 20,
            denoising_strength = denoisingStrengthSetting,
            sampler_index = samplerSetting,
            alwayson_scripts = new AlwaysonScripts()
            {
                controlnet = new ControlnetScript() 
                {
                    args = new List<ControlnetArgs>() 
                    {
                        new ControlnetArgs() 
                        {
                            input_image = encodedPreprocessedImage, //encodedImage bypasses preprocessor, switch to encodedPreprocessedImage to use preprocessor
                            model = controlnetModel,
                            resize_mode = 1,
                            control_mode = 0,
                            weight = 1
                        }   
                    }
                }
            }
        };

        UnityWebRequest request = UnityWebRequest.Post(urlCode + "/sdapi/v1/txt2img", JsonConvert.SerializeObject(jsonData), "application/json");
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = 3600;
        yield return request.SendWebRequest();

        string responseJson = request.downloadHandler.text;

        print("Stable Diffusion response: " + responseJson);

        string imageBase64 = JsonUtility.FromJson<ImageResponse>(responseJson).images[0];

        byte[] imageBytesOutput = Convert.FromBase64String(imageBase64);
        Texture2D texOutput = new Texture2D(2, 2);
        texOutput.LoadImage(imageBytesOutput);

        SaveImage(texOutput, outputFileName);

        //print("Done, image saved as " + outputFileName);

        loadScreen.SetActive(false);
        cube.SetActive(true);
        PlayerPrefs.SetString("State", "Play");

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void SaveImage(Texture2D texture, string fileName)
    {
        byte[] bytes = texture.EncodeToJPG();
        File.WriteAllBytes(fileName, bytes);
    }
}
