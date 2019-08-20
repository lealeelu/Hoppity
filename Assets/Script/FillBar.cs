using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillBar : MonoBehaviour
{
    [SerializeField]
    private Image emptyBar;
    [SerializeField]
    private Image fillBar;
    [SerializeField]
    public int Min = 0;
    [SerializeField]
    public int Max = 10;
    [SerializeField]
    private int Starting = 0;

    private int _value;
    private float stepWidth;

    public int GetValue()
    {
        return _value;
    }

    public void SetValue(int value)
    {
        if (value > Max)
        {
            Debug.LogError("Fill bar can't hold anymore captain!");
            return;
        }
        else if (value < Min)
        {
            Debug.LogError("Fillbar can't go that low.");
            return;
        }
        else
        {
            _value = value;
            fillBar.rectTransform.sizeDelta = new Vector2(stepWidth * value, fillBar.rectTransform.rect.height);
        }
    }

    private void Start()
    {
        stepWidth = emptyBar.rectTransform.rect.width / (float)Max;
        new Vector2(0, fillBar.rectTransform.rect.height);
    }

    public bool ReachedMax()
    {
        return _value > Max;
    }

    public void Increment(int byAmount = 1)
    {
        SetValue(_value + byAmount);
    }
}
