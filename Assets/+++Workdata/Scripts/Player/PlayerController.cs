using Cinemachine;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// states of the Player
    /// </summary>
    private enum State
    {
        Blob,
        Figure
    }

    #region Inspector

    [Header("Movement")]

    //walk speed of the Figure
    [SerializeField] private float movementSpeedFigure = 4f;

    //walk speed of the Blob
    [SerializeField] private float movementSpeedBlob = 2f;

    //sprint speed of the player
    [SerializeField] private float sprintSpeed = 9f;

    //climb speed for the Figure
    [SerializeField] private float climbSpeed = 3f;

    //speedchangeRate for movement of the player
    [SerializeField] private float speedChangeRate = 10f;

    //jump power for the Figure
    [SerializeField] private float FigureJumpPower = 15f;

    //shroom jump power
    [SerializeField] private float shroomJump = 18f;

    //wall slide speed for the player
    [SerializeField] private float wallSlidingSpeed = 4f;

    [SerializeField] private float fallClamp = -20f;

    [Header("GroundCheck")]
    
    //position of the groundCheck
    [SerializeField] private Vector3 groundCheckPos;

    //size of the groundCheck
    [SerializeField] private Vector2 groundCheckSize;

    //Layer of the ground
    [SerializeField] private LayerMask groundLayer;

    //Layer of enemies
    [SerializeField] private LayerMask enemyLayer;

    //Layer of shrooms
    [SerializeField] private LayerMask shroomLayer;

    //Layer of Walls where the player can climb
    [SerializeField] private LayerMask climbingWallLayer;

    //position of the wallCheck
    [SerializeField] private Vector3 wallCheckPos;

    //size of the wallCheck
    [SerializeField] private Vector2 wallCheckSize;

    [Header("Legde Info")]

    //offset of the begin position of the player while climb on a ledge to the right
    [SerializeField] private Vector2 offset1;

    //offset of the end position of the player while climb on a ledge
    [SerializeField] private Vector2 offset2;

    //offset of the begin position of the player while climb on a ledge to the left
    [SerializeField] private Vector2 offset3;

    //offset for the end position of the player as the Blob while climb on a ledge
    [SerializeField] private Vector2 offsetBlob;

    //transform of the climbposition of the player
    [SerializeField] private Transform climbPosition;

    //offset of the climbPosition
    [SerializeField] private Vector3 climbPositionoffset;

    [Header("Linking Objects")]

    /// <summary>
    /// Default Virtual Camera Object
    /// </summary>
    public CinemachineVirtualCamera cam;

    /// <summary>
    /// Virtual Camera for Title zoom out
    /// </summary>
    public CinemachineVirtualCamera camZoomOut;

    /// <summary>
    /// logic script
    /// </summary>
    public LogicScript logic;

    /// <summary>
    /// link to the PlayerHealth script
    /// </summary>
    public PlayerHealth health;

    /// <summary>
    /// link to the DeathZone script
    /// </summary>
    public DeathZone deathZone;

    /// <summary>
    /// link to the Animator of the blackscreen panel
    /// </summary>
    public Animator panelAnim;

    /// <summary>
    /// link to the Animator of the lifebar
    /// </summary>
    public Animator lifebarAnim;

    /// <summary>
    /// the button to change the state from the player to Figure
    /// </summary>
    public GameObject changeToFigureButton;

    /// <summary>
    /// the button to change the state from the player to Blob
    /// </summary>
    public GameObject changeToBlobButton;

    /// <summary>
    /// reference to the level begin music
    /// </summary>
    [SerializeField] private MusicArea levelBegin;

    /// <summary>
    /// reference to the level music
    /// </summary>
    [SerializeField] private MusicArea levelMusic;

    #endregion

    #region Input Variables

    /// <summary>
    /// Input Action Asset
    /// </summary>
    private GameInput inputActions;

    /// <summary>
    /// Input Action for the player movement
    /// </summary>
    private InputAction moveAction;

    #endregion

    #region Private Variables

    /// <summary>
    /// current state of the player
    /// </summary>
    private State state;

    /// <summary>
    /// rigidbody2D of the player
    /// </summary>
    private Rigidbody2D rb;

    /// <summary>
    /// animator of the player
    /// </summary>
    private Animator anim;

    /// <summary>
    /// sprite renderer of the player
    /// </summary>
    private SpriteRenderer sr;

    /// <summary>
    /// capsule collider 2D of the player
    /// </summary>
    private CapsuleCollider2D col;

    /// <summary>
    /// current one way platform the player is standing on
    /// </summary>
    private GameObject currentOneWayPlatform;

    /// <summary>
    /// bool for player presses the run button
    /// </summary>
    public bool isRunning;

    /// <summary>
    /// bool for player presses the interact button
    /// </summary>
    public bool isInteracting;

    /// <summary>
    /// bool for player is currently wallsliding
    /// </summary>
    public bool isWallSliding;

    /// <summary>
    /// bool for player is currently jumping
    /// </summary>
    public bool isJumping;

    /// <summary>
    /// bool for player is currently dying
    /// </summary>
    public bool isDying;

    /// <summary>
    /// bool for player is moving left
    /// </summary>
    public bool leftMovement;

    /// <summary>
    /// bool for jumpcooldown of the player
    /// </summary>
    public bool jumpCooldown;

    /// <summary>
    /// bool for the cooldown to change the state of the player
    /// </summary>
    public bool changeCooldown;

    /// <summary>
    /// bool for checking if the player is in a sequence
    /// </summary>
    public bool inSequence;

    /// <summary>
    /// bool for check if the player can change the state
    /// </summary>
    public bool canChangeState;

    /// <summary>
    /// bool for player is grounded
    /// </summary>
    public bool isGrounded;

    /// <summary>
    /// bool for player is walled
    /// </summary>
    public bool isWalled;

    /// <summary>
    /// bool for player is jumping on an enemy
    /// </summary>
    public bool isKilling;

    /// <summary>
    /// bool for player is jumping on a shroom
    /// </summary>
    public bool isShroomed;

    /// <summary>
    /// bool for checking if the player is currently in a small passage
    /// </summary>
    public bool inSmallPassage;

    /// <summary>
    /// bool for player is on a climbingWall
    /// </summary>
    public bool climbingWall;

    /// <summary>
    /// bool for player is climbing on a wall
    /// </summary>
    public bool climbWall;

    /// <summary>
    /// bool for the player reaches a ledge
    /// </summary>
    public bool ledgeDetected;

    /// <summary>
    /// bool for player can climb on the ledge
    /// </summary>
    public bool canClimbLedge;

    /// <summary>
    /// bool for the player can grab a ledge
    /// </summary>
    private bool canGrabLedge = true;

    /// <summary>
    /// begin position of the player while climb on a ledge
    /// </summary>
    private Vector2 climbBegunPosition;

    /// <summary>
    /// end position of the player while climb on a ledge
    /// </summary>
    private Vector2 climbOverPosition;

    /// <summary>
    /// bool to allow player to climb on a wall
    /// </summary>
    private bool allowClimb;

    /// <summary>
    /// 1 or -1 to calculate left or right movement
    /// </summary>
    private int directionMultiply;

    /// <summary>
    /// last movement speed of the player
    /// </summary>
    public float lastMoveSpeed;

    /// <summary>
    /// time for the player to press the jump button after leaving the ground
    /// </summary>
    private float coyoteTime = 0.2f;

    /// <summary>
    /// counter for the coyotetime 
    /// </summary>
    public float coyoteTimeCounter;

    /// <summary>
    /// last movement input of the player
    /// </summary>
    private Vector2 lastMovement;

    /// <summary>
    /// last movement input of the player for climbing
    /// </summary>
    private Vector2 lastMovementClimb;

    /// <summary>
    /// current movement input of the player
    /// </summary>
    private Vector2 moveInput;

    /// <summary>
    /// Audio Eventinstance for player footsteps
    /// </summary>
    private EventInstance playerFootsteps;

    #endregion

    #region Unity Event Functions

    //get all components of the player and the input action asset
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<CapsuleCollider2D>();
        logic = GameObject.FindGameObjectWithTag("Counter").GetComponent<LogicScript>();
        health = GetComponent<PlayerHealth>();
        deathZone = GameObject.FindGameObjectWithTag("DeathZone").GetComponent<DeathZone>();

        inputActions = new GameInput();
        moveAction = inputActions.Player.Move;
        state = State.Figure;
    }

    //set the audio for playerfootsteps
    private void Start()
    {
        AudioManager.instance.SetMusicArea(levelBegin);
        playerFootsteps = AudioManager.instance.CreateEventInstance(FMODEvents.instance.playerFootsteps);
    }

    //get the moveinput and set the coyotetime counter
    //set the jump power for the blob
    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        coyoteTimeCounter = isGrounded ? coyoteTime : coyoteTimeCounter - Time.deltaTime;
    }

    //set the ground check, wall check, shroom check and killing check. perform movement method
    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Blob:
                WallSlide();
                CheckForLedge();
                break;
            case State.Figure:
                CheckForLedge();
                break;
        }

        isGrounded = Physics2D.OverlapBox(transform.position + groundCheckPos, groundCheckSize, 0, groundLayer);

        isWalled = Physics2D.OverlapBox(transform.position + wallCheckPos, wallCheckSize, 0, groundLayer);

        isKilling = Physics2D.OverlapBox(transform.position + groundCheckPos, groundCheckSize, 0, enemyLayer);

        isShroomed = Physics2D.OverlapBox(transform.position + groundCheckPos, groundCheckSize, 0, shroomLayer);

        climbingWall = Physics2D.OverlapBox(transform.position + wallCheckPos, wallCheckSize, 0, climbingWallLayer);

        UpdateSound();

        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            rb.velocity = new Vector2(0, 0);
            lastMovement = new Vector2(0, 0);
            return;
        }

        if (!canClimbLedge && !isDying && !inSequence)
        {
            Movement();
        }
    }

    //update animator states
    private void LateUpdate()
    {
        UpdateAnimator();
    }

    //enable input actions for the player and set the state of the player
    private void OnEnable()
    {
        switch (state)
        {
            case State.Blob:
                anim.SetTrigger("change");
                break;
            case State.Figure:
                break;
        }
        inputActions.Enable();
        inputActions.Player.Sprint.performed += Run;
        inputActions.Player.Sprint.canceled += Run;

        inputActions.Player.Interact.performed += Interact;

        inputActions.Player.Jump.performed += Jump;
        inputActions.Player.Jump.canceled += Jump;

        inputActions.Player.Crouch.performed += Crouch;
        inputActions.Player.Crouch.canceled += Crouch;

        inputActions.Player.Change.performed += Change;
    }

    //disable input actions for the player and playerfootsteps instance on disable
    private void OnDisable()
    {
        playerFootsteps.stop(STOP_MODE.ALLOWFADEOUT);

        inputActions.Disable();
        inputActions.Player.Sprint.performed -= Run;
        inputActions.Player.Sprint.canceled -= Run;

        inputActions.Player.Interact.performed -= Interact;

        inputActions.Player.Jump.performed -= Jump;
        inputActions.Player.Jump.canceled -= Jump;

        inputActions.Player.Crouch.performed -= Crouch;
        inputActions.Player.Crouch.canceled -= Crouch;

        inputActions.Player.Change.performed -= Change;
    }

    #endregion

    #region Own Methods/Functions

    /// <summary>
    /// if player moves against a wall, set the velocity of y to wallslide speed and wallsliding to true
    /// </summary>
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

    /// <summary>
    /// if ledge is detected set the offsets for the position of the player to current movement direction
    /// set the player to the position for ledgeclimb and start animation
    /// </summary>
    private void CheckForLedge()
    {
        if (ledgeDetected && canGrabLedge && !changeCooldown)
        {
            canGrabLedge = false;
            rb.velocity = new Vector2(0, 0);

            Vector2 ledgePosition = climbPosition.position + climbPositionoffset;
            switch (state)
            {
                case State.Blob:
                    if (!leftMovement)
                    {
                        climbBegunPosition = ledgePosition + offset1;
                        climbOverPosition = ledgePosition + offsetBlob;
                    }
                    else if (leftMovement)
                    {
                        climbBegunPosition = ledgePosition - offset3;
                        climbOverPosition = ledgePosition - offsetBlob;
                    }
                    break;
                case State.Figure:
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
                    break;
            }

            canClimbLedge = true;
        }

        if (canClimbLedge)
        {
            switch (state)
            {
                case State.Blob:
                    anim.SetBool("LedgeBlob", true);
                    break;
                case State.Figure:
                    anim.SetBool("canClimb", true);
                    anim.SetTrigger("LedgeFigure");
                    break;
            }
            rb.velocity = new Vector2(0, 0);
            transform.position = climbBegunPosition;
        }
    }
    /// <summary>
    /// at the end of ledgeclimb animation set the player position to new position on the platform and set canClimbLedge to false
    /// </summary>
    public void LedgeClimbOver()
    {
        canClimbLedge = false;
        anim.SetBool("canClimb", false);
        anim.SetBool("LedgeBlob", false);
        transform.position = climbOverPosition;
        Invoke("AllowLedgeGrab", 0.1f);
    }

    /// <summary>
    /// set canGrabLedge to true
    /// </summary>
    private void AllowLedgeGrab()
    {
        canGrabLedge = true;
    }

    /// <summary>
    /// enables the ability to change between states
    /// </summary>
    public void EnableChangeState()
    {
        canChangeState = true;
    }

    /// <summary>
    /// sets inSequence to true and stops the player movement
    /// </summary>
    public void EnterSequence()
    {
        inSequence = true;
        rb.velocity = new Vector2(0, 0);
        lastMovement = new Vector2(0, 0);
        lifebarAnim.SetTrigger("Offscreen");
        changeToFigureButton.GetComponent<Button>().interactable = false;
        changeToBlobButton.GetComponent<Button>().interactable = false;
        StartCoroutine(ExitSequence());
    }

    /// <summary>
    /// wait for 2 seconds and set inSequence to false
    /// </summary>
    /// <returns>wait for 2 seconds</returns>
    private IEnumerator ExitSequence()
    {
        camZoomOut.Priority = 12;
        yield return new WaitForSeconds(2f);
        AudioManager.instance.SetMusicArea(levelMusic);
        yield return new WaitForSeconds(5f);
        camZoomOut.Priority = 8;
        yield return new WaitForSeconds(1.6f);
        changeToFigureButton.GetComponent<Button>().interactable = true;
        changeToBlobButton.GetComponent<Button>().interactable = true;
        lifebarAnim.SetTrigger("Onscreen");
        inSequence = false;
    }

    private IEnumerator ResetPlayer()
    {
        yield return new WaitForSeconds(1f);
        transform.position = deathZone.spawnPoint[deathZone.spawnIndex].position;
    }

    /// <summary>
    /// set the dying animation for current player state and start Game Over Coroutine
    /// </summary>
    /// <returns>wait for 0.3 seconds</returns>
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

    /// <summary>
    /// set camera follow to null and disable collison if current state of the player is Figure
    /// after 0.8 seconds reload Scene
    /// </summary>
    /// <returns>wait for 0.8 seconds</returns>
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
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// set the bool inSmallPassage true if it's currently false and false if it's currently true
    /// </summary>
    public void StayCrouched()
    {
        inSmallPassage = inSmallPassage ? false : true;
    }

    #endregion

    #region Collision Functions

    //Get collision with collidern of other object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if player is on a onewayplattform get the object and sets to current oneway platform
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Obstacles"))
        {
            //if the player hits an enemy reduce lifepoints and start hurt animation
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

                health.ReduceHealth(1);
                
                if(health.health == 0)
                {
                    panelAnim.SetTrigger("Blend");
                    StartCoroutine(ResetPlayer());
                    logic.ReduceScore(1);

                    if (logic.lifePoints == 0)
                    {
                        rb.velocity = new Vector2(0, 0);
                        lastMovement = new Vector2(0, 0);
                        isDying = true;
                        StartCoroutine(Dying());
                    }
                }
            } 
            //if the player jumps on the enemy destroy enemy
            else if (isKilling)
            {
                collision.gameObject.GetComponent<TestingEnemy>().DestroyEnemy();
            }
           
        }
    }

    //get when the player exit an collision with another collider
    private void OnCollisionExit2D(Collision2D collision)
    {
        //set current oneway platform to null if player leaves the platform
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            health.ReduceHealth(1);

            if(health.health == 0)
            {
                logic.ReduceScore(1);

                if(logic.lifePoints == 0)
                {
                    rb.velocity = new Vector2(0, 0);
                    lastMovement = new Vector2(0, 0);
                    isDying = true;
                    StartCoroutine(Dying());
                }
            }
        }

        if (collision.gameObject.CompareTag("ClimbingWall"))
        {
            allowClimb = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ClimbingWall"))
        {
            allowClimb = false;
        }
    }

    /// <summary>
    /// ignore the collider of the current oneway platform for 0.25 seconds
    /// </summary>
    /// <returns>wait for 0.25 seconds</returns>
    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(this.col, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(this.col, platformCollider, false);
    }

    #endregion

    #region Input CallbackContext Methods

    /// <summary>
    /// increase movement speed while pressing the run button
    /// </summary>
    /// <param name="context"></param>
    void Run(InputAction.CallbackContext context)
    {
        switch (state)
        {
            case State.Blob:
                break;
            case State.Figure:
                isRunning = context.performed;
                break;
        }
    }

    /// <summary>
    /// checking if the player is pressing the interact button
    /// </summary>
    /// <param name="context"></param>
    void Interact(InputAction.CallbackContext context)
    {
        isInteracting = context.performed;
    }

    /// <summary>
    /// set crouch animation if the player pressing the crouch button
    /// </summary>
    /// <param name="context"></param>
    void Crouch(InputAction.CallbackContext context)
    {
        switch (state)
        {
            case State.Blob:
                if (context.performed && isGrounded && !inSequence)
                {
                    anim.SetTrigger("BlobCrouch");
                }

                if (context.canceled && isGrounded && !inSmallPassage)
                {
                    anim.SetTrigger("BlobCrouchEnd");
                }
                break;
            case State.Figure:
                break;
        }
    }

    /// <summary>
    /// keyboard button for change state of the player
    /// </summary>
    /// <param name="context"></param>
    void Change(InputAction.CallbackContext context)
    {
        if (canChangeState && !changeCooldown && isGrounded && !inSequence && context.performed)
        {
            ChangeState();
        }
    }

    /// <summary>
    /// perform jump if the player is pressing the jump button
    /// </summary>
    /// <param name="context"></param>
    void Jump(InputAction.CallbackContext context)
    {
        if (!changeCooldown && !isShroomed)
        {
            switch (state)
            {
                case State.Blob:
                    break;
                case State.Figure:
                    if (context.performed && isGrounded)
                    {
                        isJumping = true;
                        rb.AddForce(new Vector2(0, FigureJumpPower), ForceMode2D.Impulse);
                        anim.SetTrigger("FigureJump");
                    }
                    if (context.canceled)
                    {
                        isJumping = false;
                        if (rb.velocity.y > 0f)
                        {
                            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                            coyoteTimeCounter = 0f;
                        }
                    }

                    break;
            }
        }
    }

    #endregion

    #region Movement

    /// <summary>
    /// set movement and direction of the player
    /// </summary>
    private void Movement()
    {
        float currentSpeed = lastMovement.magnitude;

        if (moveInput.x > 0)
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
        wallCheckPos.x = leftMovement ? -0.38f : 0.38f;
        climbPositionoffset.x = leftMovement ? -0.387f : 0.387f;

        if ((moveInput.y < 0) && (currentOneWayPlatform != null))
        {
            StartCoroutine(DisableCollision());
        }

        if (isShroomed)
        {
            print("shroom");
            rb.AddForce(new Vector2(rb.velocity.x, shroomJump), ForceMode2D.Impulse);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, 25, 25));
        }

       

        switch (state)
        {
            case State.Blob:
                float targetSpeedBlob = (moveInput.x == 0 ? 0 : movementSpeedBlob * moveInput.magnitude);

                if (Mathf.Abs(currentSpeed - targetSpeedBlob) > 0.01f)
                {
                    currentSpeed = Mathf.Lerp(currentSpeed, targetSpeedBlob, speedChangeRate * Time.deltaTime);
                }
                else
                {
                    currentSpeed = targetSpeedBlob;
                }

                if (isWalled && !changeCooldown && allowClimb && !canClimbLedge)
                {
                    if (climbWall)
                    {
                        anim.SetTrigger("climbing");
                        climbWall = false;
                    }
                  rb.velocity = new Vector2(moveInput.x, climbSpeed * moveInput.y);
                }
                else
                {
                    climbWall = true;
                  rb.velocity = new Vector2(currentSpeed * directionMultiply, rb.velocity.y);
                }
                break;
            case State.Figure:

                float targetSpeedFigure = (moveInput.x == 0 ? 0 : (isRunning ? sprintSpeed : movementSpeedFigure) * moveInput.magnitude);

                if (Mathf.Abs(currentSpeed - targetSpeedFigure) > 0.01f)
                {
                    currentSpeed = Mathf.Lerp(currentSpeed, targetSpeedFigure, speedChangeRate * Time.deltaTime);
                }
                else
                {
                    currentSpeed = targetSpeedFigure;
                }

                rb.velocity = new Vector2(currentSpeed * directionMultiply, rb.velocity.y);
                break;
        }

        //clamps the fall speed of the player
        if(rb.velocity.y < fallClamp && !isGrounded)
        {
            rb.velocity = new Vector2(currentSpeed * directionMultiply, fallClamp);
        }

        lastMovement.x = currentSpeed;
        lastMovementClimb.y = moveInput.y;
    }

    #endregion

    #region Animation

    /// <summary>
    /// update animtor states based on the currentspeed
    /// set animator bools based on the bools in the script
    /// </summary>
    private void UpdateAnimator()
    {
        Vector2 velocity = lastMovement;
        float speed = velocity.magnitude;
        Vector2 velocityClimb = lastMovementClimb;
        float climbSpeed = velocityClimb.magnitude;

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("MovementSpeed", speed);
        anim.SetFloat("ClimbSpeed", climbSpeed);
        anim.SetBool("isWalled", isWalled);
        anim.SetBool("isClimbing", climbingWall);
        if (rb.velocity.y < -1)
        {
            anim.SetBool("fall", true);
        }
        else
        {
            anim.SetBool("fall", false);
        }
    }

    /// <summary>
    /// Change the state of the player
    /// </summary>
    public void ChangeState()
    {
        if (!changeCooldown && isGrounded)
        {
            switch (state)
            {
                case State.Blob: //Change it to State Figure
                    anim.SetTrigger("setState");
                    changeToBlobButton.SetActive(false);
                    changeToFigureButton.SetActive(true);
                    changeToFigureButton.GetComponent<Button>().interactable = false;
                    state = State.Figure;
                    break;
                case State.Figure: // Change it to State Blob
                    changeToBlobButton.SetActive(true);
                    changeToFigureButton.SetActive(false);
                    changeToBlobButton.GetComponent<Button>().interactable = false;
                    state = State.Blob;
                    anim.SetTrigger("change");
                    break;
            }
            changeCooldown = true;
        }
    }

    /// <summary>
    /// set changecooldown to false and buttons interactable to true
    /// </summary>
    public void ChangeCooldown()
    {
        changeCooldown = false;
        changeToFigureButton.GetComponent<Button>().interactable = true;
        changeToBlobButton.GetComponent<Button>().interactable = true;
    }

    #endregion

    #region Sound

    /// <summary>
    /// set the movement sound for the player
    /// </summary>
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
            playerFootsteps.stop(STOP_MODE.IMMEDIATE);
        }
    }

    /// <summary>
    /// set the audio for the Blob jump
    /// </summary>
    public void BlobJump()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.blobJump, this.transform.position);
    }

    #endregion

    /// <summary>
    /// draws a wirecube in unity to visualize the wall and groundcheck
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + groundCheckPos, groundCheckSize);
        Gizmos.DrawWireCube(transform.position + wallCheckPos, wallCheckSize);
    }
}
