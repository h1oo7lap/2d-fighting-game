using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox3 : MonoBehaviour
{
    public int damage = 10;

    private bool hasDealtDamage = false;

    public void ResetDamageStatus()
    {
        hasDealtDamage = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasDealtDamage)
        {
            // Thử tìm bất kỳ loại PlayerHealth nào
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            PlayerHealth3 ph3 = other.GetComponent<PlayerHealth3>();
            PlayerHealth4 ph4 = other.GetComponent<PlayerHealth4>();

            if (ph != null)
            {
                ph.TakeDamage(damage);
                hasDealtDamage = true;
            }
            else if (ph3 != null)
            {
                ph3.TakeDamage(damage);
                hasDealtDamage = true;
            }
            else if (ph4 != null)
            {
                ph4.TakeDamage(damage);
                hasDealtDamage = true;
            }
        }
    }
}
