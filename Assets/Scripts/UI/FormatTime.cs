using System;
using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using TMPro;
using UnityEngine;

public class FormatTime : MonoBehaviour
{
    [SerializeField] private FloatVariable roundTimeUI;
    private TextMeshProUGUI _roundTimeMesh;
    private int _oldTime;

    void Start()
    {
        _roundTimeMesh = GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        if ((int) roundTimeUI.Value == _oldTime) return;
        _oldTime = (int) roundTimeUI.Value;
        _roundTimeMesh.text = FormatRoundTime();
    }

    private string FormatRoundTime()
    {
        var span = new TimeSpan(0, 0, _oldTime);
        return $"{(int) span.TotalMinutes}:{span.Seconds:00}";
    }
}
