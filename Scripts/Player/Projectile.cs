using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public int damage = 20;
    public float speed = 10f;
    public float lifetime = 5f;

    [Header("Rotation Settings")]
    [Tooltip("Bật/tắt xoay projectile khi bay")]
    public bool enableRotation = true;

    [Tooltip("Tốc độ xoay (độ/giây)")]
    public float rotationSpeed = 360f; // 360 độ/giây = 1 vòng/giây

    private Rigidbody2D rb;
    private bool hasHit = false;
    private GameObject owner; // người bắn ra để không tự đánh mình
    private float initialAngle; // góc ban đầu theo hướng bay

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Tự hủy sau một khoảng thời gian
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Xoay projectile nếu bật tính năng
        if (enableRotation)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    public void Initialize(Vector2 direction, GameObject shooter)
    {
        owner = shooter;

        // Di chuyển theo hướng
        if (rb != null)
        {
            rb.velocity = direction.normalized * speed;
        }

        // Xoay projectile theo hướng bay (nếu không bật rotation)
        if (!enableRotation)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            initialAngle = angle;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Đã đánh trúng rồi thì không đánh nữa
        if (hasHit)
            return;

        // Không đánh chính mình
        if (other.gameObject == owner)
            return;

        // Nếu trúng block → hủy luôn
        if (other.CompareTag("BlockProjectile"))
        {
            Destroy(gameObject);
            return;
        }

        // Kiểm tra xem có phải player không (thử tất cả các loại PlayerHealth)
        PlayerHealth targetHealth = other.GetComponent<PlayerHealth>();
        PlayerHealth3 targetHealth3 = other.GetComponent<PlayerHealth3>();
        PlayerHealth4 targetHealth4 = other.GetComponent<PlayerHealth4>();

        if (targetHealth != null)
        {
            // Gây damage
            targetHealth.TakeDamage(damage);
            hasHit = true;

            // Hủy projectile sau khi đánh trúng
            Destroy(gameObject);
        }
        else if (targetHealth3 != null)
        {
            targetHealth3.TakeDamage(damage);
            hasHit = true;
            Destroy(gameObject);
        }
        else if (targetHealth4 != null)
        {
            targetHealth4.TakeDamage(damage);
            hasHit = true;
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Nếu chạm vào tường hoặc sàn thì cũng hủy
        if (
            collision.gameObject.CompareTag("BlockProjectile")
            || collision.gameObject.CompareTag("Wall")
        )
        {
            Destroy(gameObject);
        }
    }
}
