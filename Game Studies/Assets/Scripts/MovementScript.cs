using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class MovementScript : MonoBehaviour
{

    CharacterController controller;

    private Vector2 playerInput;

    [SerializeField] private float speed = 10f;

    public void PlayerMove(InputAction.CallbackContext ctx)
    {
        playerInput = ctx.ReadValue<Vector2>();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        
        Vector3 move = new Vector3(playerInput.x, 0, playerInput.y);
        controller.Move(move * speed * Time.deltaTime);

    }



}
