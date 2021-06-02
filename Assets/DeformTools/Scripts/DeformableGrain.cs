using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DeformableGrain : MonoBehaviour
{
    [SerializeField] private float _sleepThreshold = 10f;
    [SerializeField] private string _targetTarrainName = "Terrain";
    [SerializeField] private string _bucketLayerName = "Bucket";

    private Rigidbody _rigidBody;
    private GameObject _targetTerrain;
    private bool _isDeformable;

    // Start is called before the first frame update
    private void Start()
    {
        this._rigidBody = this.GetComponent<Rigidbody>();
        this._rigidBody.sleepThreshold = this._sleepThreshold;
        this._targetTerrain = GameObject.Find(this._targetTarrainName);
        this._isDeformable = true;
    }

    private void Update()
    {
        if (this._rigidBody.IsSleeping())
        {
            if (this._isDeformable)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        if (this._targetTerrain != null)
        {
            this._targetTerrain.GetComponent<TerrainManager>().GaussianDeformation(this.transform.position);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == this._bucketLayerName)
        {
            this._isDeformable = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == this._bucketLayerName)
        {
            this._isDeformable = true;
        }
    }
}