using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BatLialCondition : MonoBehaviour
{
    private HealthSystem healthSystem;

    public Slider hpSlider;

    public int decayAmount = 19; // 1초마다 닳을 체력 양

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        healthSystem.OnDamageEvent += UpdateHealthUI;
        StartCoroutine(HealthDecayCoroutine());
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

    private IEnumerator HealthDecayCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 1초 대기
            healthSystem.ChangeHealth(-decayAmount); // 체력 감소
        }
    }
}