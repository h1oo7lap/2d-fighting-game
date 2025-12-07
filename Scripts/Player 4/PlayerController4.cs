using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController4 : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public bool isPlayer1 = true;

    [Header("Skill J - Bắn Chưởng (Projectile)")]
    public GameObject projectilePrefab; // Prefab projectile
    public int projectileDamage = 20;
    public int projectileManaCost = 30;
    public float projectileSpeed = 10f;
    public Vector3 projectileSpawnOffset = new Vector3(1f, 0f, 0f);

    [Header("Skill K - Miễn Thương (Shield)")]
    public int shieldManaCost = 50;
    public float shieldDuration = 2f;
    public float shieldCooldown = 5f;
    private bool isShieldOnCooldown = false;

    [Header("Skill L - Hồi Máu (Heal)")]
    public int healManaCost = 40;
    public int healAmount = 30;


    [Header("Mana")]
    public PlayerMana4 mana;

    [Header("Health")]
    private PlayerHealth4 health;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        mana = GetComponent<PlayerMana4>();
        health = GetComponent<PlayerHealth4>();
    }

    void Update()
    {
        // Không cho phép hành động nếu đã chết
        if (health != null && health.IsDead())
            return;

        float move = 0f;

        if (isPlayer1)
        {
            if (Input.GetKey(KeyCode.A))
                move = -1;
            if (Input.GetKey(KeyCode.D))
                move = 1;
            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
                Jump();

            if (Input.GetKeyDown(KeyCode.J))
                Attack();
            if (Input.GetKeyDown(KeyCode.K))
                SkillK();
            if (Input.GetKeyDown(KeyCode.L))
                HealSkill(); // Changed from ShootProjectile
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                move = -1;
            if (Input.GetKey(KeyCode.RightArrow))
                move = 1;
            if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
                Jump();

            if (Input.GetKeyDown(KeyCode.Alpha1))
                Attack();
            if (Input.GetKeyDown(KeyCode.Alpha2))
                SkillK();
            if (Input.GetKeyDown(KeyCode.Alpha3))
                HealSkill(); // Changed from ShootProjectile
        }

        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        anim.SetFloat("Speed", Mathf.Abs(move));
        anim.SetBool("IsJumping", !isGrounded);

        if (move > 0)
            transform.localScale = Vector3.one;
        else if (move < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.contacts[0].normal.y > 0.5f)
            isGrounded = true;
    }

    void Attack()
    {
        // Kiểm tra mana
        if (mana == null || !mana.UseMana(projectileManaCost))
            return;

        // Kiểm tra có prefab không
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Projectile Prefab chưa được gán!");
            return;
        }

        // Xác định hướng bắn
        float direction = transform.localScale.x > 0 ? 1 : -1;
        Vector2 shootDirection = new Vector2(direction, 0);

        // Tính vị trí spawn projectile
        Vector3 spawnPosition = transform.position + new Vector3(
            projectileSpawnOffset.x * direction,
            projectileSpawnOffset.y,
            projectileSpawnOffset.z
        );

        // Tạo projectile
        GameObject proj = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        // Khởi tạo projectile
        Projectile4 projScript = proj.GetComponent<Projectile4>();
        if (projScript != null)
        {
            projScript.damage = projectileDamage;
            projScript.speed = projectileSpeed;
            projScript.Initialize(shootDirection, gameObject);
        }

        // Animation
        if (anim != null)
            anim.SetTrigger("SkillJ");

        // Sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayAttackSound();

        Debug.Log(gameObject.name + " bắn chưởng!");
    }

    void SkillK()
    {
        // Kiểm tra mana
        if (mana == null || !mana.UseMana(shieldManaCost))
            return;

        // Kiểm tra cooldown
        if (isShieldOnCooldown)
        {
            Debug.Log(gameObject.name + " - Shield đang cooldown!");
            return;
        }

        // Kích hoạt shield
        StartCoroutine(ActivateShield());
    }

    IEnumerator ActivateShield()
    {
        // Activate shield
        if (health != null)
            health.SetInvincible(true);

        // Animation
        if (anim != null)
            anim.SetTrigger("SkillK");

        // Sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayAttackSound();

        Debug.Log(gameObject.name + " kích hoạt Miễn Thương!");

        // Shield duration
        yield return new WaitForSeconds(shieldDuration);

        // Deactivate shield
        if (health != null)
            health.SetInvincible(false);

        Debug.Log(gameObject.name + " - Shield hết hiệu lực!");

        // Start cooldown
        isShieldOnCooldown = true;
        yield return new WaitForSeconds(shieldCooldown);
        isShieldOnCooldown = false;

        Debug.Log(gameObject.name + " - Shield ready!");
    }

    void HealSkill()
    {
        // Kiểm tra mana
        if (mana == null || !mana.UseMana(healManaCost))
            return;

        // Hồi máu
        if (health != null)
        {
            health.Heal(healAmount);

            // Animation
            if (anim != null)
                anim.SetTrigger("SkillL");

            // Sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayAttackSound();

            Debug.Log(gameObject.name + " hồi " + healAmount + " HP!");
        }
    }
}
