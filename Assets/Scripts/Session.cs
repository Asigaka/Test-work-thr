using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Синглот для точки входа и ссылок на контроллеры
/// </summary>
public class Session : MonoBehaviour
{
    [SerializeField] private CellsController cellsController;
    [SerializeField] private SoundsController soundsController;

    public static Session Instance;

    public CellsController CellsController { get => cellsController; }
    public SoundsController SoundsController { get => soundsController; }

    private void Awake()
    {
        if (Instance)
            Destroy(Instance);

        Instance = this;
    }

    private void Start()
    {
        cellsController.Initialize();

        StartNewSession();
    }

    /// <summary>
    /// Точка входа
    /// </summary>
    public void StartNewSession()
    {
        cellsController.ClearOldCells();
        cellsController.SpawnEmptyCells();
    }
}
