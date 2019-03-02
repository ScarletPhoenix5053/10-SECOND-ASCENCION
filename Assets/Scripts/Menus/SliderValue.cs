using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SliderValue : MonoBehaviour
{
    Text tx;

    private void Awake()
    {
        tx = GetComponent<Text>();
    }

    public void UpdateValue(float value)
    {
        tx.text = (value * 100f).ToString("F0");
    }
}
