using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss00Behaviour : EnemyStats {
    [Header("Animation Tests")]
    public bool visible;
    public bool Roar, Moving, attacking1;
    public GameObject bullet;
    public GameObject[] spawners;
    public AudioSource Music;
    float c,a;
    int ii;
    public float attacktimer;
    void Start () {
		
	}
	void FixedUpdate () {
        Vector2 scale = transform.localScale;
        if (scale.x > 0 && transform.position.x < PlayerRefs[CharacterChangeInformation.atual].transform.position.x && !attack)
        {
            scale.x *= -1;
            transform.localScale = scale;
        }
        if (scale.x < 0 && transform.position.x > PlayerRefs[CharacterChangeInformation.atual].transform.position.x && !attack)
        {
            scale.x *= -1;
            transform.localScale = scale;
        }
        HitUpdate();
        LifeFill();
        ShadowControl();
        Death();
        WalkESpeed();
        if(Music.enabled == false)
        {
            idle = false;
            c += Time.deltaTime;
            if(c >= 5)
            {
                animator.SetBool("Idle", true);
                attack = true;
                Music.enabled = true;
            }
        }
        if(render.enabled == false)
        {
            directionX = Random.Range(-1,1);
        }
        if (animator.GetBool("Attack0") == true)
        {
            Cloacks[4] += Time.deltaTime;

            if (Cloacks[4] >= 5 && !attacking1)
            {
                animator.SetBool("Idle", true);
                animator.SetBool("Attack0", false);
                Cloacks[4] = 0;
            }
        }
        if (animator.GetBool("Attack1") == true)
        {
            Cloacks[4] += Time.deltaTime;

            if (Cloacks[4] >= 1)
            {
                Vector3 vec = new Vector3(PlayerRefs[CharacterChangeInformation.atual].transform.position.x, transform.position.y + 0.5f, 0);
                Instantiate(bullet, vec,transform.rotation);
                a++;
                Cloacks[4] = 0;
            }
            if (a == 3)
            {
                animator.SetBool("Idle", true);
                animator.SetBool("Attack1", false);
                a = 0;
            }
        }
        if (animator.GetBool("Idle") == true)
        {
            attack = false;
            c += Time.deltaTime;
            if (c >= attacktimer)
            {
                animator.SetBool("Idle", false);
                 ii = Random.Range(0, 3);
                if(ii == 0)
                {
                    attack = true;
                    animator.SetBool("Attack0", true);
                }
                else
                {
                    if (ii == 1)
                    {
                        attack = true;
                        animator.SetBool("Attack1", true);
                    }
                    else
                    {
                        if (ii == 2)
                        {
                            animator.SetBool("Location", true);
                        }
                    }
                }
                c = 0;
            }
        }
        if (animator.GetBool("Location") == true)
        {
            c += Time.deltaTime;
            if (c >= 3)
            {
                directionX = Random.Range(-1, 2);
                if (!Moving)
                {
                    Moving = true;
                    directionX = -1;
                    c = 0;
                }
                directionX = Random.Range(-1, 2);
                if(directionX == 0 && Moving)
                {
                    Moving = false;
                    animator.SetBool("Location", false);
                    animator.SetBool("Apear", true);
                    c = 0;
                }
                else c = 0;
            }
        }
        if (animator.GetBool("Apear") == true)
        {
            c += Time.deltaTime;
            if (c >= 3)
            {
                animator.SetBool("Apear", false);
                animator.SetBool("Idle", true);
                c = 0;
            }
        }
        if(life <= maxLife / 2)
        {
            attacktimer = 1;
        }

    }
}
