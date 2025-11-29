using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    public Slider hpSlider;

    void Awake()
    {
        currentHP = maxHP;
        if (hpSlider != null)
            hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if (currentHP < 0)
            currentHP = 0;
        if (hpSlider != null)
            hpSlider.value = currentHP;
        Debug.Log(gameObject.name + " HP: " + currentHP);
    }
}
