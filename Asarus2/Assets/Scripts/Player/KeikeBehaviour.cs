using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class KeikeBehaviour : MonoBehaviour {
    public GameObject magic;
    public GameObject magicSpawner;
    public Animator Animator;
    public float[] Cloacks, Timers, cameraShakes;
    public int manaLoss;
    public int damageAir, maxDamage;
    public static int damage;
    public bool attacking,airAttacking,canAttack,charging, attacked;
    public AudioClip[] audios;
	
	// Update is called once per frame
	void Update () {
        AnimationControl();
        if (Input.GetButton("X"))
        {
            gameObject.SendMessage("AttackKeikeReciver");
        }
        if (Input.GetButtonDown("X") && !airAttacking)
        {
            gameObject.SendMessage("AttackKeikeReciver2");
        }
        if (Input.GetButtonUp("X"))
        {
            if(Cloacks[0] > Timers[0] && !airAttacking)
            {
                damage = maxDamage;
                attacking = true;
                charging = false;
            }
            else
            {
                if (charging && !airAttacking)
                {
                    gameObject.SendMessage("DisableAttack");
                    gameObject.SendMessage("EnableActions");
                }
                if (charging)
                {
                    charging = false;
                }
            }
            Cloacks[0] = 0;
            Cloacks[1] = 0;
            Cloacks[2] = 0;
        }
    }
    void GroundAttack()
    {
        if (Input.GetButton("X"))
        {
            Cloacks[0] += Time.deltaTime;
            charging = true;
            Cloacks[4] += Time.deltaTime;
            if (Cloacks[4] > 0.5f)
            {
                gameObject.SendMessage("PlayAudio", audios[1]);
                Cloacks[4] = 0;
            }
        }
    }
    void AirAttack()
    {
        if (!airAttacking && !attacking)
        {
            canAttack = true;
            airAttacking = true;
            Cloacks[1] = 0;
            Cloacks[3] = 0;
        }
    }
    void AnimationControl()
    {
        if (attacking && !airAttacking)
        {
            Cloacks[3] += Time.deltaTime;
            if (Cloacks[3] >= Timers[3])
            {
                attacked = false;
                attacking = false;
                gameObject.SendMessage("DisableAttack");
                gameObject.SendMessage("EnableActions");
                Cloacks[3] = 0;
            }
            if (Animator.GetBool("Attack1") == false)
            {
                gameObject.SendMessage("PlayAudio", audios[0]);
                GameObject Cam = GameObject.FindGameObjectWithTag("MainCamera");
                Cam.SendMessage("ShakeCamera", cameraShakes[0]);
            }
            Animator.SetBool("Attack1", true);
            if (!attacked)
            {
                Cloacks[1] += Time.deltaTime;
            }
            if (Cloacks[1] >= Timers[1])
            {
                attacked = true;
                Instantiate(magic, magicSpawner.transform.position, magicSpawner.transform.rotation);
                Cloacks[1] = 0;
                StatsBehaior.Mana[CharacterChangeInformation.atual] -= manaLoss;
                GamePad.SetVibration(PlayerIndex.One, 0.2f, 0.2f);
                StatsBehaior.Stamina[CharacterChangeInformation.atual] -= manaLoss;
            }
        }
        else
        {
            if (Animator.GetBool("Attack1") == true)
            {
                Animator.SetBool("Attack1", false);
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
            }
        }
        if (charging)
        {
            GameObject Cam = GameObject.FindGameObjectWithTag("MainCamera");
            Cam.SendMessage("ShakeCamera", cameraShakes[0]);
            GamePad.SetVibration(PlayerIndex.One, Cloacks[0], Cloacks[0]);
            Animator.SetBool("Charge", true);
            Cloacks[1] += Time.deltaTime;
        }
        else
        {
            if (Animator.GetBool("Charge") == true)
            {
                Animator.SetBool("Charge", false);
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
            }
        }
        if (airAttacking && !attacking)
        {
            if (!attacked)
            {
                Cloacks[1] += Time.deltaTime;
            }
            Cloacks[3] += Time.deltaTime;
            if (Cloacks[3] >= Timers[3])
            {
                Cloacks[3] = 0;
                canAttack = true;
                airAttacking = false;
            }
            if (Animator.GetBool("Attack2") == false)
            {
                gameObject.SendMessage("PlayAudio", audios[0]);
                Animator.SetBool("Attack2", true);
                GameObject Cam = GameObject.FindGameObjectWithTag("MainCamera");
                Cam.SendMessage("ShakeCamera", cameraShakes[0]);
            }
            if (canAttack)
            {
                if (Cloacks[1] >= Timers[1])
                {
                    attacked = true;
                    Instantiate(magic, magicSpawner.transform.position, magicSpawner.transform.rotation);
                    Cloacks[1] = 0;
                    StatsBehaior.Mana[CharacterChangeInformation.atual] -= manaLoss;
                    StatsBehaior.Stamina[CharacterChangeInformation.atual] -= manaLoss;
                    damage = damageAir;
                    GamePad.SetVibration(PlayerIndex.One, 0.2f, 0.2f);
                    canAttack = false;
                }
            }
        }
        else
        {
            if (Animator.GetBool("Attack2") == true)
            {
                attacked = false;
                gameObject.SendMessage("DisableAttack");
                gameObject.SendMessage("EnableActions");
                Animator.SetBool("Attack2", false);
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
            }
        }
    }
}
