using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f; // tốc độ di chuyển
    public float jumpForce = 5f; // lực nhảy
    public bool isPlayer1 = true; // phân biệt P1 / P2

    [Header("Combat")]
    public GameObject hitbox; // Hitbox của đòn đánh
    public float attackDuration = 0.2f; // thời gian hitbox active
    public int damage = 10; // sát thương cơ bản

    [Header("References")]
    public PlayerHealth opponentHealth; // tham chiếu tới Player khác

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded = true;

    private Hitbox hitboxScript;
    private Collider2D hitboxCollider;

    [Header("Mana")]
    public PlayerMana mana;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        mana = GetComponent<PlayerMana>();

        if (hitbox != null)
        {
            hitboxScript = hitbox.GetComponent<Hitbox>();
            hitboxCollider = hitbox.GetComponent<Collider2D>();
            hitbox.SetActive(false); // tắt hitbox lúc bắt đầu
        }
    }

    void Update()
    {
        float move = 0f;

        // Di chuyển + nhảy
        if (isPlayer1)
        {
            if (Input.GetKey(KeyCode.A))
                move = -1f;
            if (Input.GetKey(KeyCode.D))
                move = 1f;
            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
                Jump();

            if (Input.GetKeyDown(KeyCode.J))
                Attack();
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                move = -1f;
            if (Input.GetKey(KeyCode.RightArrow))
                move = 1f;
            if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
                Jump();

            if (Input.GetKeyDown(KeyCode.Alpha1))
                Attack();
        }

        // Áp vận tốc di chuyển
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        // Animation
        anim.SetFloat("Speed", Mathf.Abs(move));
        anim.SetBool("IsJumping", !isGrounded);

        if (move > 0)
            transform.localScale = new Vector3(1, 1, 1);
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
        // Chạm mặt đất → cho phép nhảy lại
        if (col.contacts[0].normal.y > 0.5f)
            isGrounded = true;
    }

    void Attack()
    {
        if (!mana.UseMana(20))
            return;

        if (hitbox != null)
        {
            if (hitboxScript != null)
            {
                hitboxScript.ResetDamageStatus();
            }
            if (hitboxCollider != null)
            {
                hitboxCollider.enabled = false;
            }

            hitbox.SetActive(true);

            if (hitboxCollider != null)
            {
                hitboxCollider.enabled = true;
            }
            anim.SetTrigger("BasicAttack");
            StartCoroutine(DisableHitbox());
        }
    }

    IEnumerator DisableHitbox()
    {
        yield return new WaitForSeconds(attackDuration);
        hitbox.SetActive(false);
    }
}
