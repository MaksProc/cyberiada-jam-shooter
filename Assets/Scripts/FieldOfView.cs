using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class FieldOfView : MonoBehaviour {

    [SerializeField] Transform transf;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float angleBetweenRays;
    private Mesh mesh;
    [SerializeField] private float fov = 90f;
    [SerializeField] private float viewDistance = 50f;
    [SerializeField] private Vector3 origin = Vector3.zero;

    private void Start() 
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void LateUpdate() 
    {
        int rayCount = Mathf.CeilToInt(fov / angleBetweenRays) + 1;

        Vector3[] vertices = new Vector3[(rayCount + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[((rayCount - 1) * 3)];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i < rayCount; i++)
        {
            Vector3 vertex;
            float vertexAngle = angleBetweenRays * i - fov / 2 + 90;
            Vector3 vertexMaxPosLoc = UtilsClass.GetVectorFromAngle(vertexAngle) * viewDistance;
            Vector3 vertexMaxPos = UtilsClass.GetVectorFromAngle(vertexAngle - transf.eulerAngles.y) * viewDistance;

            Debug.DrawLine(transf.position, vertexMaxPos + transf.position);

            if (Physics.Raycast(transf.position, vertexMaxPos, out RaycastHit raycastHit, viewDistance, layerMask))
            {
                Vector3 vertexLoc = transf.InverseTransformPoint(raycastHit.point);
                vertex = new Vector3(vertexLoc.x, 0, vertexLoc.z);
                //Debug.Log(vertex);
            }
            else
            {
                vertex = origin + vertexMaxPosLoc;
            }

            vertices[vertexIndex] = vertex;
            
            if (i > 0)
            {
                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
    }

    //public void SetOrigin(Vector3 origin) {
    //    this.origin = origin;
    //}

    //public void SetAimDirection(Vector3 aimDirection) {
    //    startingAngle = UtilsClass.GetAngleFromVectorFloat(aimDirection) + fov / 2f;
    //}

    //public void SetFoV(float fov) {
    //    this.fov = fov;
    //}

    //public void SetViewDistance(float viewDistance) {
    //    this.viewDistance = viewDistance;
    //}
}