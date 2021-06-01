using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Terrain))]
[RequireComponent(typeof(TerrainCollider))]
public class TerrainManager : MonoBehaviour
{
    [SerializeField] private bool _isRestoreFirstTerrain;
    [SerializeField] private GameObject _sandPrefab;
    [SerializeField] private Terrain _terrain;
    [SerializeField] private float _deformSmoothLevel = 1.0f;
    [SerializeField] private float _deformGain = 0.01f;

    private Vector3 _terrainSize;
    private Vector3 _dimentionRatio;
    private float[,] _terrainHeightmap;
    private float[,] _originalHeightmap;
    private int _terrainHeightmapResolution;
    private List<Vector3> _sandGeneratePoints;
    private string _prefabBoxName = "GeneratedSands";
    private GameObject _prefabBox;

    // Start is called before the first frame update
    private void Start()
    {
        this._terrainSize = this._terrain.terrainData.size;
        this._terrainHeightmapResolution = this._terrain.terrainData.heightmapResolution;
        this._originalHeightmap = this._terrain.terrainData.GetHeights(0, 0, this._terrainHeightmapResolution, this._terrainHeightmapResolution);
        this._terrainHeightmap = this._terrain.terrainData.GetHeights(0, 0, this._terrainHeightmapResolution, this._terrainHeightmapResolution);
        this._dimentionRatio = new Vector3(this._terrainHeightmapResolution / this._terrainSize.x,
                                           1 / this._terrainSize.y,
                                           this._terrainHeightmapResolution / this._terrainSize.z);
        this._prefabBox = new GameObject(this._prefabBoxName);
    }

    private void FixedUpdate()
    {
        this._terrain.terrainData.SetHeightsDelayLOD(0, 0, this._terrainHeightmap);
        this.generateSand();
    }

    private void OnApplicationQuit()
    {
        if (this._isRestoreFirstTerrain)
        {
            this._terrain.terrainData.SetHeightsDelayLOD(0, 0, this._originalHeightmap);
        }
    }

    private void generateSand()
    {
        if (this._sandGeneratePoints != null && this._sandGeneratePoints.Count > 0)
        {
            foreach (Vector3 point in this._sandGeneratePoints)
            {
                GameObject sand = Instantiate(this._sandPrefab, point, Quaternion.identity, _prefabBox.transform);
            }
        }
    }

    private float GetGaussian(float sigma, float dist)
    {
        float step = 1 / Mathf.Sqrt(2 * Mathf.PI * Mathf.Pow(sigma, 2));
        float result = step * Mathf.Exp(-1 * dist * dist / (2 * Mathf.Pow(sigma, 2)));
        return result;
    }

    public bool SetHeightmap(Vector3Int position)
    {
        this._terrainHeightmap[position.z, position.x] = position.y;
        return true;
    }

    public bool SetHeightmap(Vector3 position)
    {
        Vector3Int terrainPos = this.ToTerrainPosition(position);
        this._terrainHeightmap[terrainPos.z, terrainPos.x] = terrainPos.y;
        return true;
    }

    public bool ExcavateWithSand(Vector3[] deformArea)
    {
        float height;
        foreach (Vector3 target in deformArea)
        {
            height = this._terrainHeightmap[(int)(target.z * this._dimentionRatio.z), (int)(target.x * this._dimentionRatio.x)] / this._dimentionRatio.y - target.y;
            if (height > 0)
            {
                this._terrainHeightmap[(int)(target.z * this._dimentionRatio.z), (int)(target.x * this._dimentionRatio.x)] = target.y * this._dimentionRatio.y;
                if ((int)height != 0)
                {
                    for (int i = 0; i < height; i++)
                    {
                        this._sandGeneratePoints.Add(target + new Vector3(0.0f, i, 0.0f));
                    }
                }
            }
        }
        return true;
    }

    public bool ExcavateWithoutSand(Vector3[] deformArea)
    {
        float height;
        foreach (Vector3 target in deformArea)
        {
            height = this._terrainHeightmap[(int)(target.z * this._dimentionRatio.z), (int)(target.x * this._dimentionRatio.x)] / this._dimentionRatio.y - target.y;
            if (height > 0)
            {
                this._terrainHeightmap[(int)(target.z * this._dimentionRatio.z), (int)(target.x * this._dimentionRatio.x)] = target.y * this._dimentionRatio.y;
            }
        }
        return true;
    }

    public bool GaussianDeformation(Vector3 position)
    {
        Vector3Int terrainPos = this.ToTerrainPosition(position);
        Vector3Int pos = new Vector3Int();
        for (int i = this.ToTerrainPositionX(position.x - this._deformSmoothLevel * 3); i < this.ToTerrainPositionX(position.x + this._deformSmoothLevel * 3); i++)
        {
            for (int j = this.ToTerrainPositionZ(position.z - this._deformSmoothLevel * 3); j < this.ToTerrainPositionZ(position.z + this._deformSmoothLevel * 3); j++)
            {
                pos = new Vector3Int(i, 0, j);
                this._terrainHeightmap[pos.z, pos.x] = this._terrainHeightmap[pos.z, pos.x] + this._deformGain * GetGaussian(this._deformSmoothLevel, (pos - terrainPos).magnitude);
            }
        }
        return true;
    }

    public Vector3Int ToTerrainPosition(Vector3 position)
    {
        Vector3 tmp = Vector3.Scale(position, this._dimentionRatio);
        return new Vector3Int((int)tmp.x, (int)tmp.y, (int)tmp.z);
    }

    public Vector2Int ToTerrainPosition(Vector2 position)
    {
        Vector2 tmp = Vector2.Scale(position, new Vector2(this._dimentionRatio.x, this._dimentionRatio.z));
        return new Vector2Int((int)tmp.x, (int)tmp.y);
    }

    public int ToTerrainPositionX(float position)
    {
        return (int)(position * this._dimentionRatio.x);
    }


    public float ToTerrainPositionY(float position)
    {
        return position * this._dimentionRatio.y;
    }


    public int ToTerrainPositionZ(float position)
    {
        return (int)(position * this._dimentionRatio.z);
    }
}
