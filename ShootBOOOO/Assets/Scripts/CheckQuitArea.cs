using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CheckQuitArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int team = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];

            if (team == 1)
                other.transform.position = PlayerSpawner.instance.RandomRedPosition();
            else if (team == 2)
                other.transform.position = PlayerSpawner.instance.RandomBluePosition();

        }
    }
}
