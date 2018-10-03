using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractiveButtonInform : MonoBehaviour {

    [Tooltip("Indica quais objetos da interface de interação")]
    public GameObject[] Buttons;
    [Tooltip("Indica quais players atuais")]
    public GameObject[] Players;
    [Tooltip("Indica qual posição acimado player em y a interface dos botões fica")]
    public int[] positionYArrumator;
    [Tooltip("Indica qual texto que é alterado para passar informação da ação")]
    public TextMeshProUGUI functionText;

    private void Update()
    {
        if (HudGerenciator.interactiveOn)
        {
            EnableButtons();
        }
        else DisableButtons();
        transform.position = (new Vector2(Players[CharacterChangeInformation.atual].transform.position.x, 
                                          Players[CharacterChangeInformation.atual].transform.position.y +
                                          positionYArrumator[CharacterChangeInformation.atual]));
        functionText.text = HudGerenciator.functionNameInteraction;
    }
    void EnableButtons()
    {
        if(OptionsSaveScript.atualControl == 0)
        {
            Buttons[0].SetActive(true);
        } else
        {
            Buttons[1].SetActive(true);
        }
    }
    void DisableButtons()
    {
        for(int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].SetActive(false);
        }
    }
}
