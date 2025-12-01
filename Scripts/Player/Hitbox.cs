using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage = 10;

    private bool hasDealtDamage = false;

    public void ResetDamageStatus()
    {
        hasDealtDamage = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!hasDealtDamage)
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();

            if (ph != null)
            {
                ph.TakeDamage(damage);
                hasDealtDamage = true;
            }
        }
    }
}
