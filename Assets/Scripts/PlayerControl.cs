using UnityEngine;
public class PlayerControl : MonoBehaviour
{
    [SerializeField] PersonMovement personMovement;
    [SerializeField] PersonFire personFire;
    [SerializeField] PersonDirection personDirection;
    [Range(0.1f,1)]
    [SerializeField] float timeScale = 1;

    InputMaster inputMaster;
    Vector2 direction;

    Vector2 inputVectorMove;
    Vector2 inputVectorAim;

    private void Awake()
    {
        inputMaster = new InputMaster();

        inputMaster.Player.Reload.performed += ctx => personFire.Reload();
    }

    private void Update()
    {
        Time.timeScale = timeScale;

        inputVectorMove = inputMaster.Player.Move.ReadValue<Vector2>();
        personMovement.Move(inputVectorMove);
        inputVectorAim = inputMaster.Player.Aim.ReadValue<Vector2>();

        if (inputVectorAim != Vector2.zero)
            direction = inputVectorAim;
        else if (inputVectorMove != Vector2.zero)
            direction = inputVectorMove;

        if (direction != Vector2.zero)
            personDirection.Direct(direction);

        if(inputMaster.Player.Fire.IsPressed())
            personFire.Fire();
    }

    private void OnEnable()
    {
        inputMaster.Player.Enable();
    }

    private void OnDisable()
    {
        inputMaster.Player.Disable();
    }
}