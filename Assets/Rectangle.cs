using UnityEngine;
using System.Collections;

public class Rectangle : MonoBehaviour
{
    public Vector3 vertLeftTopFront = new Vector3(-1, 1, 1);
    public Vector3 vertRightTopFront = new Vector3(1, 1, 1);
    public Vector3 vertRightTopBack = new Vector3(1, 1, -1);
    public Vector3 vertLeftTopBack = new Vector3(-1, 1, -1);
    public Vector3 vertLeftBottomFront = new Vector3(-1, -1, 1);
    public Vector3 vertRightBottomFront = new Vector3(1, -1, 1);
    public Vector3 vertRightBottomBack = new Vector3(1, -1, -1);
    public Vector3 vertLeftBottomBack = new Vector3(-1, -1, -1);
    // Use this for initialization
    void Start()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = mf.mesh;
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.materials[0] = (Material)Resources.Load("");

        float LayerT = 20;
        float LayerB = 10;
        mf.transform.position = new Vector3(28,LayerT,10);
        Vector3 v3 = new Vector3(28, LayerT, 10);

        float x = 28;
        float y = 10;
        vertLeftTopFront = new Vector3(x, LayerT / 1.0f, y) - v3;
        vertLeftBottomFront = new Vector3(x , LayerB / 1.0f, y) - v3;

        x = 28;
        y = 12;
        vertRightTopFront = new Vector3(x, LayerT / 1.0f, y) - v3;
        vertRightBottomFront = new Vector3(x, LayerB / 1.0f, y) - v3;
        x = 24;
        y = 12;
        vertRightTopBack = new Vector3(x, LayerT / 1.0f, y) - v3;
        vertRightBottomBack = new Vector3(x, LayerB / 1.0f, y) - v3;
        x = 24;
        y = 10;
        vertLeftTopBack = new Vector3(x, LayerT / 1.0f, y) - v3;
        vertLeftBottomBack = new Vector3(x, LayerB / 1.0f, y) - v3;

        //Vertices//
        Vector3[] vertices = new Vector3[]
        {
            //front face//
            vertLeftTopFront,//left top front, 0
            vertRightTopFront,//right top front, 1
            vertLeftBottomFront,///left bottom front, 2
            vertRightBottomFront,//right bottom front, 3
 
            //back face//
            vertRightTopBack,//right top back, 4
            vertLeftTopBack,//left top back, 5
            vertRightBottomBack,//right bottom back, 6
            vertLeftBottomBack,///left bottom back, 7
 
            //left face//
            vertLeftTopBack,//left top back, 8
            vertLeftTopFront,//left top front, 9
            vertLeftBottomBack,///left bottom back, 10
            vertLeftBottomFront,//left bottom front, 11
 
            //right face//
            vertRightTopFront,//right top front, 12
            vertRightTopBack,//right top back, 13
            vertRightBottomFront,//right bottom front, 14
            vertRightBottomBack,///right bottom back, 15
 
            //top face//
            vertLeftTopBack,//left top back, 16
            vertRightTopBack,//right top back, 17
            vertLeftTopFront,//left top front, 18
            vertRightTopFront,//right top front, 19
 
            //bottom face//
            vertLeftBottomFront,//left bottom front, 20
            vertRightBottomFront,//right bottom front, 21
            vertLeftBottomBack,////left bottom back, 22
            vertRightBottomBack,//right bottom back, 23
 
        };
        Vector3[] vertices2 = new Vector3[]
        {
            vertLeftBottomFront,//left bottom front, 20
            vertRightBottomFront,//right bottom front, 21
            vertLeftBottomBack,////left bottom back, 22
            vertRightBottomBack,//right bottom back, 23
        };
        //Triangles// 3 points, clockwise determines which side is visible
        int[] triangles = new int[]
        {
            //front face//
            0,1,3,//first triangle
            3,2,0,//second triangle
 
            //back face//
            4,5,7,//first triangle
            7,6,4,//second triangle
 
            //left face//
            8,9,11,//first triangle
            11,10,8,//second triangle
 
            //right face//
            12,13,15,//first triangle
            15,14,12,//second triangle
 
            //top face//
            16,17,19,//first triangle
            19,18,16,//second triangle
 
            //bottom face//
            20,21,23,//first triangle
            23,22,20//second triangle
        };
        
        //UVs//      
        Vector2[] uvs = new Vector2[]
        {
            //front face// 0,0 is bottom left, 1,1 is top right//
            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0),

            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0),

            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0),

            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0),

            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0),

            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0)
        };
        
       
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //GameObject plane = Instantiate<GameObject>(GameObject.Find(Cube));
        plane.transform.SetParent(transform);
        plane.transform.localPosition = new Vector3(-2, 0, 1);
        plane.transform.localScale = new Vector3(-0.4f,0.1f,0.2f);
       
    }

    // Update is called once per frame
    void Update()
    {

    }
}
