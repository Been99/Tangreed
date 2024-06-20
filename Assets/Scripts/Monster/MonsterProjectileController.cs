using UnityEngine;

public class MonsterProjectileController : MonoBehaviour
{
    private Vector3 direction;
    private LayerMask playerLayer;
    private MonsterAttackSO monsterAttackSO;
    private TrailRenderer trailRenderer;
    private float durationTime = 2;
    private float timeElapsed = 0;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        playerLayer = LayerMask.NameToLayer("Player");
    }

    public void Initialize(Vector3 direction, MonsterAttackSO attackData)
    {
        this.direction = direction.normalized;
        this.monsterAttackSO = attackData;
    }

    private void Update()
    {
        this.transform.position += direction * monsterAttackSO.projectileSpeed * Time.deltaTime;

        timeElapsed += Time.deltaTime;
        if (timeElapsed >= durationTime)
        {
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            HitTarget();
            Debug.Log("플레이어와 트리거 충돌;ㅅ;");
        }
    }

    private void HitTarget()
    {
        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        Destroy(this.gameObject);
    }
}
