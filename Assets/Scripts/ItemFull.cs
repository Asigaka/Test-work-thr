using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ItemFull : MonoBehaviour
{
    [SerializeField] private int itemID;
    [SerializeField] private float checkCellRange = 1f;
    [SerializeField] private float returnDuration = 1;
    [SerializeField] private float returnShakeDuration = 1;
    [SerializeField] private float returnShakeStrength = 0.2f;
    [SerializeField] private LayerMask mouseRaycastLayer;
    [SerializeField] private AudioClip successAudio;

    private Transform startingPoint;
    private ItemCell nearbyItemCell;

    [HideInInspector] public UnityEvent onRightCellUp;

    public int ItemID { get => itemID; }

    public void Initialize(Transform startingPoint)
    {
        this.startingPoint = startingPoint;
    }

    private void OnMouseDrag()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, mouseRaycastLayer);

        if (hit)
        {
            transform.position = hit.point;
        }
    }

    private void OnMouseUp()
    {
        nearbyItemCell = TryGetCellNearby();

        if (nearbyItemCell && !nearbyItemCell.IsFull && nearbyItemCell.ItemID == itemID)
        {
            transform.SetParent(nearbyItemCell.transform);
            transform.localPosition = Vector2.zero;
            transform.localRotation = Quaternion.identity;
            nearbyItemCell.IsFull = true;
            Session.Instance.SoundsController.PlaySound(successAudio);
            onRightCellUp.Invoke();
        }
        else
        {
            transform.DOMove(startingPoint.position, returnDuration);
            transform.DOShakeScale(returnShakeDuration, returnShakeStrength);
        }
    }

    private ItemCell TryGetCellNearby()
    {
        Collider2D[] collidersNearby = Physics2D.OverlapCircleAll(transform.position, checkCellRange);

        foreach (Collider2D collider in collidersNearby)
        {
            ItemCell itemCell = collider.GetComponent<ItemCell>();

            if (itemCell)
            {
                return itemCell;
            }
        }

        return null;
    }

    private void OnDestroy()
    {
        onRightCellUp.RemoveAllListeners();
        transform.DOKill();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkCellRange);
    }
}
