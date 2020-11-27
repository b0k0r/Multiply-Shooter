using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject cameraHolder;
    public Camera cam;

    [SerializeField] private float mouseSensetivityX = 1.0f;
    [SerializeField] private float mouseSensetivityY = 1.0f;
    [SerializeField] private float sprintSpeed = 10.0f;
    [SerializeField] private float walkSpeed = 5.0f;
    [SerializeField] private float smoothTime = 1.0f;
    [SerializeField] private float checkGroundDistance = -0.15f;

    [SerializeField] private PlayerItem[] playerItems;
    
    public int playerItemIndex;
    int previousPlayerItemIndex = -1;

    float verticalLookRotation;

    public bool isGrounded;

    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;
    PhotonView phView;

    public Weapon currentWeapon;
    PlayerItem currentItem;

    public float charge;
    public float chargeRate;
    
    private float nextTimeToFire = 0;
    bool isReloading = false;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        phView = GetComponent<PhotonView>();
        cam = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        if (phView.IsMine) 
        {
            RandomEquipWeapon();
        }
        else
        {
            cam.enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
            Destroy(rb);
        }

    }

    public void RandomEquipWeapon()
    {
        int r = Random.Range(0, playerItems.Length);
        for (int i = 0; i < playerItems.Length; i++)
        {
            if (r == i)
            {
                playerItems[i].isAvailable = true;
                GameMenuManager.instance.ActivateItem(r, true);
                EquipItem(r);
            }
            else
            {
                playerItems[i].isAvailable = false;
                GameMenuManager.instance.ActivateItem(i, false);
            }

            playerItems[i].StartComplect();
            GameMenuManager.instance.SelectWeaponItem(r);
        }
    }

    private void Update()
    {
        if (!phView.IsMine || GameTablo.matchEnded)
            return;

        //Debug.DrawRay(playerItems[playerItemIndex].bulletHolder.position, playerItems[playerItemIndex].bulletHolder.forward * currentWeapon.range);
        //Debug.DrawLine(transform.position + transform.up * 0.05f, transform.position + new Vector3(0, checkGroundDistance, 0), Color.blue);

        if (Physics.Linecast(transform.position + transform.up * 0.05f, transform.position + new Vector3(0, checkGroundDistance, 0)))
            isGrounded = true;
        else
            isGrounded = false;

        LookRotation();
        Move();

        for (int i = 0; i < playerItems.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                if (playerItems[i].isAvailable)
                {
                    EquipItem(i);
                    break;
                }
            }

        }

        if (playerItemIndex == 1)
        {
            if (Input.GetKey(KeyCode.Mouse0) 
                && charge < currentWeapon.force 
                && Time.time >= nextTimeToFire
                && !isReloading)
            {
                if (currentItem.currentAmmo > 0)
                {
                    charge += Time.deltaTime * chargeRate;
                    GameMenuManager.instance.ShowPullBowString(true);
                    GameMenuManager.instance.CurrentPullBowString(charge, currentWeapon.force);
                }
                else
                    Reload();
            }

            if (Input.GetKeyUp(KeyCode.Mouse0) && Time.time >= nextTimeToFire && !isReloading)
            {

                if (currentItem.currentAmmo > 0)
                {
                    nextTimeToFire = Time.time + currentWeapon.fireRate;
                    phView.RPC("Shoot", RpcTarget.All);
                    charge = 0;
                    GameMenuManager.instance.ShowPullBowString(false);
                    currentItem.currentAmmo--;
                    if (currentItem.currentAmmo == 0)
                        Reload();
                }
            }
        }
        else
        {
            if (Input.GetButton("Fire1") 
                && Time.time >= nextTimeToFire
                && !isReloading)
            {
                if (currentItem.currentAmmo > 0)
                {
                    nextTimeToFire = Time.time + currentWeapon.fireRate;
                    phView.RPC("Shoot", RpcTarget.All);
                    currentItem.currentAmmo--;
                    if (currentItem.currentAmmo == 0)
                        Reload();
                }
                else
                    Reload();
            }

        }


        if (Input.GetKeyDown(KeyCode.R))
            Reload();

        GameMenuManager.instance.CurrentAmmo(currentItem);
    }

    private void FixedUpdate()
    {
        if (!phView.IsMine)
            return;


        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    private void Move()
    {
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(xMov, 0, zMov).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDirection * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }

    private void LookRotation()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensetivityX);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensetivityY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -35, 80);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    public void EquipItem(int _index)
    {
        if (_index == previousPlayerItemIndex)
            return;

        isReloading = false;
        playerItemIndex = _index;
        currentWeapon = (Weapon)playerItems[playerItemIndex].item;
        currentItem = playerItems[playerItemIndex];
        

        playerItems[playerItemIndex].itemGameObject.SetActive(true);

        if (previousPlayerItemIndex != -1) 
        {
            playerItems[previousPlayerItemIndex].itemGameObject.SetActive(false);
        }

        previousPlayerItemIndex = playerItemIndex;

        if (phView.IsMine) 
        {
            Hashtable hash = new Hashtable();
            hash.Add("playerItemIndex", playerItemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

            GameMenuManager.instance.SelectWeaponItem(playerItemIndex);
        }
    }
    
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!phView.IsMine && targetPlayer == phView.Owner) 
        {
            EquipItem((int)changedProps["playerItemIndex"]);
        }
    }

    private void Reload()
    {
        if (currentItem.maxAmmo <= 0)
        {
            GameMenuManager.instance.Message("Not enought ammo..");
            return;
        }

        if (currentItem.currentAmmo == currentItem.ammoPerMagazine)
            return;

        StartCoroutine(Reloading());
    }

    private IEnumerator Reloading()
    {
        isReloading = true;
        float t = currentWeapon.reloadTime;
        GameMenuManager.instance.ShowReload(isReloading);

        while (isReloading) 
        {
            if (t >= 0)
            {
                GameMenuManager.instance.CurrentReload(t, currentWeapon.reloadTime);
                yield return new WaitForSeconds(currentWeapon.reloadTime / 100);
                t -= currentWeapon.reloadTime / 100;
            }
            else
                isReloading = false;
        }

        int ammoToLoad = currentItem.ammoPerMagazine - currentItem.currentAmmo;
        int ammoToDeduct = (currentItem.maxAmmo >= currentItem.ammoPerMagazine) ? ammoToLoad : currentItem.maxAmmo;

        currentItem.maxAmmo -= ammoToDeduct;
        currentItem.currentAmmo += ammoToDeduct;

        isReloading = false;
        GameMenuManager.instance.ShowReload(isReloading);
    }

    [PunRPC]
    public void Shoot()
    {
        if (photonView.IsMine)
            currentItem.gunAudio.Play();

        if (playerItemIndex == 0)
        {
            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;

            if(Physics.Raycast(rayOrigin, cam.transform.forward, out hit, currentWeapon.range))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    Health enemyHealth = hit.collider.GetComponent<Health>();

                    if (enemyHealth != null)
                    {                        
                        enemyHealth.TakeDamage(currentWeapon.damage);
                    }
                }

                GameObject go1 = Instantiate(currentItem.impactVfx, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(go1, 2.0f);
            }
            else
            {
                if (hit.normal != Vector3.zero)
                {
                    GameObject go1 = Instantiate(currentItem.impactVfx, rayOrigin + (cam.transform.forward * currentWeapon.range), Quaternion.LookRotation(hit.normal));
                    Destroy(go1, 2.0f);
                }
            }
        }
        else if (playerItemIndex == 1) 
        {
            GameObject bulletObject = Instantiate(playerItems[playerItemIndex].bulletGameObject,
                                                  playerItems[playerItemIndex].bulletHolder.position,
                                                  playerItems[playerItemIndex].bulletHolder.rotation);
            
            bulletObject.GetComponent<WeaponBullet>().SetWeapon(currentItem);
            Rigidbody rBody = bulletObject.GetComponent<Rigidbody>();
            rBody.velocity = playerItems[playerItemIndex].bulletHolder.forward * charge;

        }
        else if (playerItemIndex == 2)
        {
            GameObject bulletObject = Instantiate(playerItems[playerItemIndex].bulletGameObject,
                                                  playerItems[playerItemIndex].bulletHolder.position,
                                                  playerItems[playerItemIndex].bulletHolder.rotation);
            
            bulletObject.GetComponent<WeaponBullet>().SetWeapon(currentItem);
            Rigidbody rBody = bulletObject.GetComponent<Rigidbody>();
            rBody.velocity = playerItems[playerItemIndex].bulletHolder.forward * currentWeapon.force;

        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(charge);
        }
        else
        {
            charge = (float)stream.ReceiveNext();
        }
    }

    public void AddAmmo(int _count, BulletType _type)
    {
        switch (_type)
        {
            case BulletType.Patrons:
                CalculateMaxAmmo(0, _count);
                break;

            case BulletType.Arrow:
                CalculateMaxAmmo(1, _count);
                break;

            case BulletType.Rocket:
                CalculateMaxAmmo(2, _count);
                break;
        }

        if (currentItem.item.bulletType == _type &&
            currentItem.currentAmmo == 0)
        {
            Reload();
        }
    }

    private void CalculateMaxAmmo(int _index, int _count)
    {
        int _maxAmmo = playerItems[_index].maxAmmo += _count;

        if (_maxAmmo <= playerItems[_index].item.maxAmmo)
            playerItems[_index].maxAmmo = _maxAmmo;
        else
            playerItems[_index].maxAmmo = playerItems[_index].item.maxAmmo;
    }

    public bool IsMaxAmmo(BulletType _bulletType)
    {
        int _index = (int)_bulletType;

        if (playerItems[_index].maxAmmo < playerItems[_index].item.maxAmmo)
            return false;
        else
            return true;
    }

    public void PickUpItem(PlayerItem _item)
    {
        for (int i = 0; i < playerItems.Length; i++)
        {
            if (playerItems[i].item.weaponType == _item.item.weaponType)
            {
                if (!playerItems[i].isAvailable)
                {
                    playerItems[i].isAvailable = true;
                    GameMenuManager.instance.ActivateItem(i, true);
                    //playerItems[i] = _item;
                    EquipItem(i);
                }
            }
        }
    }
}
