using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
public class ExcavateLine : MonoBehaviour
{
    [SerializeField] private string _targetTarrainName = "Terrain";
    [SerializeField] private string _targetLayerName = "Terrain";
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Vector3 _lineCenterPos = new Vector3();
    [SerializeField] private float _lineLength;

    private GameObject _targetTerrain;
    private Vector3[] _lineEnd;

    // Start is called before the first frame update
    void Start()
    {
        CapsuleCollider collider = this.gameObject.AddComponent<CapsuleCollider>();
        this._targetTerrain = GameObject.Find(this._targetTarrainName);
        collider.isTrigger = true;
        collider.direction = 2;
        collider.radius = 0.01f;
        collider.height = this._lineLength / 6;
        collider.center = this._lineCenterPos / 4;
    }

    // Update is called once per frame
    void Update()
    {
        this._lineEnd = new Vector3[] { new Vector3(0, 0, -this._lineLength/2), new Vector3(0, 0, this._lineLength / 2) };
        this._lineRenderer.SetPositions(new Vector3[] { this.transform.position + this._lineCenterPos + this._lineEnd[0], this.transform.position + this._lineCenterPos + this._lineEnd[1] });
    }

    private void OnDisable()
    {
        GameObject.Destroy(this.GetComponent<CapsuleCollider>());
    }

    private void OnTriggerStay(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == this._targetLayerName)
        {
            this._targetTerrain.GetComponent<TerrainManager>().ExcavateWithSand(this.GetDeformArea(this.transform.position + this._lineCenterPos + this._lineEnd[0], 
                                                                                                      this.transform.position + this._lineCenterPos + this._lineEnd[1]));
        }
    }

    private Vector3[] GetDeformArea(Vector3 start, Vector3 end)
    {
        List<Vector3> DeformVerts = new List<Vector3>();
        int loopNum = 17;
        for (float i = 0.0f; i <= 1.0f; i += 1.0f / (float)loopNum)
        {
            DeformVerts.Add(i * start + (1 - i) * end);
        }

        return DeformVerts.ToArray();
    }
}
