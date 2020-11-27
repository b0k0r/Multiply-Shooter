using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class AmmoItem : SpawnItem
{
    public BulletType bulletType;
    public int ammo = 50;

    PhotonView pView;

    private void Start()
    {
        pView = GetComponent<PhotonView>();
        SpawnerItems.instance.CheckPoints("Ammo");
        itemPickAudio = GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerController playerController = collider.GetComponent<PlayerController>();

            if (playerController != null)
            {
                if (!playerController.IsMaxAmmo(bulletType))
                {
                    itemPickAudio.Play();
                    playerController.AddAmmo(ammo, bulletType);
                    pView.RPC("DestroyItem", RpcTarget.All);
                }
            }
        }
    }

    [PunRPC]
    public void DestroyItem()
    {
        Destroy(this.gameObject, 0.25f);
        SpawnerItems.instance.ClearAmmoPoint(myPoint);
    }
}
