using UnityEngine;
using System.Collections.Generic;


public class BoundBoxes_BoundBox : MonoBehaviour
{
    public static BoundBoxes_BoundBox Instance;
    public bool colliderBased = false;
    public bool permanent = false; //permanent//onMouseDown

    public Color lineColor = new Color(0f, 1f, 0.4f, 0.74f);

    private Bounds bound;

    private Vector3[] corners;

    private Vector3[,] lines;

    private Quaternion quat;

    public Vector3 topFrontLeft;
    public Vector3 topFrontRight;
    public Vector3 topBackLeft;
    public Vector3 topBackRight;
    public Vector3 bottomFrontLeft;
    public Vector3 bottomFrontRight;
    public Vector3 bottomBackLeft;
    public Vector3 bottomBackRight;

    public List<GameObject> targetList;

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        if (targetList.Count < 1)
            return;
    }


    void LateUpdate()
    {
        if (targetList.Count > 0)
        {
            CalculateBounds();
            SetPoints();
            SetLines();
            BoundBoxes_drawLines.Instance.SetOutlines(lines, lineColor);
        }
    }

    //void calculateBounds()
    //{

    //    bound = new Bounds();

    //    for (int i = 0; i < meshes.Count; i++)
    //    {
    //        Mesh ms = meshes[i].mesh;
    //        Vector3 tr = meshes[i].gameObject.transform.position;
    //        Vector3 ls = meshes[i].gameObject.transform.lossyScale;
    //        Quaternion lr = meshes[i].gameObject.transform.rotation;
    //        int vc = ms.vertexCount;
    //        for (int j = 0; j < vc; j++)
    //        {
    //            if (i == 0 && j == 0)
    //            {
    //                bound = new Bounds(tr + lr * Vector3.Scale(ls, ms.vertices[j]), Vector3.zero);
    //            }
    //            else
    //            {
    //                bound.Encapsulate(tr + lr * Vector3.Scale(ls, ms.vertices[j]));
    //            }
    //        }
    //    }

    //    transform.localPosition = bound.center;
    //}

    void CalculateBounds()
    {
        bound = new Bounds();
        for (int i = 0; i < targetList.Count; i++)
        {
            BoundingBoxStuff.CalculateBoundingBox(targetList[i].transform, ref bound);
        }
        transform.localPosition = bound.center;
    }
    public static void CalculateBoundingBox(Transform _transform, ref Bounds _currentBounds)
    {
        Renderer _renderer = _transform.GetComponent<Renderer>();
        if (_renderer != null && _transform.gameObject.activeInHierarchy)
        {
            if (_currentBounds.size == Vector3.zero)
                _currentBounds = CalculateBoundingBoxSingle(_transform);
            else
                _currentBounds.Encapsulate(CalculateBoundingBoxSingle(_transform));
        }

        for (int i = 0; i < _transform.childCount; i++)
        {
            CalculateBoundingBox(_transform.GetChild(i), ref _currentBounds);
        }
    }
    private static Bounds CalculateBoundingBoxSingle(Transform _transform)
    {
        Transform myTransform = _transform.transform;
        Mesh mesh = null;
        MeshFilter mF = _transform.GetComponent<MeshFilter>();
        if (mF != null)
            mesh = mF.mesh;
        else
        {
            SkinnedMeshRenderer sMR = _transform.GetComponent<SkinnedMeshRenderer>();
            if (sMR != null)
                mesh = sMR.sharedMesh;
        }


        if (mesh == null)
        {
            //Debug.Log("CalculateBoundingBox: no mesh found on the given object  --- " + _transform.name);
            return new Bounds(_transform.transform.localPosition, Vector3.zero);
        }

        Vector3[] vertices = mesh.vertices;
        if (vertices.Length <= 0)
        {
            //Debug.LogError("CalculateBoundingBox: mesh doesn't have vertices  --- " + _transform.name);
            return new Bounds(_transform.transform.localPosition, Vector3.zero);
        }
        Vector3 min, max;
        min = max = myTransform.TransformPoint(vertices[0]);
        for (int i = 1; i < vertices.Length; i++)
        {
            Vector3 V = myTransform.TransformPoint(vertices[i]);
            for (int n = 0; n < 3; n++)
            {
                if (V[n] > max[n])
                    max[n] = V[n];
                if (V[n] < min[n])
                    min[n] = V[n];
            }
        }
        Bounds B = new Bounds();
        B.SetMinMax(min, max);
        return B;
    }



    void SetPoints()
    {

        //Vector3 bc = transform.position + quat * (bound.center - transform.position);
        Vector3 bc = bound.center;
        //quat = Quaternion.identity;
        topFrontRight = bc + Vector3.Scale(bound.extents, new Vector3(1, 1, 1));
        topFrontLeft = bc + Vector3.Scale(bound.extents, new Vector3(-1, 1, 1));
        topBackLeft = bc + Vector3.Scale(bound.extents, new Vector3(-1, 1, -1));
        topBackRight = bc + Vector3.Scale(bound.extents, new Vector3(1, 1, -1));
        bottomFrontRight = bc + Vector3.Scale(bound.extents, new Vector3(1, -1, 1));
        bottomFrontLeft = bc + Vector3.Scale(bound.extents, new Vector3(-1, -1, 1));
        bottomBackLeft = bc + Vector3.Scale(bound.extents, new Vector3(-1, -1, -1));
        bottomBackRight = bc + Vector3.Scale(bound.extents, new Vector3(1, -1, -1));
        corners = new Vector3[] { topFrontRight, topFrontLeft, topBackLeft, topBackRight, bottomFrontRight, bottomFrontLeft, bottomBackLeft, bottomBackRight };

    }

    void SetLines()
    {

        int i1;
        int linesCount = 12;

        lines = new Vector3[linesCount, 2];
        for (int i = 0; i < 4; i++)
        {
            i1 = (i + 1) % 4;//top rectangle
            lines[i, 0] = corners[i];
            lines[i, 1] = corners[i1];
            //break;
            i1 = i + 4;//vertical lines
            lines[i + 4, 0] = corners[i];
            lines[i + 4, 1] = corners[i1];
            //bottom rectangle
            lines[i + 8, 0] = corners[i1];
            i1 = 4 + (i + 1) % 4;
            lines[i + 8, 1] = corners[i1];
        }
    }
}
