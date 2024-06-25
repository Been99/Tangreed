using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    private Animator animator;
    private Transform dropPosition;

    [SerializeField] ItemSO[] itemList;

    private bool isOpen = false;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        dropPosition = GetComponent<Transform>();
    }

    private void Start()
    {
        animator.SetBool("Open", isOpen);
    }

    // TODO : 상호작용 키 눌렀을 때 작동하게
    public void OpenItemBox()
    {
        if (itemList.Length > 0)
        {
            int index = Random.Range(0, itemList.Length);
            ItemSO selectedItem = itemList[index];

            if(selectedItem.dropPrefab != null )
            {
                DropItem(selectedItem);
            }

            List<ItemSO> itemListTemp = new List<ItemSO>(itemList);
            itemListTemp.RemoveAt(index);
            itemList = itemListTemp.ToArray();
        }
    }

    public void DropItem(ItemSO item)
    {
        Instantiate(item.dropPrefab, dropPosition.position, Quaternion.identity);
    }

    public void StartAnim()
    {
        animator.SetBool("Open", !isOpen);
    }
}
