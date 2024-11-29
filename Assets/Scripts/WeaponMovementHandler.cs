using UnityEngine;

public class WeaponMovementHandler : MonoBehaviour
{
    private Vector3 lastPosition;
    private Vector3 currentVelocity;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        currentVelocity = (currentPosition - lastPosition) / Time.deltaTime;

        lastPosition = currentPosition;
    }

    public Vector3 GetWeaponVelocity()
    {
        return currentVelocity;
    }
}