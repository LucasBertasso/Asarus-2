using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class SpawnerAtivatorBehaviour : MonoBehaviour
{
    public GameObject[] Objects;
    public GameObject spawner;
    public bool Active, controlRumble, function, randomRotation;
    public float[] timers = new float[2];
    float cloack,timer = 0;
    void Update () {
        if (Active)
        {
            cloack += Time.deltaTime;
            if(cloack >= timer)
            {
                if (randomRotation)
                {
                    Vector3 vec = new Vector3(0, 0, Random.Range(0, 360));
                    for (int i = 0; i < Objects.Length; i++)
                    {
                        Instantiate(Objects[i], spawner.transform.position,Quaternion.LookRotation(vec));
                    }
                }
                else
                {
                    for (int i = 0; i < Objects.Length; i++)
                    {
                        Instantiate(Objects[i], spawner.transform.position, spawner.transform.rotation);
                    }
                }
                timer = Random.Range(timers[0], timers[1]);
                cloack = 0;
            }
            if (controlRumble)
            {
                function = true;
                GamePad.SetVibration(PlayerIndex.One, 0.1f, 0.1f);
            }
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

