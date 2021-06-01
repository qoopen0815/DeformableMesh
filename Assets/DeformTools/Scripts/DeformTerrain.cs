using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DeformTerrain : MonoBehaviour
{
    [SerializeField] private float _sleepThreshold = 10f;
    [SerializeField] private string _targetTarrainName = "Terrain";

    private Rigidbody _rigidBody;
    private GameObject _targetTerrain;

    // Start is called before the first frame update
    private void Start()
    {
        this._rigidBody = this.GetComponent<Rigidbody>();
        this._rigidBody.sleepThreshold = this._sleepThreshold;
        this._targetTerrain = GameObject.Find(this._targetTarrainName);
    }

    private void Update()
    {
        if (this._rigidBody.IsSleeping())
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (this._targetTerrain != null)
        {
            this._targetTerrain.GetComponent<TerrainManager>().GaussianDeformation(this.transform.position);
        }
    }
}
