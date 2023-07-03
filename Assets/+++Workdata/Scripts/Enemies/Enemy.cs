using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// rigidbody of the enemy
    /// </summary>
    private Rigidbody2D rb;

    /// <summary>
    /// healthbar of the enemy
    /// </summary>
    public Image healthBar;

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
    /// the max health of the enemy
    /// </summary>
    public int maxHealth;

    /// <summary>
    /// the current health of the enemy
    /// </summary>
    private int currentHealth;

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
        currentHealth = maxHealth;
        SetHealth();
        healthBar.color = Color.green;
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
    /// destroy the object and stops the emitter
    /// </summary>
    private void EnemyDying()
    {
        emitter.Stop();
        Destroy(this.gameObject);
    }

    /// <summary>
    /// dívide the damage from current health and sets healths
    /// stops the movement of the enemy for a short time and plays a audio for enemy hit
    /// </summary>
    /// <param name="damage">damage to divide from current health</param>
    public void GetDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(StopMovement());
        AudioManager.instance.PlayOneShot(FMODEvents.instance.enemyGetHit, this.transform.position);
        SetHealth();
    }

    /// <summary>
    /// sets the current health of the enemy to the fill amount of the healthbar
    /// if the healthbar is under 50% set the color to red and if current health is 0 destroy the object
    /// </summary>
    public void SetHealth()
    {
        healthBar.fillAmount = (float)currentHealth / (float)maxHealth;
        if (healthBar.fillAmount <= 0.4f)
        {
            healthBar.color = Color.red;
        }
        if (currentHealth <= 0)
        {
            EnemyDying();
        }
    }

    /// <summary>
    /// stops the movement of the enemy and set it back to 1
    /// </summary>
    /// <returns>wait for 0.5 seconds</returns>
    private IEnumerator StopMovement()
    {
        moveSpeed = 0f;
        yield return new WaitForSeconds(0.5f);
        moveSpeed = 1f;
    }

    #endregion
}
