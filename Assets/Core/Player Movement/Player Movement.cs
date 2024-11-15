using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody)),RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float cameraSensitivity;
    [SerializeField] float playerSpeed;
    [SerializeField] float verticalCamClamp;

    CharacterController charController;

    private Rigidbody rb;
    private Vector2 movementInput;
    private Vector2 inputRot;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!TryGetComponent<CharacterController>(out charController)) { 
            Debug.LogError($"{name}'s Player Movement Script doesn't have a \"Character Controller\" component!"); 
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float delta = Time.deltaTime;
        //Move Character
        Vector3 moveDir = new Vector3(movementInput.x, 0f, movementInput.y);
        moveDir = transform.TransformDirection(moveDir);
        charController.Move(moveDir * playerSpeed * delta);

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

    void OnAttack()
    {
        Debug.Log("Attack (Left Clicked)");
    }

    void OnJump() {
        Debug.Log("Jump");
    }
}
