using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatBehaviour : MonoBehaviour {
    [Header("Animations & References")]
    [Space(5)]

    [Tooltip("Indica qual ação que ocorrerá ao interagir com linguagens diferentes")]
    public string[] functionName;
    [Tooltip("Qual animação quem interagiu vai ter")]
    public string animationName;
    [Tooltip("Direção do personagem")]
    public int direction;
    [Tooltip("Personagem referente à alteração, gerado automaticamente por colisão")]
    public GameObject playerRef, shadow;
    public AudioClip[] audios;

    [Space(5)]

    [Tooltip("Objetos ativados/desativados")]
    public GameObject[] activationList, desactivationList, players;

    public bool enabledToUse, playerDestination, disableOnEnd, destroyOnEnd, disableCutsceneOnEnd, disableInteractiOnEnd, sendMessage;


    void Update()
    {
        if (GetComponent<Rigidbody2D>().velocity.y == 0) shadow.SetActive(true); else shadow.SetActive(false);
        if (enabledToUse)
        {
            if (GetComponent<Rigidbody2D>().velocity.y == 0 &&
                Input.GetButtonDown("A") && playerRef.GetComponent<Rigidbody2D>().velocity.x == 0 
                && StatsBehaior.Life[CharacterChangeInformation.atual]<StatsBehaior.publicMaxLife[CharacterChangeInformation.atual])
            {
                GameObject Gc = GameObject.FindGameObjectWithTag("GameController");
                Gc.SendMessage("PlaySoundEffect", audios[0]);
                if (sendMessage)
                {
                    gameObject.SendMessage("SendAMessage");
                }
                if (disableInteractiOnEnd) DisableInteraction(true);
                if (playerDestination)
                {
                    playerRef.SendMessage("EnableCutscene");
                    playerRef.SendMessage("ChangeAtualAnimation", animationName);
                    playerRef.SendMessage("ChangeDirectionX", direction);
                }
                if (disableCutsceneOnEnd) DisableCutscene();
                ActvationObjects();
                DesactivationObjects();
                if (disableOnEnd) enabledToUse = false;
                if (destroyOnEnd) Destroy(gameObject);

            }
        }
    }
    void EnableInteraction(int charId)
    {
        HudGerenciator.functionNameInteraction = functionName[OptionsSaveScript.atualLanguage];
        HudGerenciator.interactiveOn = true;
        enabledToUse = true;
        Debug.Log("interactiveOn");
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
        for (int i = 0; i < activationList.Length; i++)
        {
            activationList[i].SetActive(true);
        }
    }
    void DesactivationObjects()
    {
        for (int i = 0; i < desactivationList.Length; i++)
        {
            desactivationList[i].SetActive(false);
        }
    }
    void DisableCutscene()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SendMessage("DisableCutscenes");
        }
    }

}
