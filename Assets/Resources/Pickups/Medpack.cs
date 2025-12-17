using Unity.VisualScripting;
using UnityEngine;

public class Medpack : PickupItem
{
    [SerializeField] private int healAmount = 500;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out var player) && player.playerData.currentHealth < player.playerData.maxHealth) {
            player.SetHealth(player.playerData.currentHealth + healAmount);
            Destroy(gameObject);
        }
    }
}
