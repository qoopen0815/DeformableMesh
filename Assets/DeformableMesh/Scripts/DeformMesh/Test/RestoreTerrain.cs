using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreTerrain : MonoBehaviour
{
    public Terrain terrain;
    private float[,] _originalHeightmap;

    // Start is called before the first frame update
    void Start()
    {
        this._originalHeightmap = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        terrain.terrainData.SetHeightsDelayLOD(0, 0, this._originalHeightmap);
    }
}
