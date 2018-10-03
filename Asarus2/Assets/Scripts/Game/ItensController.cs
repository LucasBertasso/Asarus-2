using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItensController : MonoBehaviour {

    public int atualCoins, coinsAdded;
    public float[] Cloacks, Timers;
    public AudioClip[] audioClips;
    public static int Coins;
    public static int Cristals;
    public TextMeshProUGUI[] valuesTexts;
    public GameObject[] itensHudObjects;
    void Start() {
        atualCoins = Coins;
    }
    void FixedUpdate() {
        CoinsStats();
    }
    void AddCoin(int coins)
    {
        Coins += coins;
        coinsAdded += coins;
        valuesTexts[1].enabled = true;
        valuesTexts[1].text = "+" + (coinsAdded);
    }
    void CoinsStats()
    {
        if (atualCoins != Coins)
        {
            itensHudObjects[0].GetComponent<Animator>().SetBool("Idle", true);
            itensHudObjects[0].GetComponent<Animator>().SetBool("Exit", false);
        }
        if (atualCoins < Coins)
        {
            Cloacks[0] += Time.deltaTime;
            if (Cloacks[0] > Timers[0])
            {
                atualCoins++;
                Cloacks[0] = 0;
            }
        }
        if (atualCoins > Coins)
        {
            Cloacks[0] += Time.deltaTime;
            if (Cloacks[0] > Timers[0])
            {
                atualCoins--;
                Cloacks[0] = 0;
            }
        }
        if (valuesTexts[0].enabled == true)
        {
            valuesTexts[0].text = "" + (atualCoins);
        }
        if (atualCoins == Coins)
        {
            Cloacks[0] += Time.deltaTime;
            if (Cloacks[0] > Timers[1])
            {
                valuesTexts[1].enabled = false;
                coinsAdded = 0;
                itensHudObjects[0].GetComponent<Animator>().SetBool("Exit", true);
                itensHudObjects[0].GetComponent<Animator>().SetBool("Idle", false);
                Cloacks[0] = 0;
            }
        }
    }
}
