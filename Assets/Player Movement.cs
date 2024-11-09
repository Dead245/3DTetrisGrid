using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI.Table;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float cameraSensitivity;
    [SerializeField] float playerSpeed;

    private Rigidbody rb;
    private Vector2 movementInput;

    private float camClampValue = 90.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        var movement = new Vector3(movementInput.x, 0, movementInput.y);
        gameObject.transform.Translate( movement * playerSpeed * Time.deltaTime);
    }

    void OnLook(InputValue rotation) {
        Vector2 inputRot = rotation.Get<Vector2>();
        Vector3 camRot = cam.transform.rotation.eulerAngles;
        Vector3 playerRot = transform.rotation.eulerAngles;
        camRot.x -= inputRot.y * cameraSensitivity * Time.deltaTime;
        cam.transform.rotation = Quaternion.Euler(camRot);

        playerRot.y += inputRot.x * cameraSensitivity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(playerRot);
        //Rotate player
        
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
