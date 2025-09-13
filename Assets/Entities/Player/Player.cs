using UnityEngine;
using UnityEngine.InputSystem;  
public class NewMonoBehaviourScript : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;
    public InputActionReference moveAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        movement = moveAction.action.ReadValue<Vector2>();
    }

    //Called once per physics time step    
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);
    }
}
