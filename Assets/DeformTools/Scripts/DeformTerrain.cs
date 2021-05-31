using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DeformTerrain : MonoBehaviour
{
    public Terrain terrain;
    public float effectLevel;
    public float effectRate;

    private Rigidbody _rigidBody;
    private Vector3 _terrainSize;
    private float[,] _terrainHeightmap;
    private int _terrainHeightmapResolution;

    // Start is called before the first frame update
    private void Start()
    {
        this._rigidBody = this.GetComponent<Rigidbody>();
        this._terrainSize = terrain.terrainData.size;
        this._terrainHeightmapResolution = terrain.terrainData.heightmapResolution;
    }

    private void Update()
    {
        if (this._rigidBody.IsSleeping())
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        Vector2Int vertPos = new Vector2Int();
        Vector2 pos = new Vector3( this.transform.position.x * this._terrainHeightmapResolution / this._terrainSize.x ,
                                   this.transform.position.z * this._terrainHeightmapResolution / this._terrainSize.z );

        this._terrainHeightmap = terrain.terrainData.GetHeights(0, 0, this._terrainHeightmapResolution, this._terrainHeightmapResolution);
        for (vertPos.x = 0; vertPos.x < this._terrainHeightmapResolution; vertPos.x++)
        {
            for (vertPos.y = 0; vertPos.y < this._terrainHeightmapResolution; vertPos.y++)
            {
                this._terrainHeightmap[vertPos.y, vertPos.x] = this.terrain.terrainData.GetHeight(vertPos.x, vertPos.y) / this._terrainSize.y + effectRate * GetGaussian(effectLevel, (vertPos - pos).magnitude);
            }
        }
        terrain.terrainData.SetHeightsDelayLOD(0, 0, this._terrainHeightmap);
    }

    private float GetGaussian(float sigma, float dist)
    {
        float step = 1 / Mathf.Sqrt(2 * Mathf.PI * Mathf.Pow(sigma, 2));
        float result = step * Mathf.Exp(-1 * dist * dist / (2 * Mathf.Pow(sigma, 2)));
        return result;
    }
}