using System;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    private GameObject itemPrefab;

    Int32 itemsCount;

    private void OnEnable()
    {
        GameManager.OnStartGameCallback += SpawnItems;
        GameManager.OnRestartGameCallback += OnRestart;

    }
    private void OnDisable()
    {
        GameManager.OnStartGameCallback -= SpawnItems;
        GameManager.OnRestartGameCallback -= OnRestart;
    }
    private void SpawnItems(Int32 value)
    {
        itemsCount = value;
        for (int i = 0; i < itemsCount; i++)
        {
            Instantiate(itemPrefab, new Vector3(UnityEngine.Random.Range(-5,5), .5f, UnityEngine.Random.Range(-5, 5)), Quaternion.identity,this.transform);
        }
    }

    private void OnRestart()
    {
        for (int i = 0; i < itemsCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
