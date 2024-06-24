using System;
using UnityEngine;

public class PlayerProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer;

    private PlayerRangedAttackSO attackData;
    private float currentDuration;
    private Vector2 direction;
    private bool isReady;

    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;
    ParticleSystem particleSystem;

    public bool fxOnDestory = true;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!isReady)
        {
            return;
        }

        currentDuration += Time.deltaTime;

        if (currentDuration > attackData.duration)
        {
            DestroyProjectile(transform.position, false);
        }

        rigidBody.velocity = direction * attackData.speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // levelCollisionLayer에 포함되는 레이어인지 확인합니다.
        if (IsLayerMatched(levelCollisionLayer.value, collision.gameObject.layer))
        {
            // 벽에서는 충돌한 지점으로부터 약간 앞 쪽에서 발사체를 파괴합니다.
            Vector2 destroyPosition = collision.ClosestPoint(transform.position) - direction * .2f;
            DestroyProjectile(destroyPosition, fxOnDestory);
        }
        // _attackData.target에 포함되는 레이어인지 확인합니다.
        else if (IsLayerMatched(attackData.target.value, collision.gameObject.layer))
        {
            HealthSystem healthSystem = collision.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                // 충돌한 오브젝트의 체력을 감소시킵니다.
                bool isAttackApplied = healthSystem.ChangeHealth(-attackData.power);

            }
            // 충돌한 지점에서 발사체를 파괴합니다.
            DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestory);
        }
    }

    // 레이어가 일치하는지 확인하는 메소드입니다.
    private bool IsLayerMatched(int layerMask, int objectLayer)
    {
        return layerMask == (layerMask | (1 << objectLayer));
    }

    public void InitializeAttack(Vector2 direction, PlayerRangedAttackSO attackData)
    {
        this.attackData = attackData;
        this.direction = direction;

        UpdateProjectileSprite();
        trailRenderer.Clear();
        currentDuration = 0;

        transform.right = this.direction;

        isReady = true;
    }

    private void UpdateProjectileSprite()
    {
        transform.localScale = Vector3.one * attackData.size;
    }

    private void DestroyProjectile(Vector3 position, bool createFx)
    {
        if (createFx)
        {
            particleSystem.transform.position = position;
            ParticleSystem.EmissionModule em = particleSystem.emission;
            em.SetBurst(0, new ParticleSystem.Burst(0, Mathf.Ceil(attackData.size * 5)));
            ParticleSystem.MainModule mainModule = particleSystem.main;
            mainModule.startSpeedMultiplier = attackData.size * 10f;
            particleSystem.Play();
        }
        gameObject.SetActive(false);
    }
}