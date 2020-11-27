using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponItem : SpawnItem
{
    public PlayerItem item;

    PhotonView pView;

    private void Start()
    {
        pView = GetComponent<PhotonView>();
        SpawnerItems.instance.CheckPoints("Gun");
        itemPickAudio = GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerController playerController = collider.GetComponent<PlayerController>();

            if (playerController != null)
            {
                itemPickAudio.Play();
                playerController.PickUpItem(item);
                pView.RPC("DestroyItem", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void DestroyItem()
    {
        Destroy(this.gameObject, 0.25f);
        SpawnerItems.instance.ClearWeaponPoint(myPoint);
    }
}
