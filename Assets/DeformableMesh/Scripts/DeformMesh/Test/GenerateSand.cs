using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GenerateSand : MonoBehaviour
{
    [SerializeField]
    private LineRenderer _lineRenderer;

    public Vector3 lineStart;
    public Vector3 lineEnd;
    public float offset;
    
    [SerializeField] private Terrain _targetTerrain;
    private Vector3 _terrainSize;
    private float[,] _terrainHeightmap;
    private int _terrainHeightmapResolution;
    private GameObject _sandModel;

    private Vector3 _dimentionRatio;

    // Start is called before the first frame update
    void Start()
    {
        this._terrainSize = this._targetTerrain.terrainData.size;
        this._terrainHeightmapResolution = this._targetTerrain.terrainData.heightmapResolution;
        this._sandModel = (GameObject)Resources.Load("Icosphere");
        this._dimentionRatio = new Vector3(this._terrainHeightmapResolution / this._terrainSize.x,
                                           1 / this._terrainSize.y,
                                           this._terrainHeightmapResolution / this._terrainSize.z);
    }

    // Update is called once per frame
    void Update()
    {
        lineStart = transform.TransformPoint( new Vector3(this.transform.localScale.x / 10, 0, this.transform.localScale.z / 10) );
        lineEnd = transform.TransformPoint( new Vector3(-this.transform.localScale.x / 10, 0, this.transform.localScale.z / 10) );
        _lineRenderer.SetPositions(new Vector3[] { lineStart, lineEnd });
    }

    private void OnTriggerStay(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Terrain")
        {
            this._terrainHeightmap = this._targetTerrain.terrainData.GetHeights(0, 0, this._terrainHeightmapResolution, this._terrainHeightmapResolution);
            Vector3[] targets = this.GetDeformArea(lineStart, lineEnd);
            float height;
            foreach (Vector3 target in targets)
            {
                height = this._terrainHeightmap[(int)(target.z * this._dimentionRatio.z), (int)(target.x * this._dimentionRatio.x)] - target.y * this._dimentionRatio.y;
                if (height > 0)
                {
                    //Debug.Log(height);
                    this._terrainHeightmap[(int)(target.z * this._dimentionRatio.z), (int)(target.x * this._dimentionRatio.x)] = target.y * this._dimentionRatio.y;
                    //for (int i = 0; i < height; i++)
                    //{
                    //    Debug.Log("instantiate");
                    //    GameObject sand = Instantiate(_sandModel, target + new Vector3(0.0f, i / this._dimentionRatio.y, 0.0f), Quaternion.identity);
                    //}
                }
            }
            this._targetTerrain.terrainData.SetHeightsDelayLOD(0, 0, this._terrainHeightmap);
        }
    }

    private Vector3[] GetDeformArea(Vector3 start, Vector3 end)
    {
        // Patern 1
        //Vector3 distVec = end - start;
        //float dist = 0.1f;
        //int vertNum = (int)(distVec.magnitude / dist);
        //List<Vector3> DeformVerts = new List<Vector3>();
        //Vector3 DeformVert = new Vector3();
        //for (int i = 0; i < vertNum; i++)
        //{
        //    DeformVert = start + distVec * i / vertNum;
        //    DeformVerts.Add(DeformVert);
        //}

        // Patern 2
        List<Vector3> DeformVerts = new List<Vector3>();
        int loopNum = 15;
        for (float i = 0.0f; i <= 1.0f; i += 1.0f / (float)loopNum)
        {
            DeformVerts.Add( i * start + (1 - i) * end );
        }

        return DeformVerts.ToArray();
    }
}
