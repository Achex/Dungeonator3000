using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json.Linq;
using TMPro;
using System.Collections.Generic;

public class SamplerOptions : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    private string url;
    private readonly string endpoint = "/sdapi/v1/samplers";

    void Start()
    {
        url = PlayerPrefs.GetString("StableDiffusionLink") + endpoint;
        print(url);
        StartCoroutine(GetSamplerNames());
    }

    IEnumerator GetSamplerNames()
    {
        print(url);
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                //Debug.Log(jsonResponse);

                // Parse the JSON response as a JArray
                JArray jsonArray = JArray.Parse(jsonResponse);

                // Create a list to store the names
                List<string> names = new List<string>();

                foreach (JObject sampler in jsonArray)
                {
                    string name = (string)sampler["name"];
                    names.Add(name);
                    //Debug.Log("Name: " + name);
                }

                // Assign the list of names to the dropdown options
                dropdown.ClearOptions();
                dropdown.AddOptions(names);
            }
        }
    }
}