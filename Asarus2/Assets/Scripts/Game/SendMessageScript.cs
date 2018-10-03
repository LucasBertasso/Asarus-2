using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessageScript : MonoBehaviour {
    [Tooltip("Objetos que reberão as mensagens")]
    public GameObject[] GameObjectList;
    [Tooltip("As mensagens não podem possuir variáveis")]
    public string[] MensagesToSend;
    void SendToAll()
    {
        for(int i = 0; i < GameObjectList.Length; i++)
        {
            GameObjectList[i].SendMessage(MensagesToSend[i]);
        }
    }
    void SendAMessage()
    {
        for (int i = 0; i < GameObjectList.Length; i++)
        {
            GameObjectList[i].SendMessage(MensagesToSend[i]);
        }
    }
}
