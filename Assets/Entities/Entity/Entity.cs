using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Entity : MonoBehaviour
{

    // General Entity Properties
    public int currentHealth;
    public int maxHealth;
    public float moveSpeed;

    public Rigidbody2D rb;
    public Vector2 movement;

    public AudioSource SFXSource;
    public AudioClip[] SFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
