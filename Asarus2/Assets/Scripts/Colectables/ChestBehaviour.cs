using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehaviour : MonoBehaviour {

    [Header("Animations & References")]
    [Space(5)]

    [Tooltip("Indica qual ação que ocorrerá ao interagir com linguagens diferentes")]
    public string[] functionName;
    [Tooltip("Qual animação quem interagiu vai ter")]
    public string animationName;
    [Tooltip("Direção do personagem")]
    public int direction;
    [Tooltip("Personagem referente à alteração, gerado automaticamente por colisão")]
    public GameObject playerRef, Spawner;
    public float[] Clocks, Timers;
    int a = 0;
    public AudioClip itenSound;
    public AudioClip[] audios;

    [Space(5)]

    [Tooltip("Objetos ativados/desativados")]
    public GameObject[] activationList, desactivationList, players;

    public bool sendMessage,enabledToUse, disableOnEnd, spawning = false;


    void Update()
    {
        if (spawning)
        {
            ActvationObjects();
        }
        if(GetComponent<Animator>().GetBool("Open") == true)
        {
            GetComponent<BoxCollider2D>().enabled = (false);
        }
        if (enabledToUse)
        {
            if (Input.GetButtonDown("A") && playerRef.GetComponent<Rigidbody2D>().velocity.x == 0
                && playerRef.GetComponent<Rigidbody2D>().velocity.y == 0 && 
                GetComponent<Animator>().GetBool("Open") == false)
            {
                GameObject Gc = GameObject.FindGameObjectWithTag("GameController");
                Gc.SendMessage("PlaySoundEffect", audios[0]);
                if (sendMessage)
                {
                    gameObject.SendMessage("SendAMessage");
                }
                spawning = true;
                GetComponent<Animator>().SetBool("Open", true);
                if (disableOnEnd) enabledToUse = false;

            }
        }
    }
    void EnableInteraction(int charId)
    {
        if(GetComponent<Animator>().GetBool("Open") == false)
        {
            if (HudGerenciator.interactiveOn == false)
            {
                HudGerenciator.functionNameInteraction = functionName[OptionsSaveScript.atualLanguage];
                HudGerenciator.interactiveOn = true;
                enabledToUse = true;
                Debug.Log("interactiveOn");
            }
        }
    }
    void DisableInteraction(bool sit)
    {
        HudGerenciator.interactiveOn = false;
        enabledToUse = sit;
        Debug.Log("interactiveOff");
    }
    void PlayerReference(GameObject player)
    {
        playerRef = player;
    }
    void ActvationObjects()
    {
        if (activationList.Length > 0)
        {
            for (int i = 0; i < activationList.Length; i++)
            {
                    Clocks[0] += Time.deltaTime;
                    if (Clocks[0] > Timers[0]) 
                    {
                        Instantiate(activationList[i], Spawner.transform.position, Spawner.transform.rotation);
                        GameObject Gc = GameObject.FindGameObjectWithTag("GameController");
                        Gc.SendMessage("PlaySoundEffect", itenSound);
                        i++;
                        a++;
                        Clocks[0] = 0;
                    }
            }
            if (a == activationList.Length)
            {
                spawning = false;
            }
        }
    }

}