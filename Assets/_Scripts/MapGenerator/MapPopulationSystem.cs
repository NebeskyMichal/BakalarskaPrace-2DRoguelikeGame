using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class MapPopulationSystem : MonoBehaviour
{
    [SerializeField] private List<EncounterData> basicEncounters = new();
    [SerializeField] private List<EncounterData> eliteEncounters = new();
    [SerializeField] private List<EncounterData> bossEncounters = new();
    [SerializeField] private List<EncounterData> restEncounters = new();
    [SerializeField] private List<EncounterData> shopEncounters = new();

    [System.Serializable]
    public struct FixedEncounter
    {
        public int floorLevel;
        public EncounterType encounterType;
    }

    [SerializeField] private int minimumEliteFloor = 4;
    [SerializeField] private List<FixedEncounter> fixedFloors = new();


    private Dictionary<int, EncounterType> _fixedEncounters = new();

    private Dictionary<EncounterType, List<EncounterData>> _encounterPools = new();

    public void PopulateNodes(List<List<MapNode>> mapGrid)
    {
        SetupFixedEncounters(mapGrid.Count);
        SetupEncounterPools();

        for (int y = 0; y < mapGrid.Count; y++)
        {
            List<MapNode> row = mapGrid[y];

            foreach (var node in row)
            {
                if (!_fixedEncounters.TryGetValue(y, out EncounterType chosenType))
                {
                    chosenType = GetUniqueSiblingType(node, y, mapGrid);
                }

                EncounterData chosenEncounter = GetRandomEncounterOfType(chosenType);

                if (chosenEncounter != null)
                {
                    node.Setup(chosenEncounter);
                }
            }
        }
    }

    private EncounterType GetUniqueSiblingType(MapNode currentNode, int currentRowY, List<List<MapNode>> mapGrid)
    {
        List<EncounterType> possibleTypes = new List<EncounterType>
        {
            EncounterType.Basic,
            EncounterType.Elite,
        };

        if (currentRowY < minimumEliteFloor)
        {
            possibleTypes.Remove(EncounterType.Elite);
        }

        List<MapNode> parentRow = mapGrid[currentRowY - 1];
        foreach (MapNode parent in parentRow)
        {
            if (parent.NextNodes.Contains(currentNode))
            {
                if (parent.EncounterData != null)
                {
                    EncounterType parentType = parent.EncounterData.EncounterType;
                    if (parentType == EncounterType.Elite || parentType == EncounterType.Rest ||
                        parentType == EncounterType.Shop)
                    {
                        possibleTypes.Remove(parentType);
                    }
                }

                foreach (MapNode child in parent.NextNodes)
                {
                    if (child != currentNode && child.EncounterData != null)
                    {
                        possibleTypes.Remove(child.EncounterData.EncounterType);
                    }
                }
            }
        }

        return possibleTypes.Count == 0 ? EncounterType.Basic : possibleTypes[Random.Range(0, possibleTypes.Count)];
    }

    private void SetupFixedEncounters(int totalFloors)
    {
        _fixedEncounters.Clear();

        _fixedEncounters.Add(0, EncounterType.Basic);
        _fixedEncounters.Add(totalFloors - 2, EncounterType.Rest);
        _fixedEncounters.Add(totalFloors - 1, EncounterType.Boss);

        foreach (var fixedFloor in fixedFloors)
        {
            if (!_fixedEncounters.ContainsKey(fixedFloor.floorLevel) && fixedFloor.floorLevel < totalFloors)
            {
                _fixedEncounters.Add(fixedFloor.floorLevel, fixedFloor.encounterType);
            }
        }
    }

    private void SetupEncounterPools()
    {
        _encounterPools.Add(EncounterType.Basic, basicEncounters);
        _encounterPools.Add(EncounterType.Elite, eliteEncounters);
        _encounterPools.Add(EncounterType.Boss, bossEncounters);
        _encounterPools.Add(EncounterType.Rest, restEncounters);
        _encounterPools.Add(EncounterType.Shop, shopEncounters);
    }

    private EncounterData GetRandomEncounterOfType(EncounterType type)
    {
        List<EncounterData> pool = basicEncounters;
        if (!_encounterPools.TryGetValue(type, out List<EncounterData> encounters)) return null;
        pool = encounters;
        return pool.Count == 0 ? null : pool[Random.Range(0, pool.Count)];
    }
}