using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTransformControls : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerDirection playerDirection;

    private PlayerInputActions playerInputActions;
    private Camera mainCamera;

    private bool isGamepadConnected;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        mainCamera = Camera.main;

        // Subscribe to input system events for device changes
        InputSystem.onDeviceChange += OnDeviceChange;
        UpdateGamepadConnectionStatus();
    }

    private void Update()
    {
        HandleMovementAndRotation();
    }

    private void HandleMovementAndRotation()
    {
        Vector2 inputMoveVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        playerMovement?.Move(inputMoveVector * Time.deltaTime);

        // Check for controller input first
        if (isGamepadConnected)
        {
            Vector2 lookInput = playerInputActions.Player.Look.ReadValue<Vector2>();
            if (lookInput == Vector2.zero)
            {
                playerDirection?.Direct(inputMoveVector);
            }
            else
            {
                playerDirection?.Direct(lookInput);
            }
        }
        else
        {
            // Fallback to mouse if no gamepad is connected
            if (Mouse.current?.wasUpdatedThisFrame == true)
            {
                HandleMouseRotation();
            }
        }
    }

    private void HandleMouseRotation()
    {
        if (!TryGetMouseWorldPoint(out Vector3 targetPoint))
            return;

        playerDirection?.DirectToPoint(targetPoint);
    }

    private bool TryGetMouseWorldPoint(out Vector3 worldPoint)
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPosition);

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(ray, out float enter))
        {
            worldPoint = ray.GetPoint(enter);
            return true;
        }

        worldPoint = Vector3.zero;
        return false;
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;

        playerInputActions.Player.Disable();
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change == InputDeviceChange.Added || change == InputDeviceChange.Removed)
        {
            UpdateGamepadConnectionStatus();
        }
    }

    private void UpdateGamepadConnectionStatus()
    {
        isGamepadConnected = Gamepad.current != null;
    }
}
