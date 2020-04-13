using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextSetter : MonoBehaviour
{
    public Text text;
    public TextMeshProUGUI proText;
    public IntVariable Variable;
    public string prefix;
    public string postfix;

    private void Update()
    {
        if (text != null && Variable != null)
        {
            text.text = prefix + Variable.Value.ToString() + postfix;
        }

        if (proText != null && Variable != null)
        {
            proText.text = prefix +  Variable.Value.ToString() + postfix;
        }
    }
}
