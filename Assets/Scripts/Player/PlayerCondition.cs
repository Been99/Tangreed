using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCondition : MonoBehaviour
{
    private Animator animator;
    private HealthSystem healthSystem;

    [SerializeField] private TMP_Text playerHPText;
    [SerializeField] private Slider playerHPBar;
    [SerializeField] private GameObject gameOverPanel;

    public GameObject shardPrefab;
    public int shardCount = 25;
    public float shardMinSpeed = 3f;
    public float shardMaxSpeed = 8f;
    public float shardMaxTorque = 10f;
    public float shardDuration = 4f;

    private static readonly int deadTriggerHash = Animator.StringToHash("dead");

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDamageEvent += UpdateHealthUI;
        healthSystem.OnDeathEvent += PlayerDeath;
    }

    private void Update()
    {
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthSystem.maxHP > 0)
        {
            playerHPText.text = $"HP : {healthSystem.currentHP}/{healthSystem.maxHP}";
            playerHPBar.value = (float)healthSystem.currentHP / healthSystem.maxHP;
        }
        else
        {
            playerHPText.text = $"HP : 0/{healthSystem.maxHP}";
            playerHPBar.value = 0;
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
        if(healthSystem.currentHP <= 0)
        {
            animator.SetTrigger(deadTriggerHash);

            StartCoroutine(DeactivateAfterDelay(2f));
            SpawnShards();
            GameOver();
        }
    }

    private IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void SpawnShards()
    {
        for (int i = 0; i < shardCount; i++)
        {
            GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = shard.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 forceDirection = Random.insideUnitCircle.normalized;
                float shardSpeed = Random.Range(shardMinSpeed, shardMaxSpeed);
                rb.AddForce(forceDirection * shardSpeed, ForceMode2D.Impulse);

                float torque = Random.Range(-shardMaxTorque, shardMaxTorque);
                rb.AddTorque(torque, ForceMode2D.Impulse);
            }
            Destroy(shard, shardDuration);
        }
    }
    private void GameOver()
    {
        gameOverPanel.SetActive(true);
    }
}
