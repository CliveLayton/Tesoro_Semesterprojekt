using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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

    [SerializeField] private Vector3 wallCheckPos;
    [SerializeField] private Vector2 wallCheckSize;

    [Header("Legde Info")]
    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;
    [SerializeField] private Vector2 offset3;

    public Sprite change;

    #endregion

    #region Input Variables

    private GameInput inputActions;
    private InputAction moveAction;

    #endregion

    #region Private Variables

    private Rigidbody2D rb;

    private Animator anim;

    private SpriteRenderer sr;

    private BoxCollider2D col;

    private GameObject currentOneWayPlatform;

    public bool isRunning, isInteracting, isJumping, isWallSliding, isDying, leftMovement;
    public bool isGrounded, isWalled;
    public bool ledgeDetected, canClimb;
    private bool canGrabLedge = true;

    private Vector2 climbBegunPosition;
    private Vector2 climbOverPosition;

    private int directionMultiply;
    public float lastMoveSpeed;

    private Vector2 lastMovement;

    private Vector2 moveInput;

    #endregion

    #region Unity Event Functions

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();

        inputActions = new GameInput();
        moveAction = inputActions.Player.Move;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(transform.position + groundCheckPos, groundCheckSize, 0, groundLayer);
        isWalled = Physics2D.OverlapBox(transform.position + wallCheckPos, wallCheckSize, 0, groundLayer);

        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }

        if(!canClimb && !isDying)
        {
            Movement();
        }
        CheckForLedge();
        WallSlide();
    }

    private void LateUpdate()
    {
        
    }

    private void WallSlide()
    {
        if (isWalled && !isGrounded && !canClimb && moveInput.x != 0f)
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

            canClimb = true;
        }

        if (canClimb)
        {
            rb.velocity = new Vector2(0, 0);
            transform.position = climbBegunPosition;
        }
    }

    public void LedgeClimbOver()
    {
        canClimb = false;
        transform.position = climbOverPosition;
        Invoke("AllowLedgeGrab", 0.1f);
    }

    private void AllowLedgeGrab()
    {
        canGrabLedge = true;
    }

    private void OnEnable()
    {
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
        if(context.performed && isGrounded && !isDying)
        {
            rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
        }
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
            transform.eulerAngles = new Vector3(0, 0, 0);
            leftMovement = false;
        }
        else if (moveInput.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            leftMovement = true;
        }

        directionMultiply = leftMovement ? -1 : 1;

        if((moveInput.y < 0) && (currentOneWayPlatform != null))
        {
            StartCoroutine(DisableCollision());
        }

        if (!isWalled)
        {
            rb.velocity = new Vector2(currentSpeed * directionMultiply, rb.velocity.y);
        }
        else if ((isWalled) && (currentOneWayPlatform = null))
        {
            rb.velocity = new Vector2(currentSpeed * directionMultiply, climbSpeed * moveInput.y); ;
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
    }

    public void EndDying()
    {
        isDying = false;
    }

    public void ChangeSprite()
    {
        sr.sprite = change;
    }

    #endregion

    #region Sound

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + groundCheckPos, groundCheckSize);
        Gizmos.DrawWireCube(transform.position + wallCheckPos, wallCheckSize);
    }
}
