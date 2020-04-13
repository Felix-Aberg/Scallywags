using UnityEngine.UI;
using UnityEngine;

[ExecuteInEditMode]
public class SliderSetterInt : MonoBehaviour
{
    private Slider Slider;
    public IntVariable Variable;
    public IntVariable MaxVariable;

    void Start()
    {
        Slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (Variable != null)
        {
            Slider.value = Variable.Value;
        }
        else
        {
            Debug.LogError("Variable is null");
        }
        
        if (MaxVariable != null)
        {
            Slider.maxValue = MaxVariable.Value;
        }
        else
        {
            Debug.LogError("MaxVariable is null");
        }
    }
}
