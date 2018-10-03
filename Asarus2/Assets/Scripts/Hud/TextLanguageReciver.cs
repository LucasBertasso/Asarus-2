using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextLanguageReciver : MonoBehaviour {
    [TextArea(2, 3)]
    public string[] Textos;
    private void Update()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.text = Textos[OptionsSaveScript.atualLanguage];
    }
}
