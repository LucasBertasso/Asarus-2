using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour {
    public GameObject shadow;
    public int coinValue;
    public AudioClip coinAudio;
    private void Update()
    {
        if (GetComponent<Rigidbody2D>().velocity.y == 0) shadow.SetActive(true); else shadow.SetActive(false);
    }
    void Collect()
    {
        if (GetComponent<Rigidbody2D>().velocity.y == 0)
        {
            GameObject Gc = GameObject.FindGameObjectWithTag("GameController");
            Gc.SendMessage("AddCoin", coinValue);
            Gc.SendMessage("PlaySoundEffect", coinAudio);
            Destroy(gameObject);
        }
    }
}
