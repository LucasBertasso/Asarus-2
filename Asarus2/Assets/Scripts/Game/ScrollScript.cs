using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScript : MonoBehaviour {

    public float speedX, speedY; // Velocidade de movimento X e Y

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Cria novo Vetor de duas dimenções que movimenta o objeto
        Vector2 offset = new Vector2(Time.time * speedX, Time.time * speedY);
        // Recebe componente Renderer
        Renderer renderer = GetComponent<Renderer>();
        // Aplica a função ao material
        renderer.material.mainTextureOffset = offset;

	}
}
