using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{

    [Header("Configurações")]
    [Space(5)]
    [Tooltip("Quantidade de vida")]
    public float maxLife;
    public int life;
    [Tooltip("Quantidade de vida")]
    public Image lifeFill;
    [Tooltip("Quantidade de vida")]
    public GameObject lifeFillObject;
    [Tooltip("escala de life fill")]
    public float lifeScale;
    [Tooltip("Dano causado")]
    public int Damage;
    [Tooltip("Dano causado")]
    public float attackRange;
    [Tooltip("Velociadades de andar e correr")]
    public float[] speeds = new float[2];
    [Tooltip("Temporizadores")]
    public float[] Timers = new float[5];
    [Tooltip("Referencia dos efeitos")]
    public GameObject[] Effects;
    [Tooltip("Referencia dos efeitos de dano")]
    public GameObject[] HitEffects;
    [Tooltip("Referencia os efeitos sonoros")]
    public AudioClip[] Audios;
    [Tooltip("Hit Boxes")]
    public GameObject[] HitBoxes;
    [Tooltip("Referência aos players")]
    public GameObject[] PlayerRefs;
    [Tooltip("Referência a sombra")]
    public GameObject Shadow;
    [Tooltip("Referência ao animator das sprites")]
    public Animator animator;
    [Tooltip("Referência ao renderer das sprites")]
    public Renderer render;
    [Tooltip("Referência aos materiais das sprites")]
    public Material[] materials;
    [Tooltip("Posição de respaw")]
    public Vector3 initialPosition;
    [Tooltip("Verifica o chão")]
    public Transform groundCheck;
    float groundRadius = 0.2f;
    [Tooltip("Que layes são consideradas Ground")]
    public LayerMask whatIsGround;
    Vector3 LastPosition;
    [Tooltip("0-1 = X, 2-3 = Y")]
    public float[] maxEMin;
[Space(5)]

    [Header("Random Behaviour")]
    [Space(5)]
    [Tooltip("Numeros aleatorios)")]
    public int[] randomNumber;

    [Space(5)]

    [Header("Instantiate Behaviour")]
    [Space(5)]
    [Tooltip("Objeto a ser criado)")]
    public GameObject[] deathObject;

    [Header("Debug")]
    [Space(5)]
    [Tooltip("Estado atual das funcionalidades")]
    public int atualState;
    [Tooltip("Ação a ser executada")]
    public int atualAction;
    [Tooltip("Relógios")]
    public float[] Cloacks = new float[5];
    [Tooltip("Relógios")]
    public float hitCloack;

    // Privates
    public float timeToBackToNormal, directionX, directionY;
    public bool
         grounded,
         idle,
         walk,
         attack,
         hit,
         hiting,
         death;
    [Space(5)]
    public bool
         canAttack,
         waitingMode,
         attackMode,
         invunerableMode,
         touchDamageMode;

    public Rigidbody2D rig;
    public AudioSource audioSource;

    public void EssencialsVoids()
    {
        WaitingModeControl();
        AnimationControl();
        ActionsControl();
        if (!attackMode)
        {
            LookControl();
        }
        else LookControlAttack();
        WalkESpeed();
        GroundCheck();
        CanAttack();
        HitUpdate();
        Death();
        LifeFill();
    }
    public void SetInitialValues()
    {
        initialPosition = transform.position;
        transform.position = initialPosition;
        life = (int)maxLife;
    }
    public void WalkESpeed()
    {
        rig.velocity = new Vector2(directionX * speeds[atualState], rig.velocity.y);
    }
    public void GetRefs()
    {
        rig = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }
    public void TakeDamage(int damagem)
    {
        life -= damagem;
    }
    public void Death()
    {
        if (life <= 0)
        {
            GameObject Gc = GameObject.FindGameObjectWithTag("GameController");
            Gc.SendMessage("PlaySoundEffect", Audios[1]);
            DeathSpawn();
            gameObject.SetActive(false);
        }
    }
    public void AnimationControl()
    {
        if (!attackMode)
        {
            if (idle)
            {
                animator.SetBool("Idle", true);
                animator.SetBool("Attack Idle", false);
            }
            else animator.SetBool("Idle", false);
            if (walk)
            {
                animator.SetBool("Walk", true);
                animator.SetBool("Attack Walk", false);
            }
            else animator.SetBool("Walk", false);
        }
        else
        {
            if (idle)
            {
                animator.SetBool("Attack Idle", true);
                animator.SetBool("Idle", false);
            }
            else animator.SetBool("Attack Idle", false);
            if (walk)
            {
                animator.SetBool("Attack Walk", true);
                animator.SetBool("Walk", false);
            }
            else animator.SetBool("Attack Walk", false);
        }
        if (hit)
        {
            animator.SetBool("Hit", true);
        }
        else animator.SetBool("Hit", false);
        if (death)
        {
            animator.SetBool("Death", true);
        }
        else animator.SetBool("Death", false);
        if (attack)
        {
            animator.SetBool("Attack", true);
        }
        else animator.SetBool("Attack", false);

    }
    public void ChangeToAttackMode()
    {
        waitingMode = false;
        attackMode = true;
        atualState = 1;
    }
    public void ChangeToNormalMode()
    {
        waitingMode = true;
    }
    public void ActionsControl()
    {
        if (rig.velocity.x != 0 && grounded && !attack && !hit && !death)
        {
            walk = true;
        }
        else walk = false;
        if (rig.velocity.x == 0 && rig.velocity.y == 0 && !attack && !hit && !death)
        {
            idle = true;
        }
        else idle = false;
    }
    public void LookControl()
    {
        Vector2 scale = transform.localScale;
        if (directionX > 0 && scale.x < 0 && !attack)
        {
            scale.x *= -1;
            transform.localScale = scale;
        }
        if (directionX < 0 && scale.x > 0 && !attack)
        {
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
    public void LookControlAttack()
    {
        Vector2 scale = transform.localScale;
        if (scale.x > 0 && transform.position.x > PlayerRefs[CharacterChangeInformation.atual].transform.position.x && !attack)
        {
            scale.x *= -1;
            transform.localScale = scale;
        }
        if (scale.x < 0 && transform.position.x < PlayerRefs[CharacterChangeInformation.atual].transform.position.x && !attack)
        {
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
    public void WaitingModeControl()
    {
        if (waitingMode)
        {
            Cloacks[1] += Time.deltaTime;
            if (Cloacks[1] >= timeToBackToNormal)
            {
                atualState = 0;
                Cloacks[1] = 0;
                waitingMode = false;
                attackMode = false;
            }
        } else Cloacks[1] = 0;
    }
    public void CanAttack()
    {
        if (attackMode &&
            PlayerRefs[CharacterChangeInformation.atual].transform.position.x < transform.position.x + maxEMin[0] &&
            PlayerRefs[CharacterChangeInformation.atual].transform.position.x > transform.position.x - maxEMin[0])
        {
            canAttack = true;
        } else canAttack = false;
    }
    public void GroundCheck()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        if(!grounded && directionX == transform.localScale.x)
        {
            directionX = 0;
            idle = true;
            walk = false;
        }
    }
    public void Hit(int dmg)
    {
        if (!hit && !hiting)
        {
            audioSource.PlayOneShot(Audios[0]);
            Instantiate(HitEffects[Random.Range(0, HitEffects.Length)], transform.position, transform.rotation);
            attackMode = true;
            life -= dmg;
            hit = true;
            hiting = true;
            if (!attack)
            {
                render.material = materials[1];
            }
            else
            {
                render.material = materials[1];
            }
        }
    }
    public void HitUpdate()
    {
        if (hit)
        {
            hitCloack += Time.deltaTime;
            if(hitCloack >= 0.1f)
            {
                render.material = materials[0];
            }
            if (hitCloack >= 0.4f)
            {
                hitCloack = 0;
                hit = false;
                hiting = false;
            }
        }
    }
    public void LifeFill()
    {
        lifeFill.fillAmount = life / maxLife;
        Vector2 scaleX = lifeFillObject.transform.localScale;
        if (transform.localScale.x < 0 && lifeFillObject.transform.localScale.x > 0)
        {
            scaleX *= -1;
            lifeFillObject.transform.localScale = scaleX;
        }
        else
        {
            if (transform.localScale.x > 0 && lifeFillObject.transform.localScale.x < 0)
            {
                scaleX *= -1;
                lifeFillObject.transform.localScale = scaleX;
            }
        }
    }
    public void Patrol()
    {
        Cloacks[0] += Time.deltaTime;
        if (Cloacks[0] >= Timers[0])
        {
            if (!attackMode)
            {
                directionX = Random.Range(-1, 2);
                Timers[0] = Random.Range(2, 5);
            }
            Cloacks[0] = 0;
        }
    }
    public void ShadowControl()
    {
        if (rig.velocity.y != 0)
        {
            Shadow.SetActive(false);
        }
        else Shadow.SetActive(true);
    }
    public void DeathSpawn()
    {
        if (deathObject.Length > 0)
        {
            for (int i = 0; i < deathObject.Length; i++)
            {
                Instantiate(deathObject[i], transform.position, transform.rotation);
            }
        }
    }
}
