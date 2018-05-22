using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLink : MonoBehaviour
{
    public Boss boss { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bunny"))
        {
            boss.TakeDamage();
            PickupSound.Instance.PlaySound();
            gameObject.SetActive(false);
        }
    }
}
