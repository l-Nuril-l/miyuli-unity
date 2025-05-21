using Mirror;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public int maxHealth = 100; // ћаксимальное здоровье игрока
    [SyncVar]
    private int currentHealth; // “екущее здоровье игрока

    private AudioSource audioSource;
    public DateTime lastDamage = DateTime.Now.AddSeconds(-3);
    public RectTransform healthBar;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        currentHealth = maxHealth; // ”станавливаем начальное здоровье
    }

    [ClientRpc]
    public void Heal(int hp)
    {
        currentHealth += hp;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    [ClientRpc]
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // ¬ычитаем урон из текущего здоровь¤

        // ѕровер¤ем, не ушло ли здоровье игрока в отрицательное значение
        if (currentHealth <= 0)
        {
            Die(); // ≈сли здоровье меньше или равно нулю, игрок умирает
        }
        else
        {
            var target = (currentHealth / 25) * 25;
            if (target < 25) target = 25;
            if (target > 75) target = 75;
            if (lastDamage.AddSeconds(3) > DateTime.Now) return;
            audioSource.clip = Resources.Load<AudioClip>("Sounds/actors/male/pain" + target + "_" + Random.Range(1, 3));
            audioSource.Play();
        }
        if (healthBar)
        {
            healthBar.GetComponent<Image>().fillAmount = (float)currentHealth / (float)maxHealth;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isServer)
        {
            return;
        }
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(5);
            TakeDamage(5);
        }
    }

    private void Die()
    {
        audioSource.clip = Resources.Load<AudioClip>("Sounds/actors/male/death" + Random.Range(1, 5));
        audioSource.Play();
        transform.position = Vector3.zero + Vector3.up / 2;
        currentHealth = maxHealth;
    }
}
