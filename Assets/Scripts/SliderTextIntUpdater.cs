using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderTextIntUpdater : MonoBehaviour
{
    public Slider slider;
    public TMP_Text textMeshPro;

    private void Start()
    {
        // Update text initially
        UpdateText();
    }

    public void UpdateText()
    {
        
        // Update the TextMeshPro component with the current value of the slider
        textMeshPro.text = slider.value.ToString();
    }
}