using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class MapGenerationSystem : MonoBehaviour
{
    [SerializeField] private MapPopulationSystem mapPopulationSystem;
    [SerializeField] private int gridWidth = 7;
    [SerializeField] private int gridHeight = 15;
    [SerializeField] private float spacingX = 2.0f;
    [SerializeField] private float spacingY = 2.0f;
    [SerializeField] private float paddingX = 5f;
    [SerializeField] private float paddingY = 3.5f;
    [SerializeField] private int numberOfStartingNodes = 4;
    [SerializeField] private int totalPaths = 6;
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject connectionPrefab;
    [SerializeField] private Transform mapContainer;
    [SerializeField] private SpriteRenderer mapBackground;
    [SerializeField] private float nodeRadiusOffset = 0.5f;
    private List<List<MapNode>> MapGrid { get; } = new();

    public List<List<MapNode>> SetupMap()
    {
        int runSeed = RunManagerSystem.Instance.PlayerData.MapSeed;

        Random.InitState(runSeed);
        GenerateFlatGrid();
        ResizeBackground();
        GenerateMap();

        return MapGrid;
    }

    private void GenerateFlatGrid()
    {
        int nextNodeID = 0;

        for (int y = 0; y < gridHeight; y++)
        {
            List<MapNode> currentRow = new List<MapNode>();
            bool isBossRow = (y == gridHeight - 1);
            int nodesToSpawn = y == gridHeight - 1 ? 1 : gridWidth;
            for (int x = 0; x < nodesToSpawn; x++)
            {
                Vector2 spawnPos;
                string nodeName;

                if (isBossRow)
                {
                    spawnPos = new Vector2(0f, y * spacingY);
                    nodeName = "NodeBOSS";
                }
                else
                {
                    float spawnPosX = (x * spacingX) - ((gridWidth * spacingX) / 2f) + (spacingX / 2f);
                    spawnPos = new Vector2(spawnPosX, y * spacingY);
                    nodeName = $"Node{y}_{x}";
                }

                MapNode newNode = SpawnNode(spawnPos, nodeName, y, nextNodeID);

                currentRow.Add(newNode);
                nextNodeID++;
            }

            MapGrid.Add(currentRow);
        }
    }

    private MapNode SpawnNode(Vector2 position, string nodeName, int floorLevel, int nodeID)
    {
        GameObject spawnedNodeObject = Instantiate(nodePrefab, position, Quaternion.identity, mapContainer);
        spawnedNodeObject.name = nodeName;
        MapNode spawnedNode = spawnedNodeObject.GetComponent<MapNode>();
        spawnedNode.InitializeNode(nodeID, floorLevel);
        return spawnedNode;
    }

    private void GenerateMap()
    {
        int[] previousPathNodes = new int[gridHeight];

        List<int> initializedNodes = new List<int>();
        for (int j = 0; j < numberOfStartingNodes; j++)
        {
            int initialNodeNumber = Random.Range(0, gridWidth);
            while (initializedNodes.Contains(initialNodeNumber))
            {
                initialNodeNumber = Random.Range(0, gridWidth);
            }

            initializedNodes.Add(initialNodeNumber);
        }

        List<int> pathStarts = new List<int>();
        for (int i = 0; i < totalPaths; i++)
        {
            int assignedRoot = initializedNodes[i % initializedNodes.Count];
            pathStarts.Add(assignedRoot);
        }

        pathStarts.Sort();

        foreach (var pathStart in pathStarts)
        {
            GeneratePath(pathStart, previousPathNodes);
        }

        RemoveEmptyNodes();
    }

    private void GeneratePath(int initialNodeX, int[] previousPathX)
    {
        int currentX = initialNodeX;
        int currentDirection = -99;
        int directionStreak = 0;

        for (int y = 0; y < gridHeight - 1; y++)
        {
            MapNode currentNode = MapGrid[y][currentX];

            int minX = Mathf.Max(0, currentX - 1);
            int maxX = Mathf.Min(gridWidth - 1, currentX + 1);

            int leftBoundary = previousPathX[y + 1];
            minX = Mathf.Max(minX, leftBoundary);
            if (minX > maxX) minX = maxX;

            List<int> validChoices = new List<int>();

            for (int x = minX; x <= maxX; x++)
            {
                int proposedDirection = x - currentX;
                if (directionStreak >= 3 && proposedDirection == currentDirection) continue;
                validChoices.Add(x);
            }

            if (validChoices.Count == 0)
            {
                for (int x = minX; x <= maxX; x++) validChoices.Add(x);
            }

            int nextX = validChoices[Random.Range(0, validChoices.Count)];

            if (y == gridHeight - 2) nextX = 0;

            MapNode nextNode = MapGrid[y + 1][nextX];

            int actualMoveDirection = nextX - currentX;
            if (actualMoveDirection == currentDirection) directionStreak++;
            else
            {
                currentDirection = actualMoveDirection;
                directionStreak = 1;
            }

            if (!currentNode.NextNodes.Contains(nextNode))
            {
                ConnectNodes(currentNode, nextNode);
            }

            previousPathX[y + 1] = nextX;
            currentX = nextX;
        }
    }

    private void RemoveEmptyNodes()
    {
        for (int y = 0; y < MapGrid.Count; y++)
        {
            List<MapNode> row = MapGrid[y];

            for (int x = row.Count - 1; x >= 0; x--)
            {
                MapNode mapNode = row[x];
                if (mapNode.NextNodes.Count != 0 || y >= gridHeight - 1) continue;
                Destroy(mapNode.gameObject);
                row.RemoveAt(x);
            }
        }
    }

    private void ConnectNodes(MapNode fromNode, MapNode toNode)
    {
        GameObject newLine = Instantiate(connectionPrefab, Vector3.zero, Quaternion.identity, mapContainer);
        MapConnectionLine lineScript = newLine.GetComponent<MapConnectionLine>();
        lineScript.SetupConnection(fromNode.transform, toNode.transform, nodeRadiusOffset);
        fromNode.AddConnection(toNode, lineScript);
    }

    private void ResizeBackground()
    {
        if (mapBackground == null) return;
        float totalWidth = (gridWidth - 1) * spacingX;
        float totalHeight = (gridHeight - 1) * spacingY;
        float finalWidth = totalWidth + paddingX;
        float finalHeight = totalHeight + paddingY;
        mapBackground.size = new Vector2(finalWidth, finalHeight);
        float centerY = totalHeight / 2f;
        mapBackground.transform.position = new Vector3(0, centerY, mapBackground.transform.position.z);
    }
}