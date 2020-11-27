using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Health : MonoBehaviourPunCallbacks, IPunObservable
{
    public Renderer[] renderers;

    [SerializeField] private float health = 100.0f;
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private float respawnTime = 5.0f;

    private int team = 0;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            health = (float)stream.ReceiveNext();
        }
    }

    private void Start()
    {
        GameMenuManager.instance.CurrentHealth(maxHealth);
        team = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        if (health <= 0)
        {
            photonView.RPC("RPC_PlayerKilled", RpcTarget.All, team);
            photonView.RPC("Respawning", RpcTarget.All);
        }
    }


    public void TakeDamage(float _damage)
    {
        if (photonView.IsMine)
        {
            if (health > 0)
                health -= _damage;

            GameMenuManager.instance.CurrentHealth(health, maxHealth);
        }
    }

    public void TakeHeal(float _heal)
    {
        if (photonView.IsMine)
        {
            if (health > 0)
                health += _heal;

            if (health > maxHealth)
                health = maxHealth;
            GameMenuManager.instance.CurrentHealth(health, maxHealth);
        }
    }

    [PunRPC]
    public void Respawning()
    {
        StartCoroutine(Respawn());
    }

    
    private IEnumerator Respawn()
    {
        health = 100;
        renderers = gameObject.GetComponentsInChildren<Renderer>();
        SetRenderers(false);

        float r = respawnTime;

        if (photonView.IsMine)
        {
            GameMenuManager.instance.ShowBackground(true);
            GameMenuManager.instance.RespawnerTimer("", "");
            GetComponent<PlayerController>().enabled = false;
            
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        GetComponent<CapsuleCollider>().enabled = false;

        while (r > 0)
        {
            yield return new WaitForSeconds(1);

            r -= 1.0f;
            if (photonView.IsMine)
            {
                if (GameTablo.matchEnded)
                {
                    GameMenuManager.instance.RespawnerTimer("", "");
                    yield break;
                }
                GameMenuManager.instance.RespawnerTimer("Time to respawn: ", r.ToString());
            }
        }


        GetComponent<CapsuleCollider>().enabled = true;
        if (photonView.IsMine)
        {
            GetComponent<PlayerController>().RandomEquipWeapon();
            GetComponent<PlayerController>().enabled = true;
            GameMenuManager.instance.CurrentHealth(health, maxHealth);
            GameMenuManager.instance.ShowBackground(false);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

        }
        if (team == 1)
            transform.position = PlayerSpawner.instance.RandomRedPosition();
        else if (team == 2)
            transform.position = PlayerSpawner.instance.RandomBluePosition();

        SetRenderers(true);

    }
    private void SetRenderers(bool state)
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = state;
        }
    }

    public bool IsMaxHealth()
    {
        if (health < maxHealth)
            return false;
        else
            return true;
    }

    [PunRPC]
    private void RPC_PlayerKilled(int _team)
    {
        if (_team == 1)
        {
            GameTablo.blueScore++;
        }
        else
        {
            GameTablo.redScore++;
        }
    }
}
