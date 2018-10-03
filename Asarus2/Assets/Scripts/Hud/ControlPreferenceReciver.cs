using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPreferenceReciver : MonoBehaviour {
    public GameObject[] GameObjectList;
	void Update () {
		for(int i = 0; i<GameObjectList.Length; i++)
        {
            if(i == OptionsSaveScript.atualControl)
            {
                GameObjectList[i].SetActive(true);
            } else GameObjectList[i].SetActive(false);
        }
	}
}
