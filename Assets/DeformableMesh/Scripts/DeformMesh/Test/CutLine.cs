    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CutLine : MonoBehaviour
{
    [SerializeField]
    private GameObject _line;
    [SerializeField]
    private LineRenderer _lineRenderer;

    public Vector3 start, end;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _line.transform.position = Vector3.Scale( this.transform.position, new Vector3(2,0f,2) );
        _line.transform.rotation = this.transform.rotation;
        start = this.transform.position + Vector3.Scale(this.transform.localScale, new Vector3(-0.5f, 0f, -0.5f));
        end = this.transform.position + Vector3.Scale(this.transform.localScale, new Vector3(0.5f, 0f, -0.5f));
        _lineRenderer.SetPositions(new Vector3[] { start, end });
    }
}
