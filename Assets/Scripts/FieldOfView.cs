using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class FieldOfView : MonoBehaviour
{
    [Header("Field of View Settings")]
    [SerializeField] private float fov = 90f;           // Field of view angle (degrees)
    [SerializeField] private float viewDistance = 50f;  // Maximum view distance
    [SerializeField] private float angleStep = 1f;      // Angle between rays (degrees)
    [SerializeField] private LayerMask layerMask;       // Layers to detect collisions
    [SerializeField] private Color onEnemyColor = new Color(1f, 0f, 0f, 0);

    [Header("References")]
    [SerializeField] private Transform fovTransf;       // The transform to use as the origin
    [SerializeField] private MeshRenderer meshRenderer; // Material to change the color of the FOV mesh

    private Mesh mesh;
    private Material runtimeMat;
    private Color originalColor;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // Clone the material and store the original color
        runtimeMat = new Material(meshRenderer.material);
        meshRenderer.material = runtimeMat;
        originalColor = runtimeMat.GetColor("_EmissionColor");
    }

    private void Update()
    {
        GenerateFieldOfViewMesh();
    }

    private void GenerateFieldOfViewMesh()
    {
        int rayCount = Mathf.CeilToInt(fov / angleStep) + 1;
        Vector3[] vertices = new Vector3[rayCount + 1]; // One extra for the origin
        int[] triangles = new int[(rayCount - 1) * 3];  // Each triangle needs 3 points

        vertices[0] = Vector3.zero; // Origin of the field of view

        int vertexIndex = 1;
        int triangleIndex = 0;

        bool hitEnemy = false; // Flag to detect if an enemy is hit

        for (int i = 0; i < rayCount; i++)
        {
            float angle = -fov / 2 + angleStep * i;
            float worldAngle = angle - fovTransf.eulerAngles.y;
            Vector3 direction = AngleToDirection(worldAngle);
            Vector3 rayEndpoint = direction * viewDistance;

            bool ishitted = Physics.Raycast(fovTransf.position, direction, out RaycastHit hit, viewDistance, layerMask);

            Vector3 vertex;
            // Check if the ray hit an object with the "Enemy" tag
            if (ishitted)
            {
                if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                {
                    hitEnemy = true;
                    vertex = fovTransf.InverseTransformPoint(fovTransf.position + rayEndpoint); // Otherwise, max range
                }
                else
                {
                    vertex = fovTransf.InverseTransformPoint(hit.point);
                }
            }
            else
            {
                vertex = fovTransf.InverseTransformPoint(fovTransf.position + rayEndpoint); // Otherwise, max range
            }

            vertices[vertexIndex] = new Vector3(vertex.x, 0, vertex.z);

            if (i > 0)
            {
                triangles[triangleIndex++] = 0;
                triangles[triangleIndex++] = vertexIndex - 1;
                triangles[triangleIndex++] = vertexIndex;
            }

            vertexIndex++;

            Debug.DrawLine(fovTransf.position, fovTransf.position + direction * viewDistance, hitEnemy ? Color.red : Color.green);
        }

        // Assign the generated data to the mesh
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Update the material color if an enemy was hit
        runtimeMat.SetColor("_EmissionColor", hitEnemy ? onEnemyColor : originalColor);
    }

    private Vector3 AngleToDirection(float angleInDegrees)
    {
        float radian = angleInDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(radian), 0, Mathf.Sin(radian));
    }

    public void UpdateFOV(float newFOV)
    {
        fov = Mathf.Clamp(newFOV, 0f, 360f);
    }

    public void setViewDistance(float viewDistance)
    {
        if (viewDistance >= 0) this.viewDistance = viewDistance;
    }
}
