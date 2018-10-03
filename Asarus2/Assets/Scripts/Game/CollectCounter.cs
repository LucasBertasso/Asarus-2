using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CollectCounter : MonoBehaviour {
    public int atualCollect, maxCollect;
    public TextMeshProUGUI collectText;
    public float transitionTime = 0.5f;
    public string nextScene;
    void Update()
    {
        collectText.text = atualCollect + "/" + maxCollect;
        if(atualCollect == maxCollect)
        {
            StartCoroutine(changeLevel());
        }
    }
    void AddCollect()
    {
        atualCollect++;
    }
    IEnumerator changeLevel()
    {
        float fadeTime = GameObject.FindGameObjectWithTag("GameController").GetComponent<FadeScript>().BeginFade(1);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(nextScene);
    }
}
