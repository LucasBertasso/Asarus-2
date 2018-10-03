using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageBehaviour : MonoBehaviour {
    public int dmg;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "PlayerAtual")
        {
            col.SendMessage("Hit", dmg);
        }
    }
}
