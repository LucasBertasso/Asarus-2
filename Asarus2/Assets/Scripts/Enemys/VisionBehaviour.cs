using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionBehaviour : MonoBehaviour {
    public GameObject Enemy;
 void Vision(string MessageName)
    {
        Enemy.SendMessage(MessageName,SendMessageOptions.DontRequireReceiver);
    }
}
