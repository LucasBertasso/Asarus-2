using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxBehaviour : MonoBehaviour {
    public GameObject father;
    void HitRec(int dmg)
    {
        father.SendMessage("Hit", dmg);
    }
    void Push(Vector2 vec)
    {
        father.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        father.GetComponent<Rigidbody2D>().AddForce(vec);
    }
}
