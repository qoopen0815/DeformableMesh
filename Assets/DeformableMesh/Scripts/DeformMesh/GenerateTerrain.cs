using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{
    public Terrain terrain;
    public Vector3Int terrainSize;
    public enum Level
    {
        Level0_32,
        Level1_64,
        Level2_128,
        Level3_256,
        Level4_512,
        Level5_1024,
        Level6_2048,
        Level7_4096
    }
    [SerializeField] Level resolutionLevel = Level.Level3_256;

    private float[,] originalHeightmap;
    private float[,] modifiedHeightmap;
    private int resolution;

    // Start is called before the first frame update
    void Start()
    {
        terrain.terrainData.size = terrainSize;
        resolution = 32 * (int)Mathf.Pow(2, (int)resolutionLevel);
     
        // ‰ğ‘œ“x‚Ìİ’è
        terrain.terrainData.heightmapResolution = resolution + 1;

        // heightmap‚ğæ“¾‚·‚é
        originalHeightmap = terrain.terrainData.GetHeights(0, 0, resolution + 1, resolution + 1);
        modifiedHeightmap = terrain.terrainData.GetHeights(0, 0, resolution + 1, resolution + 1);

        Vector2Int pos = new Vector2Int();
        Vector2Int center = new Vector2Int(resolution / 3, resolution / 3);

        for (int i=0; i<resolution; i++)
        {
            for(int j=0; j<resolution; j++)
            {
                pos.x = i; pos.y = j;
                modifiedHeightmap[pos.y, pos.x] = Gaussian(25f, (pos - center).magnitude);
            }
        }
        terrain.terrainData.SetHeightsDelayLOD(0, 0, modifiedHeightmap);
    }

    private void OnDisable()
    {
        terrain.terrainData.SetHeightsDelayLOD(0, 0, originalHeightmap);
    }

    private float Gaussian(float sigma, float dist)
    {
        float step = 1 / Mathf.Sqrt(2 * Mathf.PI * Mathf.Pow(sigma, 2));
        float result = step * Mathf.Exp(-1 * dist * dist / (2 * Mathf.Pow(sigma, 2)));
        return result;
    }
}