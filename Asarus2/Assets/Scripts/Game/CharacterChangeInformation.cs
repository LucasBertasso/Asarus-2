using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChangeInformation : MonoBehaviour {

    [Tooltip("Personagem inicial")]
    public int startId, idA, idB;
    [Tooltip("Habilita troca no inicio")]
    public bool canChangeInitial;
    public static bool canChange;
    public static int id1, id2, atual;
    public static float speedAtualA, speedAtualB;

	void Awake () {
        atual = startId;
        canChange = canChangeInitial;
        GameObject GC = GameObject.FindGameObjectWithTag("GameController");
        GC.SendMessage("IdChange", startId);

        id1 = idA;
        id2 = idB;
	}
    private void Update()
    {
        Physics2D.IgnoreLayerCollision(10, 10);
        Physics2D.IgnoreLayerCollision(10, 11);
        Physics2D.IgnoreLayerCollision(10, 12);
        Physics2D.IgnoreLayerCollision(11, 11);
        Physics2D.IgnoreLayerCollision(12, 12);
    }
    void SpeedAReciver(float speedA)
    {
        speedAtualA = speedA;
    }
    void SpeedBReciver(float speedB)
    {
        speedAtualB = speedB;
    }
    void ChangeCharAtual()
    {
            if (atual == id1) atual = id2; else atual = id1;
            GameObject GC = GameObject.FindGameObjectWithTag("GameController");
            GC.SendMessage("IdChange", atual);
    }
}
