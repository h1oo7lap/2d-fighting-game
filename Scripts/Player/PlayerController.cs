using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public bool isPlayer1 = true;

    [Header("Skill J")]
    public GameObject hitbox;
    public float attackDuration = 0.2f;
    public int damage = 10; // damage đánh thường

    [Header("Skill K")]
    public int skillKDamage = 30;
    public float skillKDuration = 0.3f;
    public int skillKManaCost = 50;
    public Vector3 skillKSize = new Vector3(2f, 2f, 1f);
    public float skillKForwardOffset = 0.5f;

    [Header("Skill L - Projectile")]
    public GameObject projectilePrefab; // Prefab của viên đạn chưởng
    public int projectileDamage = 20;
    public int projectileManaCost = 30;
    public float projectileSpeed = 10f;
    public Vector3 projectileSpawnOffset = new Vector3(1f, 0f, 0f); // Vị trí spawn projectile

    private Vector3 originalHitboxScale;
    private Vector3 originalHitboxPosition;

    [Header("Mana")]
    public PlayerMana mana;

    [Header("Health")]
    private PlayerHealth health;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded = true;

    private Hitbox hitboxScript;
    private Collider2D hitboxCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        mana = GetComponent<PlayerMana>();
        health = GetComponent<PlayerHealth>();

        if (hitbox != null)
        {
            hitboxScript = hitbox.GetComponent<Hitbox>();
            hitboxCollider = hitbox.GetComponent<Collider2D>();
            hitbox.SetActive(false);

            originalHitboxScale = hitbox.transform.localScale;
            originalHitboxPosition = hitbox.transform.localPosition;
        }
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
                ShootProjectile();
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
                ShootProjectile();
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
        if (!mana.UseMana(20))
            return;

        if (hitboxScript != null)
            hitboxScript.damage = damage;

        hitbox.transform.localScale = originalHitboxScale;
        hitbox.transform.localPosition = originalHitboxPosition;

        DoHitboxAttack("BasicAttack", attackDuration);
        
        // Play attack sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayAttackSound();
    }

    void SkillK()
    {
        if (!mana.UseMana(skillKManaCost))
            return;

        if (hitboxScript != null)
            hitboxScript.damage = skillKDamage;

        hitbox.transform.localScale = skillKSize;

        float dir = transform.localScale.x > 0 ? 1 : -1;

        hitbox.transform.localPosition = new Vector3(
            originalHitboxPosition.x + skillKForwardOffset * dir,
            originalHitboxPosition.y,
            originalHitboxPosition.z
        );

        DoHitboxAttack("SkillK", skillKDuration);
        
        // Play attack sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayAttackSound();
    }

    void DoHitboxAttack(string animationName, float duration)
    {
        if (hitboxScript != null)
            hitboxScript.ResetDamageStatus();

        hitboxCollider.enabled = false;
        hitbox.SetActive(true);
        hitboxCollider.enabled = true;

        anim.SetTrigger(animationName);

        StartCoroutine(DisableHitbox(duration));
    }

    IEnumerator DisableHitbox(float duration)
    {
        yield return new WaitForSeconds(duration);
        hitbox.SetActive(false);

        hitbox.transform.localScale = originalHitboxScale;
        hitbox.transform.localPosition = originalHitboxPosition;
    }

    void ShootProjectile()
    {
        // Kiểm tra mana
        if (!mana.UseMana(projectileManaCost))
            return;

        // Kiểm tra có prefab không
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Projectile Prefab chưa được gán!");
            return;
        }

        // Xác định hướng bắn (dựa vào hướng nhân vật đang quay)
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
        Projectile projScript = proj.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.damage = projectileDamage;
            projScript.speed = projectileSpeed;
            projScript.Initialize(shootDirection, gameObject);
        }

        // Chơi animation (nếu có)
        if (anim != null)
        {
            anim.SetTrigger("ShootProjectile");
        }
        
        // Play attack sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayAttackSound();

        Debug.Log(gameObject.name + " bắn chưởng!");
    }
}
