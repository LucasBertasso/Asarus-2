using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialJumpBehaviour : MonoBehaviour {
    public Rigidbody2D rig;
    public float[] forceX = new float[2];
    public float[] forceY = new float[2];

    void Start () {
        rig.AddForce(new Vector2(Random.Range(forceX[0],forceX[1]),Random.Range(forceY[0],forceY[1])));
	}
}
