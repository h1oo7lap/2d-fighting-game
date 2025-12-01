using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour
{
    public int maxMana = 100;
    public int currentMana;

    public Slider manaSlider;

    [Header("Regen")]
    public int manaRegenAmount = 10; // hồi mỗi lần
    public float manaRegenInterval = 1.0f; // mỗi bao lâu hồi
    private float regenTimer = 0f; // bao lâu hồi hành một lần

    void Awake()
    {
        currentMana = maxMana;
        if (manaSlider != null)
        {
            manaSlider.maxValue = maxMana;
            manaSlider.value = currentMana;
        }
    }

    public bool UseMana(int amount)
    {
        if (currentMana < amount)
            return false;

        currentMana -= amount;
        manaSlider.value = currentMana;

        return true;
    }

    public void AddMana(int amount)
    {
        currentMana += amount;
        if (currentMana > maxMana)
            currentMana = maxMana;

        manaSlider.value = currentMana;
    }

    void Update()
    {
        regenTimer += Time.deltaTime;

        if (regenTimer >= manaRegenInterval)
        {
            AddMana(manaRegenAmount);
            regenTimer = 0f;
        }
    }
}
