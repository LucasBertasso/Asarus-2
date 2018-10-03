using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsSaveScript : MonoBehaviour {

    [Tooltip("Indica qual configuração de controle atual: keyboard,joystick")]
    public int control;
    [Tooltip("Indica qual linguagem atual")]
    public int language;
    public static int atualLanguage = 0;
    public static int atualControl = 0;

	void Start () {
        LoadOptions();
	}
    void LoadOptions()
    {
        control = PlayerPrefs.GetInt("ControlOprions");
        language = PlayerPrefs.GetInt("Language");
        atualControl = control;
        atualLanguage = language;
    }
    void SaveOptions()
    {
        PlayerPrefs.SetInt("ControlOptions", control);
       PlayerPrefs.SetInt("Language", language);
    }
}
