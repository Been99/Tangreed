using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemBox : MonoBehaviour
{
    private Animator animator;
    [SerializeField] ItemSO[] itemList;

    private bool isOpen = false;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        animator.SetBool("Open", isOpen);
    }


}
