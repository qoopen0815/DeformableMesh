using System.Collections.Generic;
using UnityEngine;

public class GPUDeform : MonoBehaviour
{
    public MeshFilter mf;
    public MeshCollider mc;

    private Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        //The Mesh
        mesh = mf.mesh;
        mesh.name = "My Mesh";
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] newVerts = deformMesh(mesh.vertices);

        //Update mesh
        mesh.MarkDynamic();
        mesh.SetVertices(newVerts);
        mesh.RecalculateNormals();

        //Update to collider
        mc.sharedMesh = mesh;
    }

    Vector3[] deformMesh(Vector3[] mesh)
    {
        List<Vector3> newMesh = new List<Vector3>();

        foreach (Vector3 pos in mesh)
        {
            Vector3 depressionPoint = pos;
            float dist = depressionPoint.magnitude;
            depressionPoint.z = Mathf.Sin(Time.time * 2.0f + dist) * 0.25f;
            newMesh.Add(depressionPoint);
        }

        return newMesh.ToArray();
    }
}
