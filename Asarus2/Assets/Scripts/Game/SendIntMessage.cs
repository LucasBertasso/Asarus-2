using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendIntMessage : MonoBehaviour {
    public bool GcBool;
    public GameObject[] messageObject;
    public string[] MessagesToSent;
    public int messageValue;
    void SendAMessage()
    {
        GameObject Gc = GameObject.FindGameObjectWithTag("GameController");
        if (!GcBool) {
            for (int i = 0; i < messageObject.Length; i++)
            {
                messageObject[i].SendMessage(MessagesToSent[i], messageValue);
            }
        }
        else
        {
            for (int i = 0; i < MessagesToSent.Length; i++)
            {
              Gc.SendMessage(MessagesToSent[i], messageValue);
            }
        }
    }
}
