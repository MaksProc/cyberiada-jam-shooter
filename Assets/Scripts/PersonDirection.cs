using UnityEngine;

public class PersonDirection : MonoBehaviour
{
    [SerializeField] Transform personTransf;
    [SerializeField] Transform directionSphereTransf;
    [SerializeField] float directionSphereOffSet;
    public void Direct(Vector2 direction) 
    {
        Vector3 vectPos = new Vector3(personTransf.position.x + (direction.x * directionSphereOffSet), directionSphereTransf.position.y, personTransf.position.z + (direction.y * directionSphereOffSet));
        directionSphereTransf.position = vectPos;

        personTransf.LookAt(vectPos);

        personTransf.eulerAngles = new Vector3(0, personTransf.eulerAngles.y, 0);
    }
}