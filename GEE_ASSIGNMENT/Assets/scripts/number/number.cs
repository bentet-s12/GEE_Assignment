using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class number : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Slider slider;
    [SerializeField] private AudioMixer mixer;
    public void start()
    {
        if (PlayerPrefs.GetFloat("audio") > 0)
        {
            loadvalue();

        }
        else
        {

            slider.value = 100f;
        }
            updatevalue();
        
    }

    public void updatevalue()
    {
        float value = slider.value;


        text.SetText(value.ToString("0.00"));

        if (mixer != null)
        {
            float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;

            mixer.SetFloat("volofmaster", dB);
        }
    }
    public void savevalue()
    {
        PlayerPrefs.SetFloat("audio", slider.value);
    }

    public void loadvalue()
    {
        slider.value = PlayerPrefs.GetFloat("audio");
    
    }
}
