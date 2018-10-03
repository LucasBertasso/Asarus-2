using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class CameraShakeAtivator : MonoBehaviour {
    public bool Active, controlRumble, function;
	void Update () {
        if (Active)
        {
            GameObject Cam = GameObject.FindGameObjectWithTag("MainCamera");
            Cam.SendMessage("ShakeCamera", 0.1f);
            if (controlRumble)
            {
                function = true;
                GamePad.SetVibration(PlayerIndex.One, 1, 1);
            }
        } else
        {
            if (function)
            {
                function = false;
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
            }
        }
	}
}
