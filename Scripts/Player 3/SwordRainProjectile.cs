using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordRainProjectile : MonoBehaviour
{
    [Header("Sword Rain Settings")]
    public int damage = 15;
    public float fallSpeed = 8f;
    public float lifetime = 5f;

    private Rigidbody2D rb;
    private GameObject owner; // Người triệu hồi
    private HashSet<GameObject> hitTargets = new HashSet<GameObject>(); // Danh sách đã đánh

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Tự hủy sau một khoảng thời gian
        Destroy(gameObject, lifetime);

        // Rơi xuống
        if (rb != null)
        {
            rb.velocity = Vector2.down * fallSpeed;
            rb.gravityScale = 0; // Không dùng gravity, dùng velocity cố định
        }
    }

    void Update()
    {
        // Không xoay kiếm - rơi thẳng xuống
    }

    public void Initialize(int dmg, GameObject shooter)
    {
        damage = dmg;
        owner = shooter;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Không đánh chính mình (người triệu hồi)
        if (other.gameObject == owner)
            return;

        // Kiểm tra đã đánh target này chưa
        if (hitTargets.Contains(other.gameObject))
            return; // Đã đánh rồi, bỏ qua

        // Xuyên qua blocks và walls (không hủy khi chạm)
        // Mỗi thanh kiếm có thể đánh nhiều target khác nhau

        // Kiểm tra xem có phải player không (thử tất cả các loại PlayerHealth)
        PlayerHealth targetHealth = other.GetComponent<PlayerHealth>();
        PlayerHealth3 targetHealth3 = other.GetComponent<PlayerHealth3>();
        PlayerHealth4 targetHealth4 = other.GetComponent<PlayerHealth4>();

        // Gây damage và đánh dấu đã đánh target này
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage);
            hitTargets.Add(other.gameObject); // Đánh dấu đã đánh
        }
        else if (targetHealth3 != null)
        {
            targetHealth3.TakeDamage(damage);
            hitTargets.Add(other.gameObject);
        }
        else if (targetHealth4 != null)
        {
            targetHealth4.TakeDamage(damage);
            hitTargets.Add(other.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Xuyên qua tất cả (không hủy khi chạm tường/sàn)
        // Chỉ tự hủy sau lifetime
    }
}
