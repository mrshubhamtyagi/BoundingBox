using UnityEngine;
using System.Collections.Generic;


public class BoundBoxes_drawLines : MonoBehaviour
{
    public Material lineMaterial;
    public Color lColor;
    List<Vector3[,]> outlines;
    public List<Color> colors;
    // Use this for initialization

    public static BoundBoxes_drawLines Instance;

    void Awake()
    {
        Instance = this;
        outlines = new List<Vector3[,]>();
        colors = new List<Color>();
        CreateLineMaterial();
    }

    void Start()
    {
        //		outlines = new List<Vector3[,]>();
        //		colors = new List<Color>();
        //		CreateLineMaterial();
    }

    void CreateLineMaterial()
    {
        if (!lineMaterial)
        {

            lineMaterial = new Material(Shader.Find("Custom/GizmoShader"));

            //lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            //lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
        }
    }

    void OnPostRender()
    {
        if (outlines == null) return;
        CreateLineMaterial();
        lineMaterial.SetPass(0);
        GL.Begin(GL.LINES);
        for (int j = 0; j < outlines.Count; j++)
        {
            GL.Color(colors[j]);
            for (int i = 0; i < outlines[j].GetLength(0); i++)
            {
                GL.Vertex(outlines[j][i, 0]);
                GL.Vertex(outlines[j][i, 1]);
            }
        }
        GL.End();
    }

    //void OnPostRender()
    //{
    //	if (outlines == null) return;
    //	CreateLineMaterial();
    //	lineMaterial.SetPass(0);
    //	GL.Begin(GL.QUADS);
    //	//for (int j = 0; j < outlines.Count; j++)
    //	//{
    //	//	GL.Color(colors[j]);
    //	//	for (int i = 0; i < outlines[j].GetLength(0); i++)
    //	//	{
    //	//		GL.Vertex(outlines[j][i, 0]);
    //	//		GL.Vertex(outlines[j][i, 1]);
    //	//	}
    //	//}
    //	GL.Color(BoundBoxes_BoundBox.instanceBoundingBox.lineColor);
    //	GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.bottomFrontLeft);
    //	GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.bottomBackLeft);
    //	GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.bottomBackRight);
    //	GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.bottomFrontRight);

    //	GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.topFrontLeft);
    //	GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.topBackLeft);
    //	GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.topBackRight);
    //	GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.topFrontRight);

    //	GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.bottomFrontLeft);
    //	GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.topFrontLeft);
    //	GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.topFrontRight);
    //	GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.bottomFrontRight);

    //       GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.bottomBackLeft);
    //       GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.topBackLeft);
    //       GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.topBackRight);
    //       GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.bottomBackRight);

    //	GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.bottomFrontLeft);
    //	GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.bottomBackLeft);
    //	GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.topBackLeft);
    //	GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.topFrontLeft);

    //	GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.bottomFrontRight);
    //	GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.bottomBackRight);
    //	GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.topBackRight);
    //	GL.Vertex(BoundBoxes_BoundBox.instanceBoundingBox.topFrontRight);


    //	GL.End();
    //}

    public void SetOutlines(Vector3[,] newOutlines, Color newcolor)
    {
        if (newOutlines.GetLength(0) > 0)
        {
            if (outlines != null)
                outlines.Add(newOutlines);
            //Debug.Log ("no "+newOutlines.GetLength(0).ToString());
            if (colors != null)
                colors.Add(newcolor);
        }
    }

    // Update is called once per frame
    void Update()
    {
        outlines = new List<Vector3[,]>();
        colors = new List<Color>();
    }

}
