﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace ScallyWags
{
    public class Quit : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            QuitProgram();
        }
    }

    public void QuitProgram()
    {
        Application.Quit();
    }
}
}
