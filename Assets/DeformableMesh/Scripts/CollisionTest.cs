using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter");
        Debug.Log(LayerMask.LayerToName(collision.gameObject.layer));
        Debug.Log(collision.contacts);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        Debug.Log(LayerMask.LayerToName(other.gameObject.layer));
        Debug.Log(other.ClosestPointOnBounds(this.transform.position));
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStay");
        Debug.Log(LayerMask.LayerToName(other.gameObject.layer));
        Debug.Log(other.ClosestPointOnBounds(this.transform.position));
    }
}
