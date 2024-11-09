using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI.Table;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float cameraSensitivity;
    [SerializeField] float playerSpeed;

    CharacterController charController;

    private Rigidbody rb;
    private Vector2 movementInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        charController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (movementInput == null) return;
        var movement = movementInput.x * transform.right + movementInput.y * transform.forward;


        charController.Move(movement * playerSpeed * Time.deltaTime);
    }

    void OnLook(InputValue rotation) {
        //Rotate Camera Up/Down
        Vector2 inputRot = rotation.Get<Vector2>();

        float camRotX = cam.transform.rotation.eulerAngles.x;
        camRotX -= inputRot.y * cameraSensitivity * Time.deltaTime;

        cam.transform.localRotation = Quaternion.Euler(camRotX,0f,0f);

        //Rotate Player Left/Right
        transform.Rotate(Vector3.up * inputRot.x * cameraSensitivity * Time.deltaTime);

        
    }

    void OnMove(InputValue position)
    {
        movementInput = position.Get<Vector2>();
    }

    void OnAttack()
    {
        Debug.Log("Attack (Left Clicked)");
    }

    void OnJump() {
        Debug.Log("Jump");
    }

}
