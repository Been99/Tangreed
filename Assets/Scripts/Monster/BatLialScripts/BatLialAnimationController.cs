using UnityEngine;

public class BatLialAnimationController : MonsterAnimationController
{
    public GameObject shardPrefab;
    public int shardCount = 25; 
    public float shardMinSpeed = 3f;
    public float shardMaxSpeed = 8f;
    public float shardMaxTorque = 10f;
    public float shardDuration = 4f;

    protected override void SubscribeToEvents()
    {
        monsterController.OnAttackEvent += Attacking;

        if (healthSystem != null)
        {
            healthSystem.OnDamageEvent += Hitting;
            healthSystem.OnDeathEvent += Death;
        }
    }

    protected override void Death()
    {
        if (isDead) { return; }

        isDead = true;
        animator.SetBool(isMovingHash, false);
        animator.SetTrigger(deadTriggerHash);

        StartCoroutine(DeactivateAfterDelay(2f));
        SpawnShards();
        GameManager.Instance.HandleBatLialDeath();
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
}

