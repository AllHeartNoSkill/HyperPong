using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateHandler : MonoBehaviour
{
    [Header("Frame Settings")] 
    public int TargetFrameRate = 60;
    public bool UseVSync = false;
    public int VSyncAmount = 1;

    IEnumerator Start()
    {
        yield return 0;
        UpdateFrameSetting();
    }

    private void OnValidate()
    {
        if (!Application.isPlaying) return;
        UpdateFrameSetting();
    }

    void UpdateFrameSetting()
    {
        QualitySettings.vSyncCount = 0;
        if (UseVSync)
        {
            Application.targetFrameRate = Screen.currentResolution.refreshRate / VSyncAmount;
        }
        else
        {
            Application.targetFrameRate = TargetFrameRate;
        }
    }
}
