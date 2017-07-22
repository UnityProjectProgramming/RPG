using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
namespace RPG.Weapons
{
    [ExecuteInEditMode]
    public class WeaponPickupPoint : MonoBehaviour
    {
        [SerializeField] Weapon weaponConfig;
        [SerializeField] AudioClip pickUpSFX;


        void DestroyChildren()
        {
            foreach(Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                DestroyChildren();
                InstantiateWeapon();
            }
        }

        void InstantiateWeapon()
        {
            var weapon = weaponConfig.GetWeaponPrefab();
            weapon.transform.position = Vector3.zero;
            Instantiate(weapon, gameObject.transform);
        }

        private void OnTriggerEnter()
        {
            print("Pick Up Triggerd!");
            FindObjectOfType<Player>().PutWeaponInHand(weaponConfig);
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(pickUpSFX);
        }
    }
}

