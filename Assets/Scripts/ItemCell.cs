using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCell : MonoBehaviour
{
    [SerializeField] private int itemID;

    public bool IsFull { get; set; }
    public int ItemID { get => itemID; }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
