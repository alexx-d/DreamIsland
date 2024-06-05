using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private int maxObjects = 10;
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private Transform spawnZone;
    [SerializeField] private float spawnDelay = 1f;
    [SerializeField] private float minDistanceBetweenObjects = 2f;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private bool spawning = false;

    private void Start()
    {
        StartCoroutine(SpawnObjectsWithDelay());
    }

    private IEnumerator SpawnObjectsWithDelay()
    {
        spawning = true;
        for (int i = 0; i < maxObjects; i++)
        {
            SpawnObject();
            yield return new WaitForSeconds(spawnDelay);
        }
        spawning = false;
    }

    private void Update()
    {
        if (!spawning)
        {
            CheckObjectCount();
        }
    }

    private void CheckObjectCount()
    {
        if (spawnedObjects.Count < maxObjects)
        {
            int objectsToSpawn = maxObjects - spawnedObjects.Count;
            for (int i = 0; i < objectsToSpawn; i++)
            {
                StartCoroutine(SpawnObjectWithDelay());
            }
        }
    }

    private IEnumerator SpawnObjectWithDelay()
    {
        spawning = true;
        yield return new WaitForSeconds(spawnDelay);
        SpawnObject();
        spawning = false;
    }

    private void SpawnObject()
    {
        if (spawnedObjects.Count >= maxObjects)
        {
            return;
        }

        Vector3 spawnPosition = GetRandomSpawnPosition();

        GameObject newObj = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);

        spawnedObjects.Add(newObj);

        FishRemover remover = newObj.GetComponent<FishRemover>();
        if (remover == null)
        {
            remover = newObj.AddComponent<FishRemover>();
        }
        remover.Removed += OnObjectRemoved;
    }

    private void OnObjectRemoved(GameObject removedObject)
    {
        spawnedObjects.Remove(removedObject);

        CheckObjectCount();
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPosition = Vector3.zero;
        bool validPosition = false;
        int safetyCounter = 0;

        while (!validPosition)
        {
            if (spawnZone != null)
            {
                Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
                spawnPosition = spawnZone.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
            }
            else
            {
                spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            }

            validPosition = true;
            foreach (GameObject obj in spawnedObjects)
            {
                if (Vector3.Distance(spawnPosition, obj.transform.position) < minDistanceBetweenObjects)
                {
                    validPosition = false;
                    break;
                }
            }

            safetyCounter++;
            if (safetyCounter > 100)
            {
                Debug.LogWarning("Cannot find valid spawn position after 100 attempts.");
                break;
            }
        }

        return spawnPosition;
    }
}
