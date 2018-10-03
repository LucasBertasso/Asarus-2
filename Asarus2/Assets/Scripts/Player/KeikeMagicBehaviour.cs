using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeikeMagicBehaviour : MonoBehaviour {
    GameObject father;
    public float speed, damage;
	void Start () {
        damage = KeikeBehaviour.damage;
	}
	void FixedUpdate () {
        GetComponent<Rigidbody2D>().velocity = transform.up * speed;
	}
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Enemy"))
        {
            col.SendMessage("HitRec", damage);
        }
    }
}
