using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponControls : MonoBehaviour
{
    [SerializeField] private WeaponHolder playerWeaponHolder;
    private PlayerInputActions playerInputActions;
    private bool isAttacking;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        EnableInput();
    }

    private void OnDisable()
    {
        DisableInput();
    }

    private void Update()
    {
        if (isAttacking)
        {
            playerWeaponHolder?.Attack();
        }
    }

    private void EnableInput()
    {
        var playerActions = playerInputActions.Player;

        playerActions.Enable();

        playerActions.Attack.started += OnAttackStarted;
        playerActions.Attack.canceled += OnAttackCanceled;
        playerActions.Reload.performed += OnReloadPerformed;
        playerActions.ChooseFirstWeapon.performed += context => SwitchWeaponByIndex(0);
        playerActions.ChooseSecondWeapon.performed += context => SwitchWeaponByIndex(1);
        playerActions.ChooseThirdWeapon.performed += context => SwitchWeaponByIndex(2);
    }

    private void DisableInput()
    {
        var playerActions = playerInputActions.Player;

        playerActions.Attack.started -= OnAttackStarted;
        playerActions.Attack.canceled -= OnAttackCanceled;
        playerActions.Reload.performed -= OnReloadPerformed;
        playerActions.ChooseFirstWeapon.performed -= context => SwitchWeaponByIndex(0);
        playerActions.ChooseSecondWeapon.performed -= context => SwitchWeaponByIndex(1);
        playerActions.ChooseThirdWeapon.performed -= context => SwitchWeaponByIndex(2);

        playerActions.Disable();
    }

    private void OnAttackStarted(InputAction.CallbackContext context) => isAttacking = true;
    private void OnAttackCanceled(InputAction.CallbackContext context) => isAttacking = false;

    private void OnReloadPerformed(InputAction.CallbackContext context) => playerWeaponHolder?.Reload();

    private void SwitchWeaponByIndex(int index) => playerWeaponHolder?.SwitchWeaponByIndex(index);
}