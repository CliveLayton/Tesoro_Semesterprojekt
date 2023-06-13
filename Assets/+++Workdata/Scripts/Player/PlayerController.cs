using Cinemachine;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public enum State
    {
        Blob,
        Figure
    }

    #region Inspector

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 4f;

    [SerializeField] private float sprintSpeed = 9f;

    [SerializeField] private float climbSpeed = 3f;

    [SerializeField] private float speedChangeRate = 10f;

    [SerializeField] private float jumpPower = 15f;

    [SerializeField] private float wallSlidingSpeed = 4f;

    [Header("GroundCheck")]
    [SerializeField] private Vector3 groundCheckPos;
    [SerializeField] private Vector2 groundCheckSize;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private Vector3 wallCheckPos;
    [SerializeField] private Vector2 wallCheckSize;

    [Header("Legde Info")]
    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;
    [SerializeField] private Vector2 offset3;

    public CinemachineVirtualCamera cam;

    public LogicScript logic;

    public bool isChanging;

    #endregion

    #region Input Variables

    private GameInput inputActions;
    private InputAction moveAction;

    #endregion

    #region Private Variables

    public State state;

    private Rigidbody2D rb;

    private Animator anim;

    private SpriteRenderer sr;

    private CapsuleCollider2D col;

    private GameObject currentOneWayPlatform;

    public bool isRunning, isInteracting, isJumping, isWallSliding, isDying, leftMovement;
    public bool isGrounded, isWalled, isKilling;
    public bool ledgeDetected, canClimbLedge, canClimbWall;
    private bool canGrabLedge = true;

    private Vector2 climbBegunPosition;
    private Vector2 climbOverPosition;

    private int directionMultiply;
    public float lastMoveSpeed;

    private Vector2 lastMovement;

    private Vector2 moveInput;

    private EventInstance playerFootsteps;

    #endregion

    #region Unity Event Functions

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<CapsuleCollider2D>();
        logic = GameObject.FindGameObjectWithTag("Counter").GetComponent<LogicScript>();

        inputActions = new GameInput();
        moveAction = inputActions.Player.Move;
        state = State.Blob;
    }

    private void Start()
    {
        playerFootsteps = AudioManager.instance.CreateEventInstance(FMODEvents.instance.playerFootsteps);
    }

    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Blob:
                break;
            case State.Figure:
                CheckForLedge();
                WallSlide();
                break;
        }

        isGrounded = Physics2D.OverlapBox(transform.position + groundCheckPos, groundCheckSize, 0, groundLayer);

        if(canClimbWall)
            isWalled = Physics2D.OverlapBox(transform.position + wallCheckPos, wallCheckSize, 0, groundLayer);

        isKilling = Physics2D.OverlapBox(transform.position + groundCheckPos, groundCheckSize, 0, enemyLayer);

        UpdateSound();

        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }

        if (!canClimbLedge && !isDying)
        {
            Movement();
        }
    }

    private void LateUpdate()
    {
        UpdateAnimator();
    }

    private void WallSlide()
    {
        if (isWalled && !isGrounded && !canClimbLedge && moveInput.x != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void CheckForLedge()
    {
        if(ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;
            rb.velocity = new Vector2(0, 0);

            /* Vector2 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;

            if (!leftMovement)
            {
                climbBegunPosition = ledgePosition + offset1;
                climbOverPosition = ledgePosition + offset2;
            }
            else if (leftMovement)
            {
                climbBegunPosition = ledgePosition - offset3;
                climbOverPosition = ledgePosition - offset2;
            }
            */

            canClimbLedge = true;
        }

        if (canClimbLedge)
        {
            rb.velocity = new Vector2(0, 0);
            transform.position = climbBegunPosition;
        }
    }

    public void LedgeClimbOver()
    {
        canClimbLedge = false;
        transform.position = climbOverPosition;
        Invoke("AllowLedgeGrab", 0.1f);
    }

    private void AllowLedgeGrab()
    {
        canGrabLedge = true;
    }

    private IEnumerator Dying()
    {
        yield return new WaitForSeconds(0.3f);
        switch (state)
        {
            case State.Blob:
                anim.SetTrigger("BlobDying");
                break;
            case State.Figure:
                anim.SetTrigger("FigureDying");
                break;
        }
        isDying = true;
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        switch (state)
        {
            case State.Blob:
                break;
            case State.Figure:
                cam.Follow = null;
                col.enabled = false;
                break;
        }
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnEnable()
    {
        switch (state)
        {
            case State.Blob:
                break;
            case State.Figure:
                anim.SetTrigger("setState");
                break;
        }
        inputActions.Enable();
        inputActions.Player.Sprint.performed += Run;
        inputActions.Player.Sprint.canceled += Run;

        inputActions.Player.Interact.performed += Interact;
        inputActions.Player.Interact.canceled += Interact;

        inputActions.Player.Jump.performed += Jump;
        inputActions.Player.Jump.canceled += Jump;
    }

    private void OnDisable()
    {
        playerFootsteps.stop(STOP_MODE.ALLOWFADEOUT);

        inputActions.Disable();
        inputActions.Player.Sprint.performed -= Run;
        inputActions.Player.Sprint.canceled -= Run;

        inputActions.Player.Interact.performed -= Interact;
        inputActions.Player.Interact.canceled -= Interact;

        inputActions.Player.Jump.performed -= Jump;
        inputActions.Player.Jump.canceled -= Jump;
    }

    #endregion

    #region Collision Functions

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!isKilling)
            {
                switch (state)
                {
                    case State.Blob:
                        anim.SetTrigger("BlobGetHurt");
                        break;
                    case State.Figure:
                        anim.SetTrigger("FigureGetHurt");
                        break;
                }

                logic.ReduceScore(1);

                if (logic.lifePoints <= 0)
                {
                    isDying = true;
                    StartCoroutine(Dying());
                }

            } 
            else if (isKilling)
            {
                collision.gameObject.GetComponent<TestingEnemy>().DestroyEnemy();
            }
           
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(this.col, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(this.col, platformCollider, false);
    }

    #endregion

    #region Input CallbackContext Methods

    void Run(InputAction.CallbackContext context)
    {
        isRunning = context.performed;
    }

    void Interact(InputAction.CallbackContext context)
    {
        isInteracting = context.performed;
    }

    void Jump(InputAction.CallbackContext context)
    {
        switch (state)
        {
            case State.Blob:
                if (context.performed && isGrounded && !isDying)
                {
                    isJumping = true;
                    anim.SetTrigger("BlobHoldJump");
                }

                if (context.canceled && isGrounded && !isDying && isJumping)
                {
                    rb.AddForce(new Vector2(rb.velocity.x, jumpPower), ForceMode2D.Impulse);
                    anim.SetTrigger("BlobJump");
                    isJumping = false;
                }
                break;
            case State.Figure:
                if (context.performed && isGrounded && !isDying)
                {
                    rb.AddForce(new Vector2(rb.velocity.x, jumpPower), ForceMode2D.Impulse);
                    anim.SetTrigger("FigureJump");
                }
                break;
        }
        //rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
    }

    #endregion

    #region Movement

    private void Movement()
    {
        float targetSpeed = (moveInput.x == 0 ? 0 : (isRunning ? sprintSpeed : movementSpeed) * moveInput.magnitude);

        float currentSpeed = lastMovement.magnitude;

        if(Mathf.Abs(currentSpeed - targetSpeed) > 0.01f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, speedChangeRate * Time.deltaTime);
        }
        else
        {
            currentSpeed = targetSpeed;
        }

        if(moveInput.x > 0)
        {
            sr.flipX = false;
            leftMovement = false;
        }
        else if (moveInput.x < 0)
        {
            sr.flipX = true;
            leftMovement = true;
        }

        directionMultiply = leftMovement ? -1 : 1;
        wallCheckPos.x = leftMovement ? -0.5f : 0.5f;

        if((moveInput.y < 0) && (currentOneWayPlatform != null))
        {
            StartCoroutine(DisableCollision());
        }


        if (isWalled && currentOneWayPlatform == null)
        {
            rb.velocity = new Vector2(moveInput.x, climbSpeed * moveInput.y); ;
        }
        else
        {
            rb.velocity = new Vector2(currentSpeed * directionMultiply, rb.velocity.y);
        }
        lastMovement.x = currentSpeed;
        

    }

    #endregion

    #region Animation

    private void UpdateAnimator()
    {
        Vector2 velocity = lastMovement;
        velocity.y = 0;
        float speed = velocity.magnitude;

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("MovementSpeed", speed);
        anim.SetBool("canClimb", canClimbLedge);
        if (rb.velocity.y < -1)
        {
            anim.SetBool("fall", true);
        }
        else
        {
            anim.SetBool("fall", false);
        }
    }

    public void Change()
    {
        switch (state)
        {
            case State.Blob: //Change it to State Figure
                anim.SetTrigger("setState");
                canClimbWall = true;
                state = State.Figure;
                break;
            case State.Figure: // Change it to State Blob
                canClimbWall = false;
                state = State.Blob;
                break;
        }
        anim.SetTrigger("change");
    }

    #endregion

    #region Sound

    private void UpdateSound()
    {
        //start footsteps event if the player has an x velocity and is on the ground
        if(rb.velocity.x !=0 && isGrounded)
        {
            //get the playback state
            PLAYBACK_STATE playbackState;
            playerFootsteps.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                playerFootsteps.start();
            }
        }
        //otherwise, stop the footsteps event
        else
        {
            playerFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + groundCheckPos, groundCheckSize);
        Gizmos.DrawWireCube(transform.position + wallCheckPos, wallCheckSize);
    }
}
