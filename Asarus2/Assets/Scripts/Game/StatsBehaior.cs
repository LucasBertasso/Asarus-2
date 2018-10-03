using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatsBehaior : MonoBehaviour {

    [Header("Ininital Configurations")]

    [Space(5)]
    [Tooltip("Personagem atual")]
    public int idAtual;
    [Tooltip("Personagem referente")]
    public string[] charName;
    [Tooltip("Vida máxima referente")]
    public int[] maxLife;
    [Tooltip("Stamina máxima referente")]
    public int[] maxStamina;
    [Tooltip("Mana máxima referente")]
    public int[] maxMana;
    [Tooltip("Vida inicial referente")]
    public int[] startLife;
    [Tooltip("Stamina inicial referente")]
    public int[] startStamina;
    [Tooltip("Mana inicial referente")]
    public int[] startMana;
    [Tooltip("Provem a maxima quantia ds status no inicio da cena, se desmarcado os status anteriores são obtidos")]
    public bool maxInitialStatus = true;
    public GameObject[] players;

    [Header("Text references")]

    [Space(5)]
    [Tooltip("Nome do personagem atual")]
    public TextMeshProUGUI nameText;
    [Tooltip("Textos para contagem")]
    public TextMeshProUGUI lifeText;
    [Tooltip("Textos para contagem")]
    public TextMeshProUGUI staminaText, manaText;

    [Header("Update Values")]

    [Space(5)]
    [Tooltip("Vida atual referente")]
    public int[] atualLife;
    [Tooltip("Stamina atual referente")]
    public int[] atualStamina;
    [Tooltip("Mana atual referente")]
    public int[] atualMana;

    [Header("Image References")]

    [Space(5)]
    [Tooltip("Image fill")]
    public Image[] lifeFill;
    [Tooltip("Image fill")]
    public Image[] staminaFill;
    [Tooltip("Image fill")]
    public Image[] manaFill;

    [Header("Recover details")]

    [Space(5)]
    [Tooltip("Velocidade de recuperação")]
    public float staminaSpeed;
    public float recoverStaminaTime;
    [Tooltip("Velocidade de recuperação")]
    public float manaSpeed;

    // Static values
    public static int[] Life = new int [2], Stamina = new int[2], Mana = new int[2], publicMaxLife = new int [2];
    float staminaTimer, staminaReset, manaTimer, manaReset;
    float amount;

    void Start () {
        StatsLoad();
    }
	
	void FixedUpdate () {
        if(Life[idAtual] <= 0)
        {
            players[idAtual].SendMessage("Death");
        }
        // Update text
        TextStats();
        FillStats();
        // Update values
        StatsBehavior();
        atualLife[idAtual] = Life[idAtual];
        atualStamina[idAtual] = Stamina[idAtual];
        atualMana[idAtual] = Mana[idAtual];
        // Debug Functions
        if (Input.GetKeyDown(KeyCode.L))
        {
            SendMessage("SaveGame");
        }
	}
    void TextStats()
    {
        nameText.text = charName[idAtual];
        lifeText.text = atualLife[idAtual] + " / " + maxLife[idAtual];
        staminaText.text = atualStamina[idAtual] + " / " + maxStamina[idAtual];
        manaText.text = atualMana[idAtual] + " / " + maxMana[idAtual];
    }
    void FillStats()
    {
        // Life
        amount = (float)Life[idAtual] / (float)maxLife[idAtual];
        if (amount <= lifeFill[0].fillAmount) lifeFill[0].fillAmount = amount; else lifeFill[0].fillAmount += 0.5f * Time.deltaTime;

        if (amount < lifeFill[1].fillAmount) lifeFill[1].fillAmount -= 1f * Time.deltaTime;
        else lifeFill[1].fillAmount = amount;
        // Stamina
        staminaFill[0].fillAmount = (float)Stamina[idAtual] / (float)maxStamina[idAtual];
        if (staminaFill[0].fillAmount < staminaFill[1].fillAmount) staminaFill[1].fillAmount -= 1f * Time.deltaTime;
        else staminaFill[1].fillAmount = staminaFill[0].fillAmount;
        // Mana
        manaFill[0].fillAmount = (float)Mana[idAtual] / (float)maxMana[idAtual];
        if (manaFill[0].fillAmount < manaFill[1].fillAmount) manaFill[1].fillAmount -= 1f * Time.deltaTime;
        else manaFill[1].fillAmount = manaFill[0].fillAmount;
    }
    void StatsBehavior()
    {
        // Life
        for (int ii = 0; ii < Life.Length; ii++)
        {
            if (Life[ii] > maxLife[ii]) Life[ii] = maxLife[ii];
            if (Life[ii] < 0) Life[ii] = 0;
        }
        // Stamina
        for (int ii = 0; ii < Stamina.Length; ii++)
        {
            if (Stamina[ii] < maxStamina[ii]) staminaTimer += Time.deltaTime;
            if (staminaTimer >= staminaSpeed && Stamina[0] >= 1 && Stamina[1] >= 1 && Stamina[ii] < maxStamina[ii])
            {
                Stamina[ii] += 1;
                staminaTimer = 0;
            }
            if (Stamina[ii] < 0) Stamina[ii] = 0;
            if (Stamina[ii] == 0 && staminaTimer > recoverStaminaTime) Stamina[ii] = 1;
        }
        // Mana
        for (int ii = 0; ii < Mana.Length; ii++)
        {
            if (Mana[ii] < maxMana[ii]) manaTimer += Time.deltaTime;
            if (manaTimer >= manaSpeed && Mana[ii] >= 1 && Mana[ii] < maxMana[ii])
            {
                Mana[ii] += 1;
               manaTimer = 0;
            }
            if (Mana[ii] < 0) Mana[ii] = 0;
            if (Mana[ii] == 0 && manaTimer > 4) Mana[ii] = 1;
        }
    }
    void IdChange(int newId)
    {
        idAtual = newId;
    }
    void SetAtualValuesToBars()
    {
        lifeFill[1].fillAmount = lifeFill[0].fillAmount = amount;
        staminaFill[1].fillAmount = staminaFill[0].fillAmount;
        manaFill[1].fillAmount = manaFill[0].fillAmount;
    }
    void AddLife(int health)
    {
        if (Life[idAtual] < maxLife[idAtual])
        {
            Life[idAtual] += health;
        }
    }
    void StatsLoad()
    {
        // Max Stats Load
        for (int i = 0; i < maxLife.Length; i++)
        {
            maxLife[i] += SaveGameScript.lifeLevelReciver * 10;
            publicMaxLife[i] = maxLife[i];
        }
        for (int i = 0; i < maxLife.Length; i++)
        {
            maxStamina[i] += SaveGameScript.staminaLevelReciver * 10;
        }
        for (int i = 0; i < maxLife.Length; i++)
        {
            maxMana[i] += SaveGameScript.manaLevelReciver * 10;
        }
        // Atual Stats Load
        if (maxInitialStatus)
        {
            for (int i = 0; i < atualLife.Length; i++)
            {
                atualLife[i] = maxLife[i];
                Life[i] = maxLife[i];
            }
            for (int i = 0; i < atualStamina.Length; i++)
            {
                atualStamina[i] = maxStamina[i];
                Stamina[i] = maxStamina[i];
            }
            for (int i = 0; i < atualMana.Length; i++)
            {
                atualMana[i] = maxMana[i];
                Mana[i] = maxMana[i];
            }
        }
    }
}
