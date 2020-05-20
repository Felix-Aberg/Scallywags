using UnityEngine;
using UnityEngine.UI;

public class StartGameProgressBar : MonoBehaviour
{
    private float _value;
    private Slider _progressBar;
    void Start()
    {
        _progressBar = GetComponentInChildren<Slider>();
        _progressBar.wholeNumbers = false;
    }

    public void UpdateSlider(float value, float max)
    {
        _progressBar.maxValue = max;
        _progressBar.value = value;
    }
}
