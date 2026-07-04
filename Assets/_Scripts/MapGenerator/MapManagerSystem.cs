using System.Collections.Generic;
using UnityEngine;

public class MapManagerSystem : Singleton<MapManagerSystem>
{
    [SerializeField] private MapPopulationSystem mapPopulationSystem;
    [SerializeField] private MapGenerationSystem mapGenerationSystem;
    [SerializeField] private MapMovementSystem mapMovementSystem;

    [SerializeField] private GameObject mapVisualContainer;
    [SerializeField] private Camera mapCamera;
    [SerializeField] private GameObject mapEventSystem;

    private List<List<MapNode>> _mapGrid;

    private void Start()
    {
        _mapGrid = mapGenerationSystem.SetupMap();
        mapPopulationSystem.PopulateNodes(_mapGrid);
        mapMovementSystem.Initialize(_mapGrid);
    }

    public void OnNodeClicked(MapNode clickedNode)
    {
        mapMovementSystem.MoveToNode(clickedNode);
        RunManagerSystem.Instance.StartEncounter(clickedNode);
    }


    public void HideMap()
    {
        mapVisualContainer.SetActive(false);
        mapCamera.gameObject.SetActive(false);
        mapEventSystem.SetActive(false);
    }

    public void ShowMap()
    {
        mapVisualContainer.SetActive(true);
        mapCamera.gameObject.SetActive(true);
        mapEventSystem.SetActive(true);
    }
}