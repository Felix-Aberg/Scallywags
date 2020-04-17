using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextSetter : MonoBehaviour
{
    public IntVariable Variable;
    public string prefix;
    public string postfix;
    
    private Text text;
    private TextMeshProUGUI proText;
    private int _lastValue;

    private void Start()
    {
        text = GetComponent<Text>();
        proText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (text != null && Variable != null)
        {
            if (Variable.Value != _lastValue)
            {
                _lastValue = Variable.Value;
                text.text = prefix + Variable.Value.ToString() + postfix;
            }
        }

        if (proText != null && Variable != null)
        { 
            if (Variable.Value != _lastValue)
            {
                _lastValue = Variable.Value;
                proText.text = prefix +  Variable.Value.ToString() + postfix;
            }
        }
    }
}
