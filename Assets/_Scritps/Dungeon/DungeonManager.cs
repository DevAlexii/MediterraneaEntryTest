using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [Header("Dungeon Settings")]
    [SerializeField]
    private GameObject roomPrefab;
    [SerializeField]
    private int maxRooms = 10;
    [SerializeField]
    private Vector2 roomSize = new Vector2(1, 1);

    [Header("Item Settings")]
    [SerializeField]
    private GameObject itemPrefab;
    private int itemsCount;

    [Header("Trap Settings")]
    [SerializeField]
    private GameObject trapPrefab;

    private Dictionary<Vector2, GameObject> spawnedRooms = new Dictionary<Vector2, GameObject>();
    private Vector2 currentPosition = Vector2.zero;
    private Vector2 previousDirection = Vector2.zero;
    private List<GameObject> spawnedItems = new List<GameObject>();


    private void OnEnable()
    {
        GameManager.OnStartGameCallback += GenerateDungeon;
        GameManager.OnRestartGameCallback += OnRestart;
    }

    private void OnDisable()
    {
        GameManager.OnStartGameCallback -= GenerateDungeon;
        GameManager.OnRestartGameCallback -= OnRestart;
    }

    private void GenerateDungeon(int value)
    {
        spawnedRooms.Clear();
        currentPosition = Vector2.zero;
        previousDirection = Vector2.zero;

        for (int i = 0; i < maxRooms; i++)
        {
            GameObject newRoom = Instantiate(roomPrefab,
                new Vector3(currentPosition.x * roomSize.x, 0, currentPosition.y * roomSize.y),
                Quaternion.identity, transform);

            spawnedRooms[currentPosition] = newRoom;

            if (i > 0 && spawnedRooms.ContainsKey(currentPosition - previousDirection))
            {
                DestroyWall(newRoom, -previousDirection);
                DestroyWall(spawnedRooms[currentPosition - previousDirection], previousDirection);

                PlaceStuffInPreviousRoom(currentPosition - previousDirection);
            }

            Vector2 newDirection = GetRandomDirection();

            if (newDirection == Vector2.zero)
            {
                break;
            }

            previousDirection = newDirection;
            currentPosition += newDirection;
        }

        SpawnItems(value);
    }

    private Vector2 GetRandomDirection()
    {
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        List<Vector2> validDirections = new List<Vector2>();

        foreach (Vector2 direction in directions)
        {
            if (!spawnedRooms.ContainsKey(currentPosition + direction))
            {
                validDirections.Add(direction);
            }
        }

        if (validDirections.Count == 0)
        {
            return Vector2.zero;
        }

        return validDirections[Random.Range(0, validDirections.Count)];
    }

    private void DestroyWall(GameObject room, Vector2 direction)
    {
        int wallIndex = 0;

        if (direction == Vector2.up) wallIndex = 2;
        else if (direction == Vector2.down) wallIndex = 3;
        else if (direction == Vector2.left) wallIndex = 1;
        else if (direction == Vector2.right) wallIndex = 0;

        room.transform.GetChild(wallIndex).gameObject.SetActive(false);
    }


    private void PlaceStuffInPreviousRoom(Vector2 previousRoomPosition)
    {
        if (spawnedRooms.ContainsKey(previousRoomPosition))
        {
            GameObject previousRoom = spawnedRooms[previousRoomPosition];
            List<int> remainingWalls = new List<int> { 0, 1, 2, 3 };

            if (previousDirection == Vector2.up) remainingWalls.Remove(2);
            else if (previousDirection == Vector2.down) remainingWalls.Remove(3);
            else if (previousDirection == Vector2.left) remainingWalls.Remove(1);
            else if (previousDirection == Vector2.right) remainingWalls.Remove(0);

            if (remainingWalls.Count == 0) return;

            int randomWallIndex = remainingWalls[Random.Range(0, remainingWalls.Count)];

            int attempts = 0;

            while (!previousRoom.transform.GetChild(randomWallIndex).gameObject.activeSelf && attempts < 5)
            {
                remainingWalls.Remove(randomWallIndex);
                if (remainingWalls.Count == 0) return; 

                randomWallIndex = remainingWalls[Random.Range(0, remainingWalls.Count)];
                attempts++;
            }

            Vector3 trapPosition = previousRoom.transform.GetChild(randomWallIndex).position;

            Quaternion trapRotation = Quaternion.identity;

            if (randomWallIndex == 2)
            {
                trapRotation = Quaternion.Euler(90f, 0f, 0f);
            }
            else if (randomWallIndex == 3)
            {
                trapRotation = Quaternion.Euler(-90f, 0f, 0f);
            }
            else if (randomWallIndex == 1)
            {
                trapRotation = Quaternion.Euler(0f, 0f, 90f);
            }
            else if (randomWallIndex == 0)
            {
                trapRotation = Quaternion.Euler(0f, 0f, -90f);
            }

            GameObject trap = Instantiate(trapPrefab, trapPosition, trapRotation,previousRoom.transform);
        }
    }



    private void SpawnItems(int value)
    {
        itemsCount = value;

        if (spawnedRooms.Count == 0) return;

        List<Vector3> visitedPos = new List<Vector3>();

        for (int i = 0; i < itemsCount; i++)
        {
            Vector3 randPos;
            do
            {
                var keys = spawnedRooms.Keys.ToList();
                var randomKey = keys[Random.Range(1, keys.Count)];
                randPos = spawnedRooms[randomKey].transform.position;
            } while (visitedPos.Contains(randPos));

            visitedPos.Add(randPos);


            Vector3 worldPosition = new Vector3(randPos.x, .5f, randPos.z);

            if (spawnedItems.Count == itemsCount)
            {
                spawnedItems[i].transform.position = worldPosition;
                spawnedItems[i].SetActive(true);
            }
            else
            {
                GameObject newItem = Instantiate(itemPrefab, worldPosition, Quaternion.identity, transform);
                spawnedItems.Add(newItem);
            }
        }
    }

    private void OnRestart()
    {
        foreach (GameObject room in spawnedRooms.Values)
        {
            Destroy(room);
        }

        spawnedRooms.Clear();

        GenerateDungeon(itemsCount);
    }

}
