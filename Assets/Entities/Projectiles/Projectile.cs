using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject hitEffect;
    [SerializeField] public float damage = 1f;

    void OnCollissionEnter2D(Collision2D collision)
    {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        Destroy(gameObject);
    }
    
}
