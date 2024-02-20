using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using Newtonsoft.Json;
using static MazeGen;
using UnityEngine.UI;

public class PlayMenu : MonoBehaviour
{    
    public TMP_InputField promptInput;

    string FlattenJaggedArray(int[][] array)
    {
        string[] rows = new string[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            rows[i] = string.Join(",", array[i]);
        }

        return string.Join(";", rows);
    }

    public void Start() 
    {
        urlCode = PlayerPrefs.GetString("StableDiffusionLink");
    }

    private int[][] mazeGrid;

    public void PlayGame() 
    {
        print("Playing game");
        //string prompt = promptInput.text;
        //print("prompt: " + prompt);
        // PlayerPrefs.SetString("Prompt", promptInput.text);
        // PlayerPrefs.Save();

        // 1: CALL MAZE GEN
        mazeGrid = NewMaze();

        //Flatten the 2D array into a 1D array
        string flattenedString = FlattenJaggedArray(mazeGrid);
        PlayerPrefs.SetString("MazeGrid", flattenedString);
        PlayerPrefs.Save();
        
        PrintMaze(mazeGrid);

        // 2: CALL STABLE DIFFUSION
        StartCoroutine(ProcessImages());

    }

    public TMP_InputField NegativePromptSetting;
    public TMP_Dropdown SamplerSetting;
    public Slider SamplingStepsSlierSetting;
    public Slider CFGScaleSetting;
    public TMP_Dropdown UpscalerSetting;
    public Slider DenoisingStrengthSetting;
    public Slider UpscaleAmountSlider;
    public TMP_Dropdown ModelSetting;

    private string urlCode;
    private readonly string controlnetMaze = "Assets/Resources/mazeImage.png";
    private readonly string controlnetPreprocessor = "lineart_standard (from white bg & black line)";
    private readonly string controlnetModel = "control_v11p_sd15_seg_fp16 [ab613144]";
    private readonly string outputFileName = "Assets/Resources/stableDiffusionMaze.jpeg";

    //STABLE DIFFUSION CALLER
    IEnumerator ProcessImages()
    {
        //encode image to base64 for json request
        byte[] imageBytes = File.ReadAllBytes(controlnetMaze);
        string encodedImage = Convert.ToBase64String(imageBytes);

        ControlnetRequest controlnetJsonData = new()
        {
            controlnet_module = controlnetPreprocessor,
            controlnet_input_images = new List<string> {encodedImage},
            controlnet_processor_res = 512,
            controlnet_threshold_a = 64,
            controlnet_threshold_b = 64
        };

        UnityWebRequest controlnetRequest = UnityWebRequest.Post(urlCode + "/controlnet/detect", JsonUtility.ToJson(controlnetJsonData), "application/json");
        yield return controlnetRequest.SendWebRequest();

        string controlnetResponseJson = controlnetRequest.downloadHandler.text;

        print("controlnet response: " + controlnetResponseJson);

        string encodedPreprocessedImage = JsonUtility.FromJson<ImageResponse>(controlnetResponseJson).images[0];

        byte[] decodedBytes = Convert.FromBase64String(encodedPreprocessedImage);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(decodedBytes);

        string modelCheck = ModelSetting.options[ModelSetting.value].text == string.Empty ? PlayerPrefs.GetString("DefaultModel") : ModelSetting.options[ModelSetting.value].text;
        string upscalerCheck = UpscalerSetting.options[UpscalerSetting.value].text == string.Empty ? PlayerPrefs.GetString("DefaultUpscaler") : UpscalerSetting.options[UpscalerSetting.value].text;
        string samplerCheck = SamplerSetting.options[SamplerSetting.value].text == string.Empty ? PlayerPrefs.GetString("DefaultSampler") : SamplerSetting.options[SamplerSetting.value].text;

        print("---------INITIAL----------");

        print(promptInput.text);
        print(NegativePromptSetting.text);
        print(modelCheck);
        print(samplerCheck);
        print(SamplingStepsSlierSetting.value);
        print(CFGScaleSetting.value);
        print(upscalerCheck);
        print(DenoisingStrengthSetting.value);
        print(UpscaleAmountSlider.value);

        PlayerPrefs.SetString("Prompt", promptInput.text);
        PlayerPrefs.SetString("NegativePrompt", NegativePromptSetting.text);
        PlayerPrefs.SetString("ModelSetting", modelCheck);
        PlayerPrefs.SetString("SamplerSetting", samplerCheck);
        PlayerPrefs.SetFloat("SamplingStepsSlierSetting", SamplingStepsSlierSetting.value);
        PlayerPrefs.SetFloat("CFGScaleSetting", CFGScaleSetting.value);
        PlayerPrefs.SetString("UpscalerSetting", upscalerCheck);
        PlayerPrefs.SetFloat("DenoisingStrengthSetting", DenoisingStrengthSetting.value);
        PlayerPrefs.SetFloat("UpscaleAmountSlider", UpscaleAmountSlider.value);
        PlayerPrefs.Save();

        print("----------------------------");

        
        StableDiffusionRequest jsonData = new()
        {
            sd_model_checkpoint = modelCheck,
            prompt = promptInput.text,
            negative_prompt = NegativePromptSetting.text,
            batch_size = 1,
            steps = Mathf.RoundToInt(SamplingStepsSlierSetting.value),
            cfg_scale = CFGScaleSetting.value,
            enable_hr = true,
            hr_scale = UpscaleAmountSlider.value,
            hr_upscaler = upscalerCheck,
            hr_second_pass_steps = 20,
            denoising_strength = DenoisingStrengthSetting.value,
            sampler_index = samplerCheck,
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

        print("Stable Diffusion Response: " + responseJson);

        string imageBase64 = JsonUtility.FromJson<ImageResponse>(responseJson).images[0];

        byte[] imageBytesOutput = Convert.FromBase64String(imageBase64);
        Texture2D texOutput = new Texture2D(2, 2);
        texOutput.LoadImage(imageBytesOutput);

        SaveImage(texOutput, outputFileName);

        //print("Done, image saved as " + outputFileName);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void SaveImage(Texture2D texture, string fileName)
    {
        byte[] bytes = texture.EncodeToJPG();
        File.WriteAllBytes(fileName, bytes);
    }
}
