using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharImageBehaviour : MonoBehaviour
{
    public GameObject[] CharImages;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < CharImages.Length; i++)
        {
            if (CharacterChangeInformation.atual == i) CharImages[CharacterChangeInformation.atual].SetActive(true);
            else CharImages[i].SetActive(false);
        }
        
	}
}
