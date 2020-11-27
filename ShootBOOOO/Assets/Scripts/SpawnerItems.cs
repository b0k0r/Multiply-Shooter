using UnityEngine;
using Photon.Pun;
using System.IO;

public class SpawnerItems : MonoBehaviour
{
    #region Singleton
    public static SpawnerItems instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one instance of SpawnerItems found!");
            Destroy(gameObject);
            return;
        }

        instance = this;
        pView = GetComponent<PhotonView>();
    }
    #endregion

    [SerializeField] private SpawnerPoint[] healthSpawnPoints;
    [SerializeField] private SpawnerPoint[] ammoSpawnPoints;
    [SerializeField] private SpawnerPoint[] weaponSpawnPoints;
    PhotonView pView;


    private void Start()
    {
        HealthItemSpawn();
        AmmoItemSpawn();
        WeaponItemSpawn();
    }

    public void HealthItemSpawn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < healthSpawnPoints.Length; i++)
            {
                GameObject go = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefs", "HealthObject"),
                                                        healthSpawnPoints[i].transform.position, Quaternion.identity);

            }
        }
    }

    public void HealthItemSingleSpawn(int _id)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < healthSpawnPoints.Length; i++)
            {
                if (healthSpawnPoints[i].id == _id)
                {
                    GameObject go = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefs", "HealthObject"),
                                                            healthSpawnPoints[i].transform.position, Quaternion.identity);
                    
                }
            }
        }
    }

    public void AmmoItemSpawn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < ammoSpawnPoints.Length; i++)
            {
                int _random = Random.Range(0, 3);
                GameObject go;
                switch(_random)
                {
                    case 0:
                        go = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefs", "AmmoPistolObject"),
                                                                ammoSpawnPoints[i].transform.position, Quaternion.identity);
                        break;

                    case 1:
                        go = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefs", "AmmoBowObject"),
                                                                ammoSpawnPoints[i].transform.position, Quaternion.identity);
                        break;

                    case 2:
                        go = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefs", "AmmoRocketObject"),
                                                                ammoSpawnPoints[i].transform.position, Quaternion.identity);
                        break;
                }

            }
        }
    }

    public void AmmoSingleItemSpawn(int _id)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < ammoSpawnPoints.Length; i++)
            {
                if (ammoSpawnPoints[i].id == _id)
                {
                    int _random = Random.Range(0, 3);
                    GameObject go;
                    switch (_random)
                    {
                        case 0:
                            go = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefs", "AmmoPistolObject"),
                                                                    ammoSpawnPoints[i].transform.position, Quaternion.identity);
                            break;

                        case 1:
                            go = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefs", "AmmoBowObject"),
                                                                    ammoSpawnPoints[i].transform.position, Quaternion.identity);
                            break;

                        case 2:
                            go = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefs", "AmmoRocketObject"),
                                                                    ammoSpawnPoints[i].transform.position, Quaternion.identity);
                            break;
                    }
                }
            }
        }
    }

    public void WeaponItemSpawn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < weaponSpawnPoints.Length; i++)
            {
                int _random = Random.Range(0, 3);
                GameObject go;
                switch (_random)
                {
                    case 0:
                        go = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefs", "PistolItemObject"),
                                                                weaponSpawnPoints[i].transform.position, Quaternion.identity);
                        break;

                    case 1:
                        go = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefs", "BowItemObject"),
                                                                weaponSpawnPoints[i].transform.position, Quaternion.identity);
                        break;

                    case 2:
                        go = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefs", "BazookaItemObject"),
                                                                weaponSpawnPoints[i].transform.position, Quaternion.identity);
                        break;
                }


            }
        }
    }

    public void WeaponSingleItemSpawn(int _id)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < weaponSpawnPoints.Length; i++)
            {
                if (ammoSpawnPoints[i].id == _id)
                {
                    int _random = Random.Range(0, 3);
                    GameObject go;
                    switch (_random)
                    {
                        case 0:
                            go = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefs", "PistolItemObject"),
                                                                    weaponSpawnPoints[i].transform.position, Quaternion.identity);
                            break;

                        case 1:
                            go = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefs", "BowItemObject"),
                                                                    weaponSpawnPoints[i].transform.position, Quaternion.identity);
                            break;

                        case 2:
                            go = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefs", "BazookaItemObject"),
                                                                    weaponSpawnPoints[i].transform.position, Quaternion.identity);
                            break;
                    }

                }
            }
        }
    }
         
    public void CheckPoints(string _tag)
    {
        if (_tag == "Health")
        {
            for (int i = 0; i < healthSpawnPoints.Length; i++)
            {
                healthSpawnPoints[i].SpawnObject(_tag);
            }
        }
        else if(_tag == "Ammo")
        {
            for (int i = 0; i < ammoSpawnPoints.Length; i++)
            {
                ammoSpawnPoints[i].SpawnObject(_tag);
            }
        }
        else if(_tag == "Gun")
        {
            for (int i = 0; i < weaponSpawnPoints.Length; i++)
            {
                weaponSpawnPoints[i].SpawnObject(_tag);
            }
        }
    }


    public void ClearHealthPoint(int _id)
    {
        foreach(SpawnerPoint s in healthSpawnPoints)
        {
            if(_id == s.id)
                s.ClearSpawn("Health"); 
        }
    }

    public void ClearAmmoPoint(int _id)
    {
        foreach (SpawnerPoint s in ammoSpawnPoints)
        {
            if (_id == s.id)
                s.ClearSpawn("Ammo");
        }
    }

    public void ClearWeaponPoint(int _id)
    {
        foreach (SpawnerPoint s in weaponSpawnPoints)
        {
            if (_id == s.id)
                s.ClearSpawn("Gun");
        }
    }
}
