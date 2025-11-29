using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth ph = other.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(damage);
        }
    }
}
