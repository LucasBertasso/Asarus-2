using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScript : MonoBehaviour {
    public Texture2D texture;
    public float timeFade;

    int fadeDir = -1,
        drawDeaph = -1000;
    float Alpha = 1.0f;

    private void OnGUI()
    {
        Alpha += fadeDir * timeFade * Time.deltaTime;
        Alpha = Mathf.Clamp01(Alpha);
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, Alpha);
        GUI.depth = drawDeaph;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
    }

    public float BeginFade (int direction)
    {
        fadeDir = direction;
        return (timeFade);
    }
    private void OnLevelWasLoaded(int level)
    {
        BeginFade(-1);
    }
}
