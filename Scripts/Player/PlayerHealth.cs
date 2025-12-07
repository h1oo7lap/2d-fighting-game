using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHP = 100;
    public int currentHP;
    public Slider hpSlider;

    [Header("Animation & State")]
    private Animator anim;
    private bool isDead = false;
    private bool isInvincible = false;
    public float invincibilityDuration = 0.5f; // Thời gian bất tử sau khi bị đánh
    public float dieAnimationDuration = 1.5f; // Thời gian animation die (điều chỉnh theo animation thực tế)

    // Event để thông báo khi nhân vật chết hoàn toàn (sau animation)
    public System.Action OnDeathComplete;

    void Awake()
    {
        currentHP = maxHP;
        anim = GetComponent<Animator>();
        
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }
    }

    public void TakeDamage(int dmg)
    {
        // Không nhận sát thương nếu đã chết hoặc đang trong trạng thái bất tử
        if (isDead || isInvincible)
            return;

        currentHP -= dmg;
        if (currentHP < 0)
            currentHP = 0;

        // Cập nhật HP UI
        if (hpSlider != null)
            hpSlider.value = currentHP;

        Debug.Log(gameObject.name + " HP: " + currentHP);

        // Kiểm tra nếu chết
        if (currentHP <= 0)
        {
            Die();
        }
        else
        {
            // Chơi animation hurt
            PlayHurtAnimation();
            
            // Play hurt sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayHurtSound();
            
            // Kích hoạt invincibility frames
            StartCoroutine(InvincibilityFrames());
        }
    }

    void PlayHurtAnimation()
    {
        if (anim != null)
        {
            anim.SetTrigger("Hurt");
        }
    }

    void Die()
    {
        if (isDead)
            return;

        isDead = true;
        Debug.Log(gameObject.name + " đã chết!");
        
        // Play death sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayDeathSound();

        // Chơi animation die
        if (anim != null)
        {
            anim.SetTrigger("Die");
        }

        // Bắt đầu coroutine xử lý sau khi animation die kết thúc
        StartCoroutine(HandleDeathSequence());
    }

    IEnumerator HandleDeathSequence()
    {
        // Đợi animation die chơi xong
        yield return new WaitForSeconds(dieAnimationDuration);

        // Vô hiệu hóa các component
        DisablePlayerComponents();

        // Thông báo cho BattleManager (hoặc các script khác) rằng đã chết hoàn toàn
        OnDeathComplete?.Invoke();

        Debug.Log(gameObject.name + " animation die đã kết thúc!");
    }

    void DisablePlayerComponents()
    {
        // Vô hiệu hóa controller để không thể di chuyển
        PlayerController controller = GetComponent<PlayerController>();
        if (controller != null)
            controller.enabled = false;

        // Vô hiệu hóa rigidbody để không bị ảnh hưởng bởi vật lý
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        // Vô hiệu hóa collider để không va chạm
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;
    }

    IEnumerator InvincibilityFrames()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    // Getter để các script khác kiểm tra trạng thái
    public bool IsDead()
    {
        return isDead;
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }

    // Hàm để hồi máu (nếu cần)
    public void Heal(int amount)
    {
        if (isDead)
            return;

        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;

        if (hpSlider != null)
            hpSlider.value = currentHP;

        Debug.Log(gameObject.name + " hồi máu: " + amount + ", HP hiện tại: " + currentHP);
    }
}
