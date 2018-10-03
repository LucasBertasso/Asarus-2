using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {
    [Tooltip("Seleciona se quer destruir ou desativar")]
    public bool desactive;
    [Tooltip("Tempo para executar função")]
    public float timeToDestroy;
    float timer;
	void Update () {
        timer += Time.deltaTime;
        if (!desactive && timer > timeToDestroy) Destroy(gameObject);
        if (desactive && timer > timeToDestroy) gameObject.SetActive(false);
	}
}
