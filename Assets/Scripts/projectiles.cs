using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class projectiles : MonoBehaviour
{
    private List<GameObject> Projectiles = new List<GameObject>();
    public GameObject projectilePrefab;
    public AudioClip laserSound;
    private float nextFire = 0.0f;
    public Player player;

    public Inventory inventory; // Assign in inspector
    public TextMeshProUGUI ammoText; // Assign in inspector

    void Update()
    {
        // Shooting
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && Time.time > nextFire)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (inventory != null && inventory.GetTotalAmmo("Laser Ammo") > 0)
            {
                if (inventory.ConsumeAmmo("Laser Ammo", 1))
                {
                    nextFire = Time.time + player.fireRateDelay;

                    GameObject bullet = Instantiate(projectilePrefab, transform.position, transform.rotation);
                    Projectiles.Add(bullet);
                    AudioSource.PlayClipAtPoint(laserSound, transform.position);
                }
            }
            else
            {
                Debug.Log("Out of Ammo!");
            }
        }

        // Update UI
        if (ammoText != null && inventory != null)
        {
            ammoText.text = inventory.GetTotalAmmo("Laser Ammo").ToString();
        }
    }
}
