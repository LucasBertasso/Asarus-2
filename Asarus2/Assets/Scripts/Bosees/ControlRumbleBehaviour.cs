using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ControlRumbleBehaviour : MonoBehaviour {
    public bool Active, function;
    public float rumbleIntensity;
    void Update()
    {
        if (Active)
        {
                function = true;
                GamePad.SetVibration(PlayerIndex.One, rumbleIntensity, rumbleIntensity);
        }
        else
        {
            if (function)
            {
                function = false;
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
            }
        }
    }
}
