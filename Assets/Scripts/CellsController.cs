using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellsController : MonoBehaviour
{
    [SerializeField] private ItemCell[] itemCellsPrefabs;
    [SerializeField] private ItemFull[] itemPrefabs;
    [SerializeField] private Transform[] cellSpawnPoints;
    [SerializeField] private Transform cellStartingPoint;
    [SerializeField] private AudioClip cellSpawnAudio;

    [Space]
    [SerializeField] private float fullItemSpawnDuration = 1;
    [SerializeField] private float fullItemSpawnXOffset = 7;
    [SerializeField] private float cellSpawnDelay = 1;
    [SerializeField] private float cellSpawnMoveDuration = 2;
    [SerializeField] private float cellSpawnYOffset = 5;

    private List<ItemCell> spawnedCells;
    private ItemFull spawnedItem;

    public void Initialize() 
    {
        spawnedCells = new List<ItemCell>();
    }

    public void SpawnEmptyCells()
    {
        StartCoroutine(ESpawnCells());
    }

    public void ClearOldCells()
    {
        for (int i = 0; i < spawnedCells.Count; i++)
        {
            Destroy(spawnedCells[i].gameObject);
        }

        spawnedCells.Clear();

        if (spawnedItem)
        {
            Destroy(spawnedItem);
        }
    }

    public void CheckOrSpawnItemToBeFound()
    {
        List<ItemCell> localSpawnedCells = new List<ItemCell>();

        for (int i = 0; i < spawnedCells.Count; i++)
        {
            if (!spawnedCells[i].IsFull)
            {
                localSpawnedCells.Add(spawnedCells[i]);
            }
        }

        if (localSpawnedCells.Count == 0)
        {
            Session.Instance.StartNewSession();
        }
        else
        {
            int randEmptyCellIndex = Random.Range(0, localSpawnedCells.Count - 1);

            foreach (ItemFull itemPrefab in itemPrefabs)
            {
                if (itemPrefab.ItemID == localSpawnedCells[randEmptyCellIndex].ItemID)
                {
                    spawnedItem = Instantiate(itemPrefab);
                    spawnedItem.transform.position = new Vector2(cellStartingPoint.position.x + fullItemSpawnXOffset, cellStartingPoint.position.y);
                    spawnedItem.transform.DOMove(cellStartingPoint.position, fullItemSpawnDuration).SetEase(Ease.InSine);
                    spawnedItem.transform.DOShakeRotation(fullItemSpawnDuration);
                    spawnedItem.Initialize(cellStartingPoint);
                    spawnedItem.onRightCellUp.AddListener(CheckOrSpawnItemToBeFound);
                }
            }
        }
    }

    private IEnumerator ESpawnCells()
    {
        ItemCell[] unrandomedCells = itemCellsPrefabs; 

        for (int i = 0; i < cellSpawnPoints.Length; i++)
        {
            Session.Instance.SoundsController.PlaySound(cellSpawnAudio);
            int randIndex = Random.Range(0, unrandomedCells.Length);
            ItemCell randomCellPrefab = unrandomedCells[randIndex];

            Transform cellPoint = cellSpawnPoints[i];
            ItemCell newSpawnedCell = Instantiate(randomCellPrefab,
                new Vector2(cellPoint.position.x, cellPoint.position.y + cellSpawnYOffset), Quaternion.identity);

            newSpawnedCell.transform.DOMove(cellPoint.position, cellSpawnMoveDuration);
            newSpawnedCell.transform.DOShakeRotation(cellSpawnMoveDuration);
            newSpawnedCell.transform.SetParent(cellPoint);
            spawnedCells.Add(newSpawnedCell);

            yield return new WaitForSeconds(cellSpawnDelay);
        }

        CheckOrSpawnItemToBeFound();
    }
}
