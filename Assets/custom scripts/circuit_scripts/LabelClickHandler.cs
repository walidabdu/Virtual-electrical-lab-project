
using UnityEngine;
using System;

public class LabelClickHandler : MonoBehaviour
{
    public event Action OnClick;

    void OnMouseDown()
    {
        OnClick?.Invoke();
    }
}