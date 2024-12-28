using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float cameraSensitivity;
    [SerializeField] float playerSpeed;
    [SerializeField] float verticalCamClamp;
    CharacterController charController;

    private Vector2 movementInput;
    private Vector2 inputRot;

    private void Start()
    {
        if (!TryGetComponent<CharacterController>(out charController)) { 
            Debug.LogError($"{name}'s Player Movement Script doesn't have a \"Character Controller\" component!"); 
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float delta = Time.deltaTime;
        //Player Movement
        Vector3 moveDir = new Vector3(movementInput.x, 0f, movementInput.y);
        moveDir = transform.TransformDirection(moveDir);
        moveDir *= playerSpeed * delta;
        //Gravity
        moveDir.y = Physics.gravity.y * Time.deltaTime;
        //Apply movement
        charController.Move(moveDir);
        
        //Rotate Camera Up/Down
        float camRotX = cam.transform.rotation.eulerAngles.x - inputRot.y * cameraSensitivity * delta;
        cam.transform.localRotation = Quaternion.Euler(camRotX, 0f, 0f);
        //Rotate Player Left/Right
        transform.Rotate(Vector3.up * inputRot.x * cameraSensitivity * delta);
    }

    void OnLook(InputValue rotation) {
        inputRot = rotation.Get<Vector2>();
    }

    void OnMove(InputValue position)
    {
        movementInput = position.Get<Vector2>();
    }

    void OnJump() {
        Debug.Log("Jump");
    }
}
