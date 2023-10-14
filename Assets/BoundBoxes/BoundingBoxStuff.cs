using UnityEngine;

public class BoundingBoxStuff
{
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
            Debug.Log("CalculateBoundingBox: no mesh found on the given object  --- " + _transform.name);
            return new Bounds(_transform.transform.localPosition, Vector3.zero);
        }

        Vector3[] vertices = mesh.vertices;
        if (vertices.Length <= 0)
        {
            Debug.LogError("CalculateBoundingBox: mesh doesn't have vertices  --- " + _transform.name);
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
}