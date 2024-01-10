using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputButtonScript : MonoBehaviour
{
    [SerializeField]
    private InputButtonDisplay left, right, special;

    [SerializeField]
    private PaddleInput paddleInput;

    private void LateUpdate()
    {
        float dir = paddleInput.XDir;
        bool sp = paddleInput.Special;
        
        left.SetHighlight(dir < 0, true);
        right.SetHighlight(dir > 0, true);
        special.SetHighlight(sp, true);
    }
}
