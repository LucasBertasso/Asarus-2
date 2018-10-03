using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour {

    [Header("Main Stats Config")]
    [Space(5)]

    [Tooltip("Nível da vida, multiplica-se o valor por 10")]
    public int lifeLevel;
    [Tooltip("Nível da stamina, multiplica-se o valor por 10")]
    public int staminaLevel;
    [Tooltip("Nível da mana, multiplica-se o valor por 10")]
    public int manaLevel;

}
