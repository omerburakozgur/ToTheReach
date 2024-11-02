using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(HealthManager))]

// To set launch and jump animations smoothly, jump button should call the launch animation
// and the launch animation event should call the main jump codeline and then auto transition 
// to the jumping animation.

// -> jump input set is grounded to false to exit from ground state machine
// play the launch animation, and it should continue just fine
// for falling also set is grounded to false
public class NewPlayerController : MonoBehaviour
{
    private AnimationScript animScript;
    TouchingDirections touchingDirections;
    HealthManager healthManager;
    public AudioSystem AudioSystemReference;

    //gameCanvas = GameObject.Find("GameUI").GetComponent<Canvas>();


    [Header("Movement")]
    [SerializeField] float walkSpeed = 200f;
    [SerializeField] float runSpeed = 300f;
    [SerializeField] float airwalkSpeed = 100f;
    [SerializeField] float jumpImpulse = 10;
    [SerializeField] public float ladderClimbSpeed = 10f;
    float tempWalkSpeed;
    float tempRunSpeed;
    public bool doubleJumpEnabled;
    public bool isOnLadder;
    public float coyoteTime;
    float coyoteTimeCounter;
    public int footstepStates = 0;
    bool isInDialogue = false;

    [Header("Damage References")]
    public Attack attackOne;
    public Attack attackTwo;
    public Attack attackThree;
    public Attack spAttackDamage;
    public Attack airAttackDamage;

    public int rangedAttackDamage;

    [Header("Stamina Requirements")]
    [SerializeField] public int jumpStaminaRequirement = 20;
    [SerializeField] public int slideStaminaRequirement = 20;
    [SerializeField] public int attackStaminaRequirement = 20;
    [SerializeField] public int SPAttackStaminaRequirement = 20;

    public bool canMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    public float CurrentMoveSpeed
    {
        get
        {
            if (canMove)
            {
                if (IsMoving && !touchingDirections.isOnWall)
                {
                    if (touchingDirections.isGrounded)
                    {
                        if (IsRunning) return runSpeed;

                        else return walkSpeed;
                    }
                    else
                    {
                        return airwalkSpeed;
                    }

                }
                else return 0; // idle speed is 0
            }
            else
                return 0;


        }
    }

    Vector2 moveInput;

