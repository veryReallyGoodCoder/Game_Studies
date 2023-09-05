using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class MovementScript : MonoBehaviour
{

    CharacterController controller;

    private Vector2 playerInputMove, playerInputLook;

    [SerializeField] private float speed = 10f;
    private float gravity = -9.81f;

    //camera
    public Transform camera;
    [SerializeField] private float lookSpeed = 50f;
    private float xRotation = 0f;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        
        //movement
        //Vector3 move = new Vector3(playerInputMove.x, gravity, playerInputMove.y);

        Vector3 move = transform.right * playerInputMove.x  + transform.forward * playerInputMove.y + transform.up * gravity;
        //Vector3 applyGravity = new Vector3(0, gravity, 0);
        controller.Move(move * speed * Time.deltaTime);

        //camera


    }

    public void PlayerMove(InputAction.CallbackContext ctx)
    {
        playerInputMove = ctx.ReadValue<Vector2>();
    }

    public void PlayerLook(InputAction.CallbackContext ctx)
    {
        playerInputLook = ctx.ReadValue<Vector2>();
        Debug.Log(playerInputLook);

        xRotation -= playerInputLook.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        gameObject.transform.Rotate(Vector3.up * playerInputLook.x);

    }

}
