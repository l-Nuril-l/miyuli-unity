using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Urine : Bullet
{
    private AudioSource audioSource;
    public DateTime lastUrineDamage = DateTime.Now.AddSeconds(-3);

    public Urine()
    {
        damage = 10;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private new void OnCollisionEnter(Collision collision)
    {
        if (lastUrineDamage.AddSeconds(3) > DateTime.Now) return;
        base.OnCollisionEnter(collision);
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            audioSource.clip = Resources.Load<AudioClip>("Sounds/actors/male/flamedeath" + UnityEngine.Random.Range(1, 4));
            audioSource.Play();
            lastUrineDamage = DateTime.Now;
            //Destroy(gameObject);
        }

    }
}
