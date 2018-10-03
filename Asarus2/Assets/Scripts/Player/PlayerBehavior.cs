using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerBehavior : MonoBehaviour {

    [Header("Components References")]

    [Space(5)]
    [Tooltip("Referencia o animator das sprites")]
    public Animator Animator;
    [Tooltip("Referencia quem o player vai seguir")]
    public GameObject playerFollow;
    [Tooltip("Referencia o spawn do efeito de fumaça ao andar")]
    public GameObject[] EffectSpawn;
    [Tooltip("Efeito de fumaça")]
    public GameObject[] Effect;
    [Tooltip("Verifica o chão")]
    public Transform groundCheck;
    float groundRadius = 0.2f;
    [Tooltip("Que layes são consideradas Ground")]
    public LayerMask whatIsGround;
    [Tooltip("Objeto em que estão os sprites")]
    public Renderer Render;
    [Tooltip("Objeto em que estão os sprites")]
    public SortingGroup SGroup;
    [Tooltip("Sombras projetadas do personagem")]
    public GameObject[] shadows;
    // Ref Private
    Rigidbody2D rigPlayer;
    AudioSource AudioSource;

    [Header("Stats Values")]

    [Space(5)]
    [Tooltip("Velociade ao andar")]
    public float speedA;
    [Tooltip("Velociade ao correr")]
    public float speedB;
    [Tooltip("Força do pulo")]
    public float jumpForce;

    [Header("Control Values")]

    [Space(5)]
    [Tooltip("Refencia o personagem com relação ao game controller")]
    public int characterId;
    [Tooltip("Habilita controles")]
    public bool Controlable;
    [Tooltip("Habilita cutsces, desabilita todas as funções")]
    public bool Cutscene;
    [Tooltip("Habilita voar")]
    public bool isFlying;
    [Tooltip("Distância em que segue")]
    public float followDistance = 1;

    [Header("Functions Enable")]

    [Space(5)]
    [Tooltip("Habilita mecânica")]
    public bool walkEnable;
    [Tooltip("Habilita mecânica")]
    public bool runEnable;
    [Tooltip("Habilita mecânica")]
    public bool jumpEnable;
    [Tooltip("Habilita mecânica")]
    public bool doubleJumpEnable;
    [Tooltip("Habilita mecânica")]
    public bool dashEnable;
    [Tooltip("Habilita mecânica")]
    public bool hitEnable;
    [Tooltip("Habilita mecânica")]
    public bool deathEnable;

    [Header("Dash e Hit Proprieties")]

    [Space(5)]
    [Tooltip("Tempo entre dashes")]
    public float dashReset;
    [Tooltip("Tempo entre hits")]
    public float hitReset;
    [Tooltip("Tempo entre anim de hits")]
    public float hitAnimReset;
    [Tooltip("Força do dash")]
    public float dashForce;
    [Tooltip("Stamina gasta ao utilizar habilidade")]
    public int dashCost, jumpCost;

    [Header("Functions Enable")]

    [Space(5)]
    [Tooltip("Diz o tempo para o audio ao andar se repetir, esse tempo é dividido em 2 ao correr")]
    public float walkAudioTime, waitAfterDie;
    [Tooltip("Audio correspondente")]
    public AudioClip
    jumpAudio,
    fallAudio,
    hitAudio,
    dashAudio,
    walkAudio;

    // Private variables
    [SerializeField]
    bool
        attakcing,      // diz se ataca
        doubleJumping, // diz se está pulando pela segunda vez
        dashing,      // diz se está em dash
        grounded,    // diz quando enconsta em um chão
        walking,    // diz se está andando
        runing,    // diz se está correndo
        jumping,  // diz se está pulando
        falling, // diz se está caindo
        hiting, // diz se recebeu um golpe
        idle,  // diz se está parado
        death,// diz se o personagem está morto
        hit; // diz se está invunerável

    float timerChangeChar, timerFallTru, timerAudioWalk, dashTimer, hitTimer, hitAnimTimer, hitBlinkTimer;
    float directionX, directionY;
    bool canFallTru, far; // diz se está muito longe
    // Habilita e desabilita animações ou controle por outro script
    [SerializeField]
    bool actionEnable = true, animEnable = true;

    void Start () {
        rigPlayer = GetComponent<Rigidbody2D>();
        AudioSource = GetComponent<AudioSource>();
        if(characterId == CharacterChangeInformation.atual)
        {
            GameObject Gc = GameObject.FindGameObjectWithTag("GameController");
            Gc.SendMessage("SpeedAReciver", speedA);
            Gc.SendMessage("SpeedBReciver", speedB);
        }
	}
    void Update () {
        HitComponents();
        ShadowControl();
        if (death)
        {
            GamePad.SetVibration(PlayerIndex.One, 0,0);
            Animator.SetBool("Death", true);
            StartCoroutine(changeLevel());
        }
        if (!dashing)
        {
            grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        }
        if (!Cutscene)
        {
            gameObject.SendMessage("DisableCutsceneNow");
            // Recebe se estamos no chão
            if (!dashing)
            {
                if (falling && grounded)
                {
                    EffectSpawner(Effect[0], EffectSpawn[1]);
                    AudioSource.PlayOneShot(fallAudio);
                }
            }
            else grounded = false;

            if (characterId == CharacterChangeInformation.atual)
            {
                Controlable = true;
                gameObject.tag = "PlayerAtual";
            }
            else
            {
                Controlable = false;
                gameObject.tag = "Player";
            }
            if (Controlable)
            {
                if (!AudioSource.enabled)
                {
                    AudioSource.enabled = true;
                }
                if (actionEnable)
                {
                    ChangeChar();
                    Dash(transform.localScale);
                    JumpControl();
                    if (!attakcing)
                    {
                        Movement();
                    }
                    BoxColliderEnable();
                    if (canFallTru) FallTru();
                }
                else DisableActions();
            }
            else
            {
                if (AudioSource.enabled)
                {
                    AudioSource.enabled = false;
                }
                AutoFollow();
            }
            if (animEnable) Animations();
        }
        else
        {
            gameObject.SendMessage("EnableCutsceneNow");
        }
        if (jumping)
        {
            gameObject.layer = 11;
        }
        else gameObject.layer = 10;
        if (!attakcing)
        {
            LookControl();
        }
        if (attakcing)
        {
            dashing = false;
            dashTimer = 0;
        }
    }
    IEnumerator changeLevel()
    {
        float fadeTime = GameObject.FindGameObjectWithTag("GameController").GetComponent<FadeScript>().BeginFade(1);
        yield return new WaitForSeconds(waitAfterDie);
        Scene cena = SceneManager.GetActiveScene();
        SceneManager.LoadScene(cena.name);
    }
    void FixedUpdate()
    {
        if (!Cutscene )
        {
            if (Controlable && !attakcing)
            {
                if (actionEnable)
                {
                    Walk(speedA, speedB);
                }
            }
        }
        else
        {
            if(!attakcing) Walk(speedA, speedB);
        }
        Animator.SetFloat("DirectionX", directionX);
    }
    // Animations
    void Animations()
    {
        if (idle) Animator.SetBool("Idle", true); else Animator.SetBool("Idle", false);
        if (walking) Animator.SetBool("Walk1", true); else Animator.SetBool("Walk1", false);
        if (runing) Animator.SetBool("Walk2", true); else Animator.SetBool("Walk2", false);
        if (jumping) Animator.SetBool("Jump", true); else Animator.SetBool("Jump", false);
        if (falling) Animator.SetBool("Fall", true); else Animator.SetBool("Fall", false);
        if (dashing) Animator.SetBool("Dash", true); else Animator.SetBool("Dash", false);
        if(hiting) Animator.SetBool("Hit", true); else Animator.SetBool("Hit", false);
    }
    // Actions
    void Movement()
    {
        SGroup.sortingOrder = 0;
        // Controls movement
        directionX = Input.GetAxisRaw("Horizontal");
        directionY = (int)Input.GetAxisRaw("Vertical");

        // Fall control
        if (rigPlayer.velocity.y < 0 && !grounded && !dashing) falling = true; else falling = false;

        // Idle
            if (!walking && !runing && !falling && !jumping && grounded) idle = true; else idle = false;

        // Hit
        HitComponents();    }
    void Jump(float jump)
    {
        jumping = true;
        rigPlayer.velocity = new Vector2(rigPlayer.velocity.x, 0);
        rigPlayer.AddForce(new Vector2(0, jump));
        EffectSpawner(Effect[0], EffectSpawn[1]);
        grounded = false;
        AudioSource.PlayOneShot(jumpAudio);
    }
    void JumpControl()
    {
        // Jump control
        if (Input.GetButtonDown("A") && grounded && directionY >= 0 && HudGerenciator.interactiveOn == false) Jump(jumpForce);
        else
        {
            if (Input.GetButtonDown("A") && !doubleJumping && !grounded && doubleJumpEnable && StatsBehaior.Stamina[characterId] > 0)
            {
                StatsBehaior.Stamina[characterId] -= jumpCost;
                Jump(jumpForce / 1.25f);
                doubleJumping = true;
            }
        }
        if (grounded) doubleJumping = false;
        if ((falling || (grounded && (idle || walking || runing || jumping) && rigPlayer.velocity.y == 0))) jumping = false;
        if (rigPlayer.velocity.y > 0 && !grounded && !dashing) jumping = true;
    }
    void HitComponents()
    {
        if (hit)
        {
            hitBlinkTimer += Time.deltaTime;
            if(hitBlinkTimer >= 0.2f && !hiting)
            {
                if (Render.enabled == true)
                {
                    Render.enabled = false;
                } else Render.enabled = true;
                hitBlinkTimer = 0;
            }
            hitTimer -= Time.deltaTime;
        }
        if (hitTimer <= 0 && hit)
        {
            hit = false;
            Render.enabled = true;
        }
        if (hiting)
        {
            hitAnimTimer += Time.deltaTime;
            GamePad.SetVibration(PlayerIndex.One, 1, 1);
            if (hitAnimTimer >= hitAnimReset)
            {
                hiting = false;
                hitAnimTimer = 0;
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
            }
        }
    }
    void Hit(int dmg)
    {
        attakcing = false;
        if (!dashing && !hit)
        {
            AudioSource.PlayOneShot(hitAudio);
            Instantiate(Effect[3], transform.position,transform.rotation);
            hit = true;
            hitTimer = hitReset;
            hiting = true;
            StatsBehaior.Life[characterId] -= dmg;
            GameObject Cam = GameObject.FindGameObjectWithTag("MainCamera");
            Cam.SendMessage("ShakeCamera", 0.2f);
        }
    }
    void FallTru()
    {
        if (Input.GetButtonDown("A") && directionY == -1)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            falling = true;
        }
    }
    void Dash(Vector3 scale)
    {
        if (Input.GetButtonDown("B") && dashEnable && !dashing && StatsBehaior.Stamina[characterId] > 0)
        {
            jumping = false;
            dashTimer = dashReset;
            dashing = true;
            StatsBehaior.Stamina[characterId] -= dashCost;
            EffectSpawner(Effect[1], gameObject);
            AudioSource.PlayOneShot(dashAudio);
            hitTimer = hitReset;
            dashTimer = dashReset;
        }
        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
            rigPlayer.AddForce(new Vector2(dashForce * scale.x, 0));
        } else dashing = false;

    }
    void Walk(float speedAA, float speedBB)
    {
        // Walk Control
        if (walkEnable && !runing)
        {
            if (directionX != 0)
            {
                timerAudioWalk += Time.deltaTime;
                if (walking && timerAudioWalk > walkAudioTime && grounded)
                {
                    EffectSpawner(Effect[2], EffectSpawn[0]);
                    AudioSource.PlayOneShot(walkAudio);
                    timerAudioWalk = 0;
                }
                if (rigPlayer.velocity.y == 0 && rigPlayer.velocity.x != 0 && grounded) walking = true; else walking = false;
            }
            else walking = false;
            rigPlayer.velocity = new Vector2(directionX * speedAA, rigPlayer.velocity.y);
        }
        else
        {
            if (runEnable)
            {
                if (directionX != 0)
                {
                    timerAudioWalk += Time.deltaTime;
                    if (runing && timerAudioWalk > walkAudioTime / 2 && grounded)
                    {
                        EffectSpawner(Effect[2], EffectSpawn[0]);
                        AudioSource.PlayOneShot(walkAudio);
                        timerAudioWalk = 0;
                    }
                    if (rigPlayer.velocity.y == 0 && rigPlayer.velocity.x != 0 && grounded) runing = true; else runing = false;
                }
                else runing = false;

                rigPlayer.velocity = new Vector2(directionX * speedBB, rigPlayer.velocity.y);
            }
        }
        if (isFlying)
        {
            rigPlayer.velocity = new Vector2(directionX * speedAA, directionY * speedBB);
        }
    }
    void ChangeChar()
    {
        if (Input.GetButtonDown("Y") && grounded && CharacterChangeInformation.canChange && timerChangeChar <= 0
            && playerFollow.GetComponent<Rigidbody2D>().velocity.y == 0 && HudGerenciator.interactiveOn == false)
        {
            GameObject GC = GameObject.FindGameObjectWithTag("GameController");
            GC.SendMessage("ChangeCharAtual");
            GameObject Cam = GameObject.FindGameObjectWithTag("MainCamera");
            Cam.SendMessage("ChangeCamera", playerFollow.transform);
            timerChangeChar = 0.4f;
        }
        if(timerChangeChar > 0)
        {
            timerChangeChar -= Time.deltaTime;
            GameObject GC = GameObject.FindGameObjectWithTag("GameController");
            GC.SendMessage("SetAtualValuesToBars");
            if (!hit)
            {
                hitTimer = 0.4f;
                hit = true;
            }
        }
    }
    void DisableActions()
    {
        rigPlayer.velocity = new Vector2(0,0);
        dashing = false;
        walking = false;
        runing = false;
        jumping = false;
        falling = false;
        hiting = false;
        idle = false;
    }
    void EnableActions()
    {
        actionEnable = true;
    }
    void CanFallTruEnable(bool aaa)
    {
        canFallTru = aaa;
    }
    void Death()
    {
        DisableActions();
        Cutscene = true;
        death = true;
    }
    // Attacks
    void AttackKeikeReciver()
    {
        if (!Cutscene && Controlable && !hiting && StatsBehaior.Mana[characterId] > 0 && grounded)
        {
            gameObject.SendMessage("GroundAttack", characterId);
            actionEnable = false;
            attakcing = true;
            directionX = 0;
        }
    }
    void AttackKeikeReciver2()
    {
        if (Controlable && !hiting && StatsBehaior.Mana[characterId] > 0 && StatsBehaior.Stamina[characterId] > 0 && !grounded)
        {
            gameObject.SendMessage("AirAttack", characterId);
            dashing = false;
            actionEnable = false;
            attakcing = true;
            directionX = 0;
        }
    }
    void AttackKellyReciver()
    {
        if(!Cutscene && Controlable && !hiting && StatsBehaior.Stamina[characterId] > 0 && grounded)
        {
            gameObject.SendMessage("GroundAttack", characterId);
            actionEnable = false;
            attakcing = true;
            directionX = 0;
        }
        if (Controlable && !hiting && StatsBehaior.Stamina[characterId] > 0 && !grounded)
        {
            gameObject.SendMessage("AirAttack", characterId);
            DisableActions();
            actionEnable = false;
            attakcing = true;
            directionX = 0;
        }
    }
    void AttackKellyReciver2()
    {
        if (!Cutscene && Controlable && !hiting && StatsBehaior.Stamina[characterId] > 0 &&  grounded && StatsBehaior.Mana[characterId] > 0)
        {
            gameObject.SendMessage("AttackCharge");
            actionEnable = false;
            attakcing = true;
            directionX = 0;
        }
    }
    void DisableAttack()
    {
        attakcing = false;
        actionEnable = true;
    }
    // Effects
    void ShadowControl()
    {
        if (grounded && rigPlayer.velocity.y == 0)
        {
            shadows[0].GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            shadows[0].GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    // Utilities actions
    void AutoFollow()
    {
        SGroup.sortingOrder = -1;
        // Idle
        if (!walking && !runing && rigPlayer.velocity.y == 0 && grounded) idle = true; else idle = false;

        // Fall 
        if (rigPlayer.velocity.y < 0 && !grounded && !dashing) falling = true; else falling = false;

        // Walk
        if (!far) Walk(CharacterChangeInformation.speedAtualA, CharacterChangeInformation.speedAtualB);
        if (playerFollow.transform.position.x - followDistance > transform.position.x) directionX = ((playerFollow.transform.position.x - transform.position.x)/2);
        else
            if (playerFollow.transform.position.x + followDistance < transform.position.x) directionX = ((transform.position.x - playerFollow.transform.position.x) / 2 * -1); else directionX = 0;

        // Jump
        if (playerFollow.transform.position.y - followDistance / 2 > transform.position.y && !jumping) Jump(jumpForce);
        if (rigPlayer.velocity.y < 0 && jumping || grounded && playerFollow.transform.position.y - followDistance / 2 <= transform.position.y) jumping = false;
        if (rigPlayer.velocity.y > 0 && !grounded && !dashing) jumping = true;

        // Look
        LookControl();
        // Disable box collider
        if (transform.position.x > playerFollow.transform.position.x + followDistance * 2 ||
            transform.position.y > playerFollow.transform.position.y + followDistance * 2 ||
            transform.position.x < playerFollow.transform.position.x - followDistance * 2 ||
            transform.position.y < playerFollow.transform.position.y - followDistance * 2)
        {
            far = true;
        }
        else
        {
            if(transform.position.y <= playerFollow.transform.position.y) far = false;
        }
        if (far)
        {
            Walk(CharacterChangeInformation.speedAtualA * 2, CharacterChangeInformation.speedAtualB * 2);
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
        else GetComponent<CapsuleCollider2D>().enabled = true;

        //Bug
        if(Render.enabled == false)
        {
            Render.enabled = true;
        }
    }
    void LookControl()
    {
        // Look Control
        Vector2 scale = transform.localScale;
        if (directionX > 0 && scale.x < 0 && !attakcing)
        {
            scale.x *= -1;
            transform.localScale = scale;
        }
        if (directionX < 0 && scale.x > 0 && !attakcing)
        {
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
    void BoxColliderEnable()
    {
        if (GetComponent<CapsuleCollider2D>().enabled == false)
        {
            timerFallTru += Time.deltaTime;
            if (timerFallTru > 0.5f)
            {
                GetComponent<CapsuleCollider2D>().enabled = true;
                timerFallTru = 0;
            }
        }
    }
    void EffectSpawner(GameObject Effect, GameObject Spawner)
    {
        Instantiate(Effect, Spawner.transform.position, transform.rotation);
    }
    // Funcionalities
    void PlayAudio(AudioClip audi)
    {
        AudioSource.PlayOneShot(audi);
    }
    void EnableCutscene()
    {
        Cutscene = true;
    }
    void DisableCutscenes()
    {
        Cutscene = false;
    }
    void ChangeWalkAudio(AudioClip audio)
    {
        walkAudio = audio;
    }
    void ChangeDirectionX(int dir)
    {
        directionX = dir;
    }
    // Collisions
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Moving Ground" && !jumping)
        {
            transform.parent = collision.transform;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Moving Ground")
        {
            transform.parent = null;
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Interactive Object") && Controlable)
        {
            col.SendMessage("EnableInteraction", characterId);
            col.SendMessage("PlayerReference", gameObject);
        }
        if (col.tag.Equals("Coin") && Controlable)
        {
            col.SendMessage("Collect");
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag.Equals("Vision Box") && Controlable)
        {
            col.SendMessage("Vision", "ChangeToAttackMode", SendMessageOptions.DontRequireReceiver);
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag.Equals("Interactive Object") && Controlable)
        {
            col.SendMessage("DisableInteraction", false);
        }
        if (col.tag.Equals("Vision Box") && Controlable)
        {
            col.SendMessage("Vision", "ChangeToNormalMode",SendMessageOptions.DontRequireReceiver);
        }
    }
}
