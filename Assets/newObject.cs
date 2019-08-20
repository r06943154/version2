using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class newObject : NetworkBehaviour {
    private Vector3 vertLeftTopFront = new Vector3(-1, 1, 1);
    private Vector3 vertRightTopFront = new Vector3(1, 1, 1);
    private Vector3 vertRightTopBack = new Vector3(1, 1, -1);
    private Vector3 vertLeftTopBack = new Vector3(-1, 1, -1);
    private Vector3 vertLeftBottomFront = new Vector3(-1, -1, 1);
    private Vector3 vertRightBottomFront = new Vector3(1, -1, 1);
    private Vector3 vertRightBottomBack = new Vector3(1, -1, -1);
    private Vector3 vertLeftBottomBack = new Vector3(-1, -1, -1);
    public Transform camera;
    public Camera mainCam;

    private static ILogger logger = Debug.logger;
    public static List<List<string>> inductance = new List<List<string>>();
    public static List<List<string>> capacitance = new List<List<string>>();
    public static List<List<string>> mapping_list = new List<List<string>>();
    public static List<List<string>> component_node = new List<List<string>>();
    public static List<List<string>> equal_node = new List<List<string>>();
    public static List<string> port = new List<string>();
    public static List<string> gnd = new List<string>();
    public static int Rect_Num;
    GameObject objToSpawn;

    // Use this for initialization
    void Start() {
        string myPath = Application.persistentDataPath;

        //textLayer = File.ReadAllText(myPath, "LA.txt");
        //TextAsset odczytaj = Resources.Load("/LA.txt") as TextAsset;
        //LayerNum = odczytaj.text.Split(new char[]{ '\r', '\n' });
        //android use path
        //StreamReader sr = File.OpenText(myPath+"/ER.txt");
        //textLayer = System.IO.File.ReadAllText (myPath+"/LA.txt");
        //textCap = System.IO.File.ReadAllText (myPath+"/CAP.txt");
        //textInd = System.IO.File.ReadAllText (myPath+"/IND.txt");

        string textLayer = System.IO.File.ReadAllText("layer.txt");
        string[] LayerNum = textLayer.Split(new char[] { '\r', '\n' });
        string textNetnumber = System.IO.File.ReadAllText("nodetag.txt");
        string[] Netnumber = textNetnumber.Split(new char[] { '\t' });

        ///////////讀取capacitance
        string textCap = System.IO.File.ReadAllText("Output_cap.txt");
        string[] cap_split = textCap.Split(new char[] { '\r', '\n' });
        for (int i = 0; i < cap_split.Length - 1; i = i + 2)
        {
            List<string> cap_1 = new List<string>();
            string[] cap1 = cap_split[i].Split(new char[] { ' ' });
            for (int j = 0; j < cap1.Length; j++)
            {
                cap_1.Add(cap1[j]);
            }
            capacitance.Add(cap_1);
        }
        Debug.Log("caparray=" + capacitance.Count);
        ///////////讀取inductance
        string textInd = System.IO.File.ReadAllText("Output_ind.txt");
        string[] ind_split = textInd.Split(new char[] { '\r', '\n' });
        for (int i = 0; i < ind_split.Length - 1; i = i + 2) {
            List<string> ind_1 = new List<string>();
            string[] ind1 = ind_split[i].Split(new char[] { ' ' });
            for (int j = 0; j < ind1.Length; j++) {
                ind_1.Add(ind1[j]);
            }
            inductance.Add(ind_1);
        }
        Debug.Log("indarray=" + inductance.Count);
        ///////////讀取Port list和Gnd list 和component node
        string[] a;
        string b;
        b = System.IO.File.ReadAllText("netlist_output.net");
        a = b.Split(new char[] { '\r', '\n' });
        for (int i = 0; i < a.Length - 1; i = i + 2) {
            List<string> component_1 = new List<string>();
            string[] c = a[i].Split(new char[] { ' ', '\t' });
            if (c[0] == "") continue;
            if (c[0] == "Port") {
                port.Add(c[2]);
                component_1.Add(c[1]);
                component_1.Add(c[2]);
            }
            if (c[0] == "GROUND")
            {
                gnd.Add(c[2]);
                component_1.Add(c[1]);
                component_1.Add(c[2]);
            }
            if (c[0] == "C" || c[0] == "L")
            {
                component_1.Add(c[1]);
                component_1.Add(c[2]);
                component_1.Add(c[3]);
            }
            component_node.Add(component_1);
        }
        Debug.Log("component_node= " + component_node.Count);
        Debug.Log("port=" + port.Count);
        Debug.Log("gnd=" + gnd.Count);
        ///////////讀取mapping list
        string textMapping = System.IO.File.ReadAllText("mapping_list.txt");
        string[] mappping_split = textMapping.Split(new char[] { '\r', '\n' });
        for (int i = 0; i < mappping_split.Length - 1; i = i + 2)
        {
            List<string> mapping_1 = new List<string>();
            string[] mapping1 = mappping_split[i].Split(new char[] { '\t' });
            for (int j = 0; j < mapping1.Length; j++)
            {
                mapping_1.Add(mapping1[j]);
            }
            mapping_list.Add(mapping_1);
        }
        Debug.Log("mapping_list count=" + mapping_list.Count);
        ///////////讀取node list
        string textnode = System.IO.File.ReadAllText("Output_net.txt");
        string[] node_split = textnode.Split(new char[] { '\r', '\n' });
        for (int i = 0; i < node_split.Length - 1; i = i + 2)
        {
            List<string> equal_1 = new List<string>();
            string[] node1 = node_split[i].Split(new char[] { '\t' });
            equal_1.Add(node1[0]);
            equal_1.Add(node1[1]);
            equal_node.Add(equal_1);
        }

        Debug.Log("node=" + equal_node.Count);
        //Debug.Log("equal_node= " + equal_node.Count);
        ///////////讀取essential rectangal
        StreamReader sr = File.OpenText("essential_Rect.txt");
        string text = sr.ReadLine();
        string Idx;
        int Idx_int = 0;
        while (text != null) {
            string[] splitString = text.Split(new char[] { '\t', '\n', ' ' });
            //spawn object
            float LayerT = 0, LayerB = 0;
            //2019.6
            for (int i = 0; i < LayerNum.Length; i++) {
                string[] LayerNumSplit = LayerNum[i].Split(new char[] { '\t', '\n', ' ' });
                for (int q = 0; q < LayerNumSplit.Length; q++)
                    if (LayerNumSplit[q] == splitString[0] && q > 1)
                    {
                        int j = (i - 1) / 4;
                        LayerB = (j * 58f) + (((i - 1) % 4) / 2 * 13f);
                        LayerT = (j * 58f) + (((i - 1) % 4) / 2 * 45f) + 13f;
                        //Debug.Log("splitString[0] = " + splitString[0] + "  LayerT - LayerB = " + (LayerT - LayerB));
                    }
            }
            Idx = splitString[2];
            Idx_int = int.Parse(Idx);
            Rect_Num++;
            objToSpawn = new GameObject(splitString[2]);
            objToSpawn.AddComponent<NetworkTransform>();
            GameObject ngo = new GameObject("myTextGO");
            ngo.transform.SetParent(objToSpawn.transform);
            var s = ngo.AddComponent<followPlayer>();
            s.PlayerPos = camera;

            ngo.transform.Rotate(0, 180, 0);
            Canvas canvas = ngo.AddComponent<Canvas>();
            CanvasScaler cs = ngo.AddComponent<CanvasScaler>();
            GraphicRaycaster gr = ngo.AddComponent<GraphicRaycaster>();

            GameObject g2 = new GameObject("Text");
            g2.transform.SetParent(ngo.transform);
            Text t = g2.AddComponent<Text>();

            t.alignment = TextAnchor.MiddleCenter;
            t.horizontalOverflow = HorizontalWrapMode.Overflow;
            t.verticalOverflow = VerticalWrapMode.Overflow;
            Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            t.font = ArialFont;
            t.fontSize = 30;
            t.text = "";
            t.enabled = false;
            t.color = Color.black;///字體顏色

            objToSpawn.AddComponent<MeshFilter>();
            objToSpawn.AddComponent<BoxCollider>();
            objToSpawn.AddComponent<MeshRenderer>();

            MeshRenderer mr = objToSpawn.GetComponent<MeshRenderer>();
            mr.materials[0] = (Material)Resources.Load("");

            BoxCollider[] bc = objToSpawn.GetComponents<BoxCollider>();
            MeshFilter mf = objToSpawn.GetComponent<MeshFilter>();
            Mesh mesh = mf.mesh;

            text = sr.ReadLine();
            splitString = text.Split(new char[] { '\t', '\n' });
            float x = float.Parse(splitString[0]);//ex1 *50
            float y = float.Parse(splitString[1]);
            float xSum = 0, ySum = 0;
            xSum += x;
            ySum += y;

            objToSpawn.transform.position = new Vector3(x / 10000.0f, LayerT / 1.0f, y / 10000.0f);
            Vector3 v3 = new Vector3(x / 10000.0f, LayerT / 1.0f, y / 10000.0f);
            vertLeftTopFront = new Vector3(x / 10000.0f, LayerT / 1.0f, y / 10000.0f) - v3;
            vertLeftBottomFront = new Vector3(x / 10000.0f, LayerB / 1.0f, y / 10000.0f) - v3;
            text = sr.ReadLine();
            splitString = text.Split(new char[] { '\t', '\n' });
            x = float.Parse(splitString[0]);
            y = float.Parse(splitString[1]);
            xSum += x;
            ySum += y;
            vertRightTopFront = new Vector3(x / 10000.0f, LayerT / 1.0f, y / 10000.0f) - v3;
            vertRightBottomFront = new Vector3(x / 10000.0f, LayerB / 1.0f, y / 10000.0f) - v3;
            text = sr.ReadLine();
            splitString = text.Split(new char[] { '\t', '\n' });
            x = float.Parse(splitString[0]);
            y = float.Parse(splitString[1]);
            xSum += x;
            ySum += y;
            vertRightTopBack = new Vector3(x / 10000.0f, LayerT / 1.0f, y / 10000.0f) - v3;
            vertRightBottomBack = new Vector3(x / 10000.0f, LayerB / 1.0f, y / 10000.0f) - v3;
            text = sr.ReadLine();
            splitString = text.Split(new char[] { '\t', '\n' });
            x = float.Parse(splitString[0]);
            y = float.Parse(splitString[1]);
            xSum += x;
            ySum += y;
            vertLeftTopBack = new Vector3(x / 10000.0f, LayerT / 1.0f, y / 10000.0f) - v3;
            vertLeftBottomBack = new Vector3(x / 10000.0f, LayerB / 1.0f, y / 10000.0f) - v3;

            bc[0].center = new Vector3(-(xSum / 4.0f - x) / 10000.0f, -(LayerT - LayerB) / 2.0f, (ySum / 4.0f - y) / 10000.0f);
            bc[0].size = new Vector3(Mathf.Abs(xSum / 4.0f - x) * 2.0f / 10000.0f, (LayerT - LayerB) / 1.0f, Mathf.Abs(ySum / 4.0f - y) * 2.0f / 10000.0f);

            //Vertices//
            Vector3[] vertices = new Vector3[] {
				//front face//
				vertLeftTopFront,//left top front, 0
				vertRightTopFront,//right top front, 1
				vertLeftBottomFront,//new Vector3(-1,-1,1),//left bottom front, 2
				vertRightBottomFront,//new Vector3(1,-1,1),//right bottom front, 3

				//back face//
				vertRightTopBack,//right top back, 4
				vertLeftTopBack,//left top back, 5
				vertRightBottomBack,//new Vector3(1,-1,-1),//right bottom back, 6
				vertLeftBottomBack,//new Vector3(-1,-1,-1),//left bottom back, 7

				//left face//
				vertLeftTopBack,//left top back, 8
				vertLeftTopFront,//left top front, 9
				vertLeftBottomBack,//new Vector3(-1,-1,-1),//left bottom back, 10
				vertLeftBottomFront,//new Vector3(-1,-1,1),//left bottom front, 11
                
				//right face//
				vertRightTopFront,//right top front, 12
				vertRightTopBack,//right top back, 13
				vertRightBottomFront,//new Vector3(1,-1,1),//right bottom front, 14
				vertRightBottomBack,//new Vector3(1,-1,-1),//right bottom back, 15

				//top face//
				vertLeftTopBack,//left top back, 16
				vertRightTopBack,//right top back, 17
				vertLeftTopFront,//left top front, 18
				vertRightTopFront,//right top front, 19

				//bottom face//
				vertLeftBottomFront,//new Vector3(-1,-1,1),//left bottom front, 20
				vertRightBottomFront,//new Vector3(1,-1,1),//right bottom front, 21
				vertLeftBottomBack,//new Vector3(-1,-1,-1),//left bottom back, 22
				vertRightBottomBack,//new Vector3(1,-1,-1)//right bottom back, 23

			};

            //Triangles// 3 points, clockwise determines which side is visible
            int[] triangles = new int[] {
				//front face//
				0, 1, 3,//first triangle
				3, 2, 0,//second triangle

				//back face//
				4, 5, 7,//first triangle
				7, 6, 4,//second triangle

				//left face//
				8, 9, 11,//first triangle
				11, 10, 8,//second triangle
                
				//right face//
				12, 13, 15,//first triangle
				15, 14, 12,//second triangle

				//top face//
				16, 17, 19,//first triangle
				19, 18, 16,//second triangle

				//bottom face//
				20, 21, 23,//first triangle
				23, 22, 20//second triangle
			};
            //UVs//
            Vector2[] uvs = new Vector2[] {
				//front face// 0,0 is bottom left, 1,1 is top right//
				new Vector2 (0, 1),
                new Vector2 (0, 0),
                new Vector2 (1, 1),
                new Vector2 (1, 0),

                new Vector2 (0, 1),
                new Vector2 (0, 0),
                new Vector2 (1, 1),
                new Vector2 (1, 0),

                new Vector2 (0, 1),
                new Vector2 (0, 0),
                new Vector2 (1, 1),
                new Vector2 (1, 0),

                new Vector2 (0, 1),
                new Vector2 (0, 0),
                new Vector2 (1, 1),
                new Vector2 (1, 0),

                new Vector2 (0, 1),
                new Vector2 (0, 0),
                new Vector2 (1, 1),
                new Vector2 (1, 0),

                new Vector2 (0, 1),
                new Vector2 (0, 0),
                new Vector2 (1, 1),
                new Vector2 (1, 0)
            };

            int count_ = 0;
            for (int i = 0; i < capacitance.Count; i++) {
                for (int j = 0; j < capacitance[i].Count; j++) {
                    if (capacitance[i][j] == Idx) {
                        count_++;
                        float f = capacitance.Count / 2, ff = i / 2;
                        float intt = 1 * ff / f;
                        if (count_ > 1 && Idx_int != 0) {
                            GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                            plane.transform.SetParent(objToSpawn.transform);
                            plane.transform.localPosition = new Vector3(-(vertLeftBottomFront - vertLeftBottomBack).x / 2f, 0.01f, (vertRightBottomBack - vertLeftBottomBack).z / 2f);
                            plane.transform.localScale = new Vector3((vertLeftBottomFront - vertLeftBottomBack).x / 10f, 0.1f, (vertRightBottomBack - vertLeftBottomBack).z / 10f);
                            MeshRenderer plane_surface = plane.GetComponent<MeshRenderer>();
                            plane_surface.material.color = mr.material.color;
                        }
                        mr.material.color = new Color(1, intt, 0, 1);//Color.red;
                        t.text += "C" + (i + 1) + ":  " + Idx + " " + Netnumber[Idx_int] + '\n';
                    }
                }
            }

            for (int i = 0; i < inductance.Count; i++) {
                for (int j = 0; j < inductance[i].Count; j++) {
                    if (inductance[i][j] == Idx) {
                        if (j == 0) {
                            t.enabled = true;
                        }
                        float f = inductance.Count / 2, ff = i / 2;
                        float intt = 1 * ff / f;
                        mr.material.color = new Color(0, intt, 1, 1);//Color.blue;
                        t.text += "L" + (i + 1) + ":  " + Idx + " " + Netnumber[Idx_int] + '\n';
                    }
                }
            }
            for (int i = 0; i < port.Count; i++) {
                if (port[i] == Idx) {
                    mr.material.color = new Color(0, 1, 0, 1);//Color.green;
                    t.text += "P" + (i + 1) + ":  " + Idx + " " + Netnumber[Idx_int] + '\n';
                    t.enabled = true;
                }
            }
            for (int i = 0; i < gnd.Count; i++) {
                if (gnd[i] == Idx) {
                    mr.material.color = new Color(0.5f, 0.5f, 0.5f, 1);//Color.blue;
                    t.text += "Gnd: " + Idx + " " + Netnumber[Idx_int] + '\n';
                }
            }

            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();

            text = sr.ReadLine();//skip last point
            text = sr.ReadLine();//next data
            //Cmdspand(objToSpawn.name);
              
        }
        Debug.Log("Rect_Num=" + Rect_Num);
    }

    [Command]
    void Cmdspand(string objToSpawn)
    {
        GameObject rec = GameObject.Find(objToSpawn);
        NetworkServer.Spawn(rec);
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetMouseButtonDown(0)){
            Debug.Log("Mouse is down");
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hitInfo);
            Debug.Log(mainCam.ScreenPointToRay(Input.mousePosition).direction);

            if (hit){
                if (hitInfo.transform.gameObject.name == "Cube")
                {
                    selectOBJ.selected = "";
                }
                else
                {
                    hitInfo.transform.GetComponentInChildren<Text>().enabled = !hitInfo.transform.GetComponentInChildren<Text>().enabled;
                }
            }
        }
        List<string> ind_instantiate = new List<string>();
        List<string> cap_instantiate = new List<string>();
        //RaycastHit mouse_overlap = new RaycastHit();
        //bool mouse_overlap_cube = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mouse_overlap);
        /*if (mouse_overlap_cube){
            for (int i = 0; i < inductance.Count; i++){
                for (int j = 0; j < inductance[i].Count; j++){
                    if (inductance[i][j] == mouse_overlap.transform.name){
                        for (int k = 0; k < inductance[i].Count; k++)
                            ind_instantiate.Add(inductance[i][k]);
                    }
                }
            }
           
            for (int i = 0; i < ind_instantiate.Count; i++){
                GameObject origin_cube = Instantiate<GameObject>(GameObject.Find(ind_instantiate[i]));
                MeshRenderer mesh = origin_cube.GetComponent<MeshRenderer>();
                mesh.material = new Material(Shader.Find("Custom/BoundryShader"));
                Debug.Log(mouse_overlap.transform.name);
                GameObject.Destroy(origin_cube, 0.4f);
            }
            for (int i = 0; i < capacitance.Count; i++)
            {
                for (int j = 0; j < capacitance[i].Count; j++)
                {
                    if (capacitance[i][j] == mouse_overlap.transform.name)
                    {
                        for (int k = 0; k < capacitance[i].Count; k++)
                            cap_instantiate.Add(capacitance[i][k]);
                    }
                }
            }
            for (int i = 0; i < cap_instantiate.Count; i++)
            {
                GameObject origin_cube = Instantiate<GameObject>(GameObject.Find(cap_instantiate[i]));
                MeshRenderer mesh = origin_cube.GetComponent<MeshRenderer>();
                mesh.material = new Material(Shader.Find("Custom/BoundryShader"));
                Debug.Log(mouse_overlap.transform.name);
                GameObject.Destroy(origin_cube, 0.4f);
            }
        }*/
    }
    
}
