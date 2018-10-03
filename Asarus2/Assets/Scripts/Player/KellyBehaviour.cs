using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class KellyBehaviour : MonoBehaviour {
    public Animator Animator;
    public AudioClip[] audios;
    [Tooltip("Stamina gasta para executar ataque")]
    public int[] attackStaminaLoss;
    [Tooltip("Timers para resetar ataques")]
    public float[] timers, cloacks, cameraShakes, staminaLoss;
    public GameObject[] attackBoxes;
    public bool canAttack3 = true;
    [SerializeField]
    bool attacking, airAttacking,charging, atk0,atk1,atk2;
    int id;
    private void Update()
    {
        AttackMananger();
        AttackAnimations();
    }
    void AttackMananger()
    {
        if (Input.GetButtonDown("X"))
        {
            SendMessage("AttackKellyReciver");
        }
        if (Input.GetButtonUp("X"))
        {
            if (cloacks[3] >= timers[3])
            {
                atk2 = true;
                charging = false;
                cloacks[3] = 0;
            }
            if (charging && !atk2)
            {
                charging = false;
                gameObject.SendMessage("DisableAttack");
                gameObject.SendMessage("EnableActions");
                attacking = false;
                airAttacking = false;
            }
            cloacks[4] = 0;
        }
        if (Input.GetButton("X") && canAttack3)
        {
                SendMessage("AttackKellyReciver2");
        }
        // Cloacks
        if (atk0)
        {
            attacking = true;
            cloacks[0] += Time.deltaTime;
            if (cloacks[0] >= timers[0])
            {
                atk0 = false;
            }
        }
        else
        {
           cloacks[0] = 0;
        }
        if (atk1 && !atk0)
        {
            attacking = true;
            cloacks[1] += Time.deltaTime;
            if (cloacks[1] >= timers[1])
            {
                atk1 = false;
            }
        }
        else
        {
            cloacks[1] = 0;
        }
        if (atk2 && !atk1)
        {
            attacking = true;
            cloacks[2] += Time.deltaTime;
            if (cloacks[2] >= timers[2])
            {
                atk2 = false;
            }
        }
        else
        {
            cloacks[2] = 0;
        }
        // Attackings
        if (!charging && !atk0 && !atk1 && !atk2 && (attacking || airAttacking))
        {
            gameObject.SendMessage("DisableAttack");
            gameObject.SendMessage("EnableActions");
            attacking = false;
            airAttacking = false;
        }
        if (!attacking && !airAttacking)
        {
            for (int i = 0; i < attackBoxes.Length; i++)
            {
                attackBoxes[i].SetActive(false);
            }
        }
    }
    void GroundAttack(int id2)
    {
        id = id2;
        if(!atk0 && !atk1 && !atk2)
        {
            atk0 = true;

        }
        else
        {
            if(!atk1 && !atk2)
            {
                atk1 = true;

            }
        }
    }
    void AttackCharge()
    {
        if (Input.GetButton("X") && canAttack3)
        {
            cloacks[4] += Time.deltaTime;
            if (cloacks[4] >= timers[0])
            {
                charging = true;
            }
            cloacks[5] += Time.deltaTime;
            if(cloacks[5] > 0.3f)
            {
                gameObject.SendMessage("PlayAudio", audios[3]);
                cloacks[5] = 0;
            }
        }
        if (charging)
        {
            if (Input.GetButton("X"))
            {
                GamePad.SetVibration(PlayerIndex.One, cloacks[3]/2, cloacks[3]/2);
                cloacks[3] += Time.deltaTime;

            }
        } else
        {
            cloacks[3] = 0;
        }
    }
    void AttackAnimations()
    {
        if (atk0)
        {
            if (Animator.GetBool("Attack1") == false)
            {
                gameObject.SendMessage("PlayAudio", audios[0]);
                GameObject Cam = GameObject.FindGameObjectWithTag("MainCamera");
                Cam.SendMessage("ShakeCamera", cameraShakes[0]);
                StatsBehaior.Stamina[id] -= (int)staminaLoss[0];
            }
            Animator.SetBool("Attack1", true);
            GamePad.SetVibration(PlayerIndex.One, 0.2f, 0.2f);
        }
        else
        {
            if (Animator.GetBool("Attack1") == true)
            {
                Animator.SetBool("Attack1", false);
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
            }
        }
        if (atk1 && !atk0)
        {
            if (Animator.GetBool("Attack2") == false)
            {
                gameObject.SendMessage("PlayAudio", audios[1]);
                GameObject Cam = GameObject.FindGameObjectWithTag("MainCamera");
                Cam.SendMessage("ShakeCamera", cameraShakes[1]);
                StatsBehaior.Stamina[id] -= (int)staminaLoss[1];
            }
            Animator.SetBool("Attack2", true);
            GamePad.SetVibration(PlayerIndex.One, 0.3f, 0.3f);
        }
        else
        {
            if (Animator.GetBool("Attack2") == true)
            {
                gameObject.SendMessage("DisableAttack");
                Animator.SetBool("Attack2", false);
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
            }
        }
        if (atk2 && !atk1)
        {
            if (Animator.GetBool("Attack3") == false)
            {
                gameObject.SendMessage("PlayAudio", audios[2]);
                GameObject Cam = GameObject.FindGameObjectWithTag("MainCamera");
                Cam.SendMessage("ShakeCamera", cameraShakes[2]);
                StatsBehaior.Stamina[id] -= (int)staminaLoss[2];
                StatsBehaior.Mana[id] -= (int)staminaLoss[3];
            }
            Animator.SetBool("Attack3", true);
            GamePad.SetVibration(PlayerIndex.One, 1, 1);
        }
        else
        {
            if (Animator.GetBool("Attack3") == true)
            {
                Animator.SetBool("Attack3", false);
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
            }
        }
        if (charging)
        {
            GameObject Cam = GameObject.FindGameObjectWithTag("MainCamera");
            Cam.SendMessage("ShakeCamera", 0.1f);
            Animator.SetBool("Attack3 Charge", true);
        }
        else
        {
            if (Animator.GetBool("Attack3 Charge") == true)
            {
                Animator.SetBool("Attack3 Charge", false);
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
            }
        }
    }
    void AirAttack(int id2)
    {
        id = id2;
        atk1 = true;
        StatsBehaior.Stamina[id] -= 20;
        airAttacking = true;
    }
}
