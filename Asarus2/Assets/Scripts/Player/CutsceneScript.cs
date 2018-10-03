using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneScript : MonoBehaviour {

    public Animator Animator;
    public string[] animationNames;
    public string atualAnimation;

    public bool enabledCutsceneFunctions;

    private void Update()
    {
        if (enabledCutsceneFunctions)
        {
            Cutscene();
        }
    }
    void Cutscene()
    {
        Animation();
    }
    void Animation()
    {
        for(int i = 0; i < animationNames.Length; i++)
        {
            if (animationNames[i] != atualAnimation)
            {
                Animator.SetBool(animationNames[i], false);
            } else
            {
                Animator.SetBool(animationNames[i], true);
            }
        }
    }
    void ChangeAtualAnimation(string name)
    {
        atualAnimation = name;
    }
    void EnableCutsceneNow()
    {
        enabledCutsceneFunctions = true;
    }
    void DisableCutsceneNow()
    {
        enabledCutsceneFunctions = false;
    }
}
