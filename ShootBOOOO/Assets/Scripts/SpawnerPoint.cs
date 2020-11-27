using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerPoint : MonoBehaviour
{
    public int id = -1;
    public bool isSpawned = false;
    [SerializeField] private GameObject spawnedObject;
    [SerializeField] private GameObject spawnerCanvas;
    [SerializeField] private Image spawnerSlider;
    [SerializeField] private float spawnTimer = 5.0f;

    private float currentSpawnTimer = 0f;

    private void Start()
    {
        spawnerCanvas.SetActive(false);
    }

    public void SpawnObject(string _tag)
    {
        isSpawned = true;

        var spawnItems = GameObject.FindGameObjectsWithTag(_tag);

        if (spawnItems.Length > 0)
        {
            for (int i = 0; i < spawnItems.Length; i++)
            {
                if (transform.position == spawnItems[i].gameObject.transform.position)
                {
                    spawnedObject = spawnItems[i].gameObject;
                    spawnedObject.GetComponent<SpawnItem>().myPoint = id;
                    spawnItems[i].transform.parent = gameObject.transform;
                }
            }
        }
        if (currentSpawnTimer <= 0)
            spawnerCanvas.SetActive(false);
    }

    public void ClearSpawn(string _tag)
    {
        isSpawned = false;
        spawnedObject = null;

        spawnerCanvas.SetActive(true);
        currentSpawnTimer = spawnTimer;
        StartCoroutine(StartSpawnTimer(_tag));
    }

    private IEnumerator StartSpawnTimer(string _tag)
    {
        while (currentSpawnTimer > 0)
        {

            spawnerSlider.fillAmount = currentSpawnTimer / spawnTimer;

            yield return new WaitForSeconds(spawnTimer/100f);

            currentSpawnTimer -= spawnTimer/100;

        }

        if (_tag == "Health")
            SpawnerItems.instance.HealthItemSingleSpawn(id);
        else if (_tag == "Ammo")
            SpawnerItems.instance.AmmoSingleItemSpawn(id);
        else if (_tag == "Gun")
            SpawnerItems.instance.WeaponSingleItemSpawn(id);
    }
    
}
