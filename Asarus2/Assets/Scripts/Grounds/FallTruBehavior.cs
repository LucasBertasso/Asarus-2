using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTruBehavior : MonoBehaviour {

    public bool canFallTru;

    private void OnCollisionStay2D(Collision2D collision)
    {
        collision.gameObject.SendMessage("CanFallTruEnable", canFallTru, SendMessageOptions.DontRequireReceiver);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.gameObject.SendMessage("CanFallTruEnable", false , SendMessageOptions.DontRequireReceiver);
    }
}
