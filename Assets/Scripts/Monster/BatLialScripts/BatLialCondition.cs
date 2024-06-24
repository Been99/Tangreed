using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BatLialCondition : MonoBehaviour
{
    private HealthSystem healthSystem;

    public Slider hpSlider;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        healthSystem.OnDamageEvent += UpdateHealthUI;
    }

    private void UpdateHealthUI()
    {
        if (healthSystem.maxHP > 0)
        {
            hpSlider.value = (float)healthSystem.currentHP / healthSystem.maxHP;
        }
        else
        {
            hpSlider.value = 0;
        }
    }
}