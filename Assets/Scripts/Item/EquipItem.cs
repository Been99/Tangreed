using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class EquipItem : MonoBehaviour
{
    public ItemSO itemdata;
    private InputController input;
    private BoxCollider2D box2D;
    private Animator animator;
    private bool isActivated = false;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        input = GetComponentInParent<InputController>();
        box2D = GetComponent<BoxCollider2D>();
        input.PlayerAttack += OnAttack;

        if (box2D != null)
        {
            box2D.enabled = false;
        }
    }

    void OnAttack()
    {
        if (!isActivated)
        {
            StartCoroutine(ActivateCollider());
        }
    }

    private IEnumerator ActivateCollider()
    {
        if (box2D != null)
        {
            isActivated = true;
            box2D.enabled = true;
            animator.SetTrigger("OnAttack");

            // 지정된 시간 동안 대기
            yield return new WaitForSeconds(1f);

            // 대기 후 BoxCollider2D 비활성화
            box2D.enabled = false;
            isActivated = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsLayerMatched(itemdata.target.value, collision.gameObject.layer))
        {
            HealthSystem healthSystem = collision.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                // 충돌한 오브젝트의 체력을 감소시킵니다.
                bool isAttackApplied = healthSystem.ChangeHealth(-itemdata.power);

                Debug.Log($"{itemdata.power}만큼의 데미지");
            }
        }
    }

    private bool IsLayerMatched(int layerMask, int objectLayer)
    {
        return layerMask == (layerMask | (1 << objectLayer));
    }

    public GameObject prefab; // 생성할 프리팹 = ItemSO에 weaponPrefab(예시)를 만들고 등록해준 뒤 찾아오기
    public Transform parentTransform; // 부모가 될 오브젝트의 트랜스폼 =  WaeponPivot
    private GameObject instantiatedObject; // 생성된 오브젝트를 추적하기 위한 변수
    public void OnEquip()
    {
        //prefab = itemdata.weaponPrefab;
        //instantiatedObject = Instantiate(prefab);
        //instantiatedObject.transform.SetParent(parentTransform);
    }
    public void UnEquip()
    {
        //if (instantiatedObject != null)
        //{
        //    Destroy(instantiatedObject);
        //    instantiatedObject = null; // 오브젝트가 제거되었음을 추적하기 위해 null로 설정함.
        //}
    }
}