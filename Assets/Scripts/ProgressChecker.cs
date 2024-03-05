using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class ProgressChecker : MonoBehaviour
{
    public TMP_Text progressText;
    private string urlCode;
    private string progressAPI = "/sdapi/v1/progress";
    private string argument = "?=skip_current_image=true";

    void OnEnable() //THIS WAS START. MIGHT SCREW WITH PREVIOUS WORK THAT I CHANGED IT
    {
        //start the coroutine to make periodic requests
        urlCode = PlayerPrefs.GetString("StableDiffusionLink");
        StartCoroutine(MakePeriodicGetRequest());
    }

    IEnumerator MakePeriodicGetRequest()
    {
        while (true)
        {
            // Make the GET request with the parameter
            yield return MakeGetRequest(urlCode + progressAPI + argument);

            // Wait for one second before making the next request
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator MakeGetRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                string responseJson = webRequest.downloadHandler.text;
                string progress = JsonUtility.FromJson<ProgressResponse>(responseJson)?.progress ?? "";
                double progressDouble = Double.Parse(progress) * 100;
                double roundedResult = Math.Round(progressDouble, 0);
                if (Double.Parse(progressText.text.Substring(0, progressText.text.Length - 1)) < roundedResult) 
                {
                    progressText.text = roundedResult.ToString() + "%";
                }
            }
        }
    }
}
