using UnityEngine;
using DG.Tweening;
using System;

public class UICredits : MonoBehaviour
{
    [SerializeField] private GameObject textsCredits;
    [SerializeField] private float position = 200f;
    [SerializeField] private float duration = 10f;
    Tween scrollTween;

    void Start()
    {
        ScrollCredits();    
    }

    private void ScrollCredits()
    {
        scrollTween = textsCredits.transform
            .DOMoveY(position, duration)
            .SetEase(Ease.Linear);
    }
}
