using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HealthItem : SpawnItem
{
    public float health = 50;
    
    PhotonView pView;

    private void Start()
    {
        itemPickAudio = GetComponent<AudioSource>();
        pView = GetComponent<PhotonView>();
        SpawnerItems.instance.CheckPoints("Health");
    }

    public void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            Health myHealth = collider.GetComponent<Health>();

            if (myHealth != null)
            {
                if (!myHealth.IsMaxHealth())
                {
                    itemPickAudio.Play();
                    myHealth.TakeHeal(health);
                    pView.RPC("DestroyItem", RpcTarget.All);
                }
            }
        }
    }

    [PunRPC]
    public void DestroyItem()
    {
        Destroy(this.gameObject, 0.25f);
        SpawnerItems.instance.ClearHealthPoint(myPoint);
    }
}
