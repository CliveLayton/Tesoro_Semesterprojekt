using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;

    public Image healthBar;

    public Transform posA, posB;

    private Vector3 targetPos;
    private Vector3 moveDirection;

    private float moveSpeed = 1f;

    public int maxHealth;
    private int currentHealth;

    private StudioEventEmitter emitter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        SetHealth();
        healthBar.color = Color.green;
    }

    private void Start()
    {
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.enemyFootsteps, this.gameObject);
        emitter.Play();
        targetPos = posB.position;
        transform.eulerAngles = new Vector3(0, 180, 0);
        DirectionCalculate();
    }

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

    private void FixedUpdate()
    {
        rb.velocity = moveDirection * moveSpeed;
    }

    private void DirectionCalculate()
    {
        moveDirection = (targetPos - transform.position).normalized;
    }

    private void EnemyDying()
    {
        emitter.Stop();
        Destroy(this.gameObject);
    }

    public void GetDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(StopMovement());
        AudioManager.instance.PlayOneShot(FMODEvents.instance.enemyGetHit, this.transform.position);
        SetHealth();
    }
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

    private IEnumerator StopMovement()
    {
        moveSpeed = 0f;
        yield return new WaitForSeconds(0.5f);
        moveSpeed = 1f;
    }
}
