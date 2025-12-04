using Unity.VisualScripting;
using UnityEngine;

public class Medpack : PickupItem
{
    [SerializeField] private int healAmount = 500;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.TryGetComponent<Player>(out var player);
        if (player.playerData.currentHealth < player.playerData.maxHealth) {
            player.SetHealth(player.playerData.currentHealth + healAmount);
            Destroy(gameObject);
        }
    }
}
