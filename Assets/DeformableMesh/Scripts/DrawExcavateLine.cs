using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawExcavateLine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // LineRenderer�R���|�[�l���g���Q�[���I�u�W�F�N�g�ɃA�^�b�`����
        var lineRenderer = gameObject.AddComponent<LineRenderer>();

        var positions = new Vector3[]{
        new Vector3(0, 0, 0),               // �J�n�_
        new Vector3(8, 0, 0),               // �I���_
        };

        // ���������ꏊ���w�肷��
        lineRenderer.SetPositions(positions);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
