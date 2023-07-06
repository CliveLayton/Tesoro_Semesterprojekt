using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// rigidbody of the enemy
    /// </summary>
    private Rigidbody2D rb;

    /// <summary>
    /// animator of the enemy
    /// </summary>
    private Animator enemyAnim;

    /// <summary>
    /// position points for the enemy to travel 
    /// </summary>
    public Transform posA, posB;

    /// <summary>
    /// the target position the enemy goes to
    /// </summary>
    private Vector3 targetPos;

    /// <summary>
    /// the move direction of the enemy
    /// </summary>
    private Vector3 moveDirection;

    /// <summary>
    /// the move speed of the enemy
    /// </summary>
    private float moveSpeed = 1f;

    /// <summary>
    /// Event emitter from FMOD for enemy footsteps
    /// </summary>
    private StudioEventEmitter emitter;

    #endregion

    #region Unity Event Functions
    /// <summary>
    /// get components and set healthbar to max health
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
    }

    /// <summary>
    /// set the emitter audio and play it, set the begin target position for the enemy and calculate the direction
    /// </summary>
    private void Start()
    {
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.enemyFootsteps, this.gameObject);
        emitter.Play();
        targetPos = posB.position;
        transform.eulerAngles = new Vector3(0, 180, 0);
        DirectionCalculate();
    }

    /// <summary>
    /// sets the target position new if the enemy reaches current target position
    /// </summary>
    private void Update()
    {
        if (Vector2.Distance(transform.position, posA.position) < 0.05f)
        {
            targetPos = posB.position;
            transform.eulerAngles = new Vector3(0, 180, 0);
            DirectionCalculate();
        }

        if (Vector2.Distance(transform.position, posB.position) < 0.05f)
        {
            targetPos = posA.position;
            transform.eulerAngles = new Vector3(0, 0, 0);
            DirectionCalculate();
        }
    }

    /// <summary>
    /// set velocity of the player to the movedirection times the speed
    /// </summary>
    private void FixedUpdate()
    {
        rb.velocity = moveDirection * moveSpeed;
    }

    #endregion

    #region Own Functions
    /// <summary>
    /// calculate the direction the enemy moves
    /// </summary>
    private void DirectionCalculate()
    {
        moveDirection = (targetPos - transform.position).normalized;
    }

    /// <summary>
    /// stops the movement of the enemy and plays audio for enemy hit
    /// </summary>
    public void GetDamage()
    {
        enemyAnim.SetTrigger("Dying");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.enemyGetHit, this.transform.position);
        moveSpeed = 0f;
        emitter.Stop();
        StartCoroutine(EnemyDying());
    }

    /// <summary>
    /// destroys the object
    /// </summary>
    /// <returns>wait for 0.2 seconds</returns>
    private IEnumerator EnemyDying()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
    }

    #endregion
}
