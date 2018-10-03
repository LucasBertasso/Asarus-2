using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBehaviour : MonoBehaviour {
    public int damage;
    public GameObject father;
    public GameObject[] Effects;
    public float directionXForce, directionYForce;
    public string tagHit;
    Vector2 vec;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == tagHit)
        {
            vec = new Vector2(directionXForce*father.transform.localScale.x,directionYForce);
            col.SendMessage("HitRec", damage);
            col.SendMessage("Push", vec);
            gameObject.SetActive(false);
        }
    }
}
