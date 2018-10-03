using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralaxing : MonoBehaviour {

    public Transform [] backgrounds;         // Lista de todos os backgrounds a serem movimentados

    float[] paralaxScales;                  // Lista a dimensão dos objetos a serem movimentados em paralax

    public float smoothing = 1f;            // Suavisa a movimentação

    Transform cam;                          // referência a câmera

    Vector3 prevousCamPosition;             // calcula o paralax

    private void Awake()
    {
        // recebe a ref da camera
        cam = Camera.main.transform;
    }

    // Use this for initialization
    void Start () {
        // Frame anterior tem a posição anterior da camera
        prevousCamPosition = cam.position;
        // Cria a lista de paralax igual ao numero de backgrounds
        paralaxScales = new float[backgrounds.Length];
        // Transforma a posição do background relativo ao paralax inicial
        for(int i = 0; i < backgrounds.Length; i++)
        {
            paralaxScales[i] = backgrounds[i].position.z*-1;
        }
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // recebe a posição anterior e a posterior e multiplica pela esca do paralax
            float parallax = (prevousCamPosition.x - cam.position.x) * paralaxScales[i];

            // recebe a posição do background e soma com o paralax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            // cria uma posição alvo que é a posição atual do bkg com a posição x alvo
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            // muda a posição atual para a alvo com lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);

            // muda a posição da camera 
            prevousCamPosition = cam.position;
        }

    }
}