    [SerializeField]
    private bool _isMoving = false;

    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
            //animator.SetBool("isLanded", false);

        }
    }

    [SerializeField]
    private bool _isRunning = false;

    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    private bool _isSliding = false;

    public bool IsSliding
    {
        get
        {
            return _isSliding;
        }
        private set
        {
            _isSliding = value;
            animator.SetBool(AnimationStrings.isSliding, value);
            print($"is Sliding:  {value}");

        }
    }

    public float rollVelocity = 5f;

    public bool IsFacingRight;

    [Header("Stamina")]
    public float staminaDecreaseRate = 0.1f;
    public float staminaRecoveryRate = 0.2f;
    public float maxStamina;

    public float _stamina;
    public float stamina
    {
        get { return _stamina; }

        set
        {
            _stamina = value;
            //uiManager.UpdatePlayerStamina();
        }
    }

    [HideInInspector]
    public Rigidbody2D rb;
    private Animator animator;
    public SpriteRenderer sr;
    public UIManager uiManager;

    [Header("Polish")]
    public CameraShake cameraShakeReference;
    public ParticleSystem dashParticle;
    public ParticleSystem jumpParticle;
    public ParticleSystem wallJumpParticle;
    public ParticleSystem slideParticle;

    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponentInChildren<PlayerAnimationManager>();
        animator = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        animScript = GetComponentInChildren<AnimationScript>();
        touchingDirections = GetComponent<TouchingDirections>();
        healthManager = GetComponent<HealthManager>();
        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
        AudioSystemReference = GameObject.Find("Audio System").GetComponent<AudioSystem>();
        tempRunSpeed = runSpeed;
        tempWalkSpeed = walkSpeed;

    }

    //dashParticle.Play();
    //jumpParticle.Play();
    //wallJumpParticle.Play();
    //slideParticle.Play();

    private void FixedUpdate()
    {

        if (!healthManager.LockVelocity && !isInDialogue)
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed * Time.fixedDeltaTime, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
        UpdateStamina();

        if (touchingDirections.isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive && !isInDialogue)
        {
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }

    }

    private void SetFacingDirection(Vector2 moveInput)
    {

        if (moveInput.x < 0 && !IsFacingRight)
        {
            IsFacingRight = true;
            transform.localScale *= new Vector2(-1, 1);

        }

        else if (moveInput.x > 0 && IsFacingRight)
        {
            IsFacingRight = false;
            transform.localScale *= new Vector2(-1, 1);

        }

    }

    public void OnRun(InputAction.CallbackContext context)
    {

        if (context.started && !isInDialogue)
        {

            if (stamina > 20)
            {
                IsRunning = true;

            }
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        cameraShakeReference.StartShakeCamera(0.7f);
        AudioSystemReference.PlaySound(AudioSystemReference.playerJump, transform.position, 5f);
        // check if alive
        if (context.started && coyoteTimeCounter > 0f && canMove && stamina > jumpStaminaRequirement && !isInDialogue)
        {
            stamina -= jumpStaminaRequirement;
            // Play launch anim
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
        else if (context.started && rb.velocity.y > 0f && !isInDialogue)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;

        }
        else if (context.started && doubleJumpEnabled && !isInDialogue)
        {
            stamina -= jumpStaminaRequirement;
            // Play launch anim
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
            doubleJumpEnabled = false;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && stamina > attackStaminaRequirement && !isInDialogue)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (context.started && stamina > attackStaminaRequirement && !isInDialogue)
        {
            stamina -= attackStaminaRequirement;
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
        }
    }

    public void OnSpecialAttack(InputAction.CallbackContext context)
    {
        if (context.started && stamina > SPAttackStaminaRequirement && !isInDialogue)
        {
            stamina -= SPAttackStaminaRequirement;

            animator.SetTrigger(AnimationStrings.specialAttackTrigger);
            rb.velocity = Vector2.zero;
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnSlide(InputAction.CallbackContext context)
    {
        // check if alive
        if (context.started && touchingDirections.isGrounded && canMove && animator.GetBool(AnimationStrings.isMoving) && stamina > slideStaminaRequirement && !isInDialogue)
        {
            stamina -= slideStaminaRequirement;
            // Play launch anim
            IsSliding = true;
            rb.velocity = new Vector2(rollVelocity, rb.velocity.y);
            healthManager.isInvincible = true;
            // set stamina

        }
    }

    public void UpdateStamina()
    {
        if (animator.GetBool(AnimationStrings.isRunning) && animator.GetBool(AnimationStrings.isMoving))
        {
            if (stamina > 0)
            {
                //reduce stamina
                _stamina -= staminaDecreaseRate;
                uiManager.UpdatePlayerStamina();


            }
        }
        else
        {
            if (_stamina < maxStamina && touchingDirections.isGrounded )
            { 
                //increase stamina
                _stamina += staminaRecoveryRate;
                uiManager.UpdatePlayerStamina();

            }
        }
    }

    public bool RestoreStamina(int staminaRestore)
    {
        if (IsAlive && stamina + staminaRestore < maxStamina)
        {
            int maxStaminaRestore = Mathf.Max((int)maxStamina - (int)stamina, 0);
            int actualRestore = Mathf.Min((int)maxStamina, staminaRestore);
            stamina += actualRestore;
            CharacterEvents.staminaPotionPickedUp(actualRestore);

            if (gameObject.tag.Equals("Player"))
            {
                uiManager.UpdatePlayerStamina();
            }
            return true;
        }

        return false;
    }

    public void OnLadderClimb(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (stamina > 20)
            {
                IsRunning = true;

            }
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void DisablePlayerMovement()
    {
        isInDialogue = true;
        rb.velocity = Vector2.zero;
        walkSpeed = 0;
        runSpeed = 0;

    }

    public void EnablePlayerMovement()
    {
        isInDialogue = false;
        walkSpeed = tempWalkSpeed;
        runSpeed = tempRunSpeed;

    }

    public void UpdateDamageValues(int damageIncrease)
    {
        attackOne.attackDamage += damageIncrease;
        attackTwo.attackDamage += damageIncrease;
        attackThree.attackDamage += damageIncrease;
        rangedAttackDamage += damageIncrease;
        spAttackDamage.attackDamage += damageIncrease;
        airAttackDamage.attackDamage += damageIncrease;


    }
}
