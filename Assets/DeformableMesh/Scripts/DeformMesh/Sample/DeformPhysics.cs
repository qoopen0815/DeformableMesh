using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformPhysics : MonoBehaviour
{
    public float collisionRadius = 0.1f;
    public MakeDeformable deformableMesh;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionStay(Collision collision)
    {
        foreach (var contacts in collision.contacts)
        {
            deformableMesh.AddDepression(contacts.point, collisionRadius);
        }
    }
}
