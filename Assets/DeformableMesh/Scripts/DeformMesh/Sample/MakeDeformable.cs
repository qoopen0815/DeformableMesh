using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
public class MakeDeformable : MonoBehaviour
{
    public float collisionRadius = 0.1f;
    public float maximumDepression;
    public Vector3[] originalVertices;
    public Vector3[] modifiedVertices;
    public List<bool> isModifiedVertices;

    public GameObject SandModel;
    private MeshFilter filter;
    private new MeshCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        //sphere = (GameObject)Resources.Load("Sphere");
        collider = GetComponent<MeshCollider>();
        filter = GetComponent<MeshFilter>();

        collider.sharedMesh.MarkDynamic();
        filter.mesh.MarkDynamic();
        originalVertices = filter.mesh.vertices;
        modifiedVertices = filter.mesh.vertices;
        isModifiedVertices = new List<bool>(originalVertices.Length);
        for (int i = 0; i < originalVertices.Length; i++) isModifiedVertices.Add(false);
    }

    private void FixedUpdate()
    {
        collider.sharedMesh = filter.mesh;
    }

    public void AddDepression(Vector3 depressionPoint, float radius)
    {
        var worldPos4 = this.transform.worldToLocalMatrix * depressionPoint;
        var worldPos = new Vector3(worldPos4.x, worldPos4.y, worldPos4.z);

        for (int i = 0; i < modifiedVertices.Length; ++i)
        {
            var distance = (worldPos - (modifiedVertices[i] + Vector3.down * maximumDepression)).magnitude;
            if (distance < radius)
            {
                Debug.Log(i);
                var newVert = originalVertices[i] + Vector3.back * maximumDepression;
                modifiedVertices[i].Set(newVert.x, newVert.y, newVert.z);

                if (!isModifiedVertices[i])
                {
                    GameObject gameObject1 = Instantiate(SandModel, depressionPoint + (0.005f * Vector3.down), Quaternion.identity);
                    //isModifiedVertices[i] = !isModifiedVertices[i];
                }
            }
        }

        collider.sharedMesh.SetVertices(modifiedVertices);
        filter.mesh.SetVertices(modifiedVertices);
        //Debug.Log("Mesh Depressed");
    }

    private void OnCollisionStay(Collision collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) != "SandPrefab") {
            foreach (var contacts in collision.contacts) {
                AddDepression(contacts.point, collisionRadius);
            }
        }
    }
}
