using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tingaru : EnemyStats {
	void Start () {
        GetRefs();
        SetInitialValues();
        Timers[0] = Random.Range(1, 5);
        atualState = 0;
        
	}
	
	void FixedUpdate () {
        EssencialsVoids();
        ShadowControl();
        // Specific Commands
        if(atualState != 0)
        {
            if (!canAttack && grounded && !attack)
            {
                directionX = transform.localScale.x;
            }
            else directionX = 0;
        }
        // Cloacks 0 = Random move timer / Timer 0
        Patrol();
        // Cloacks 2 = Attack random move timer /  Timer 1
        if (canAttack && !hiting && !attack && !waitingMode)
        {
            Cloacks[2] += Time.deltaTime;
            if (Cloacks[2] >= Timers[1])
            {
                atualAction = Random.Range(0, 4);
                if(atualAction != 3)
                {
                    attack = true;
                }
                Cloacks[2] = 0;
            }
        }
        // Cloacks 4 = Tempo para sair do modo de ataque / Timer 2
        if (attack)
        {
            Cloacks[3] += Time.deltaTime;
            if (Cloacks[3] >= Timers[3])
            {
                audioSource.PlayOneShot(Audios[2]);
                GameObject Cam = GameObject.FindGameObjectWithTag("MainCamera");
                Cam.SendMessage("ShakeCamera", 0.02f);
                Cloacks[3] = 0;
            }
            // Cloacks 3 = Tempo para dar shake na camera / Timer 3
            Cloacks[4] += Time.deltaTime;
            if(Cloacks[4] >= Timers[2])
            {
                attack = false;
                Cloacks[4] = 0;
                Cloacks[3] = 0;
            }
        }
       
	}


}
