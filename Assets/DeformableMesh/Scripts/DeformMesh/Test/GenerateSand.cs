using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Terrain))]
[RequireComponent(typeof(TerrainCollider))]
public class GenerateSand : MonoBehaviour
{
    public float offset;
    
    private GameObject sandModel;
    private Terrain _terrain;
    private Vector3 _terrainSize;
    private float[,] _terrainHeightmap;
    private int _terrainHeightmapResolution;
    private TerrainCollider _terrainCollider;

    // Start is called before the first frame update
    void Start()
    {
        this._terrain = this.GetComponent<Terrain>();
        this._terrainCollider = this.GetComponent<TerrainCollider>();
        this._terrainSize = this._terrain.terrainData.size;
        this._terrainHeightmapResolution = this._terrain.terrainData.heightmapResolution;
        sandModel = (GameObject)Resources.Load("Icosphere");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "EndEffector")
        {
            foreach (ContactPoint point in collision.contacts)
            {
                int dist;
                this._terrainHeightmap = this._terrain.terrainData.GetHeights(0, 0, this._terrainHeightmapResolution, this._terrainHeightmapResolution);
                for (int i=-3; i<=3; i++)
                {
                    dist = (int)(this._terrainHeightmap[(int)(point.point.z * this._terrainHeightmapResolution / this._terrainSize.z) + i, (int)(point.point.x * this._terrainHeightmapResolution / this._terrainSize.x) + i] - (point.point.y - offset) / this._terrainSize.y);
                    this._terrainHeightmap[(int)(point.point.z * this._terrainHeightmapResolution / this._terrainSize.z)+i, (int)(point.point.x * this._terrainHeightmapResolution / this._terrainSize.x)+i]
                       = (point.point.y - offset) / this._terrainSize.y;
                    //Instantiate(sandModel, 
                    //            new Vector3((int)(point.point.z * this._terrainHeightmapResolution / this._terrainSize.z) + i,
                    //                        this._terrainHeightmap[(int)(point.point.z * this._terrainHeightmapResolution / this._terrainSize.z) + i, (int)(point.point.x * this._terrainHeightmapResolution / this._terrainSize.x) + i], 
                    //                        (int)(point.point.x * this._terrainHeightmapResolution / this._terrainSize.x) + i), 
                    //            Quaternion.identity);
                }
                //this._terrainHeightmap[(int)(point.point.z * this._terrainHeightmapResolution / this._terrainSize.z), (int)(point.point.x * this._terrainHeightmapResolution / this._terrainSize.x)] 
                //    = (point.point.y - offset) / this._terrainSize.y;
                this._terrain.terrainData.SetHeightsDelayLOD(0, 0, this._terrainHeightmap);
            }
        }
    }
}
