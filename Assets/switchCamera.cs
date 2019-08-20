using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class switchCamera : NetworkBehaviour
{
    public TextAsset imageRed;
    public TextAsset imageGreen;
    public TextAsset imageBlue;
    public TextAsset imageGreenBlue;
    public TextAsset imageRedBlue;
    public TextAsset imageYellow;
    public TextAsset imageNoFind;
    public TextAsset imageGnd;
    public Material materialBlue;
    private Transform playCameraTransform;
    private Camera mainCam;

    List<Rect> playerPositionRect = new List<Rect>();
    List<string> nodeName = new List<string>();
    List<Vector2> currentDrag = new List<Vector2>();
    List<bool> selectList = new List<bool>();
    public static List<List<string>> mismatch_component_node = new List<List<string>>();

    int countPG = 0;
    int countC = 0;
    int countL = 0;
    int countN = 0;
    public static int selected = 0;

    void Start() {
        playCameraTransform = transform.Find("Main Camera");
        mainCam = playCameraTransform.GetComponent<Camera>();

        Rect _rect = new Rect(10, 10, 30, 30);
        for (int i = 0; i < newObject.component_node.Count; i++)
        {
            int match = 0;
            //Debug.Log("newObject.component_node[i][0] " + newObject.component_node[i][0]);
            for (int j = 0; j < newObject.mapping_list.Count; j++)
            {
                if (newObject.component_node[i][0] == newObject.mapping_list[j][1])
                {
                    GameObject findlist = GameObject.Find(newObject.mapping_list[j][0]);
                    _rect = new Rect(((findlist.transform.position.x + 7f * 15f / 19f + 4) * 32f + 6f) * 0.5f + 940, ((0 - (findlist.transform.position.y) - 1500 + 7f * 15f / 19f + 4) * 32f + 6) * 0.5f, 40, 40);
                    nodeName.Add(newObject.mapping_list[j][1]);
                    playerPositionRect.Add(_rect);
                    currentDrag.Add(new Vector2());
                    selectList.Add(false);
                    countPG++;
                    match = 1;
                }
            }
            if (match == 0)
            {
                List<string> component_1 = new List<string>();
                component_1.Add(newObject.component_node[i][0]);
                component_1.Add(newObject.component_node[i][1]);
                component_1.Add(newObject.component_node[i][2]);
                mismatch_component_node.Add(component_1);
            }

            int index = nodeName.FindIndex(s => s.Equals(newObject.component_node[i][1]));

            if (index == -1)
            {
                if (newObject.component_node[i][1] != "0")
                {
                    playerPositionRect.Add(new Rect(_rect.x, _rect.y, 30, 30));
                    nodeName.Add(newObject.component_node[i][1]);
                    currentDrag.Add(new Vector2());
                    selectList.Add(false);
                    countN++;
                }
            }
            else
            {
                if (match == 1)
                    playerPositionRect[index] = new Rect((playerPositionRect[index].x + _rect.x) / 2, (playerPositionRect[index].y + _rect.y) / 2, 30, 30);
            }
           
            if (newObject.component_node[i].Count == 3)
            {
                index = nodeName.FindIndex(s => s.Equals(newObject.component_node[i][2]));
                if (index == -1)
                {
                    if (newObject.component_node[i][2] != "0")
                    {
                        playerPositionRect.Add(new Rect(_rect.x, _rect.y, 30, 30));
                        nodeName.Add(newObject.component_node[i][2]);
                        currentDrag.Add(new Vector2());
                        selectList.Add(false);
                        countN++;
                    }
                }
                else
                {
                    if (match == 1)
                        playerPositionRect[index] = new Rect((playerPositionRect[index].x + _rect.x) / 2, (playerPositionRect[index].y + _rect.y) / 2, 30, 30);
                    if (newObject.component_node[i][2] == "44")
                    {
                        Debug.Log("node 44 second : " + playerPositionRect[index].width + "  " + playerPositionRect[index].height);
                    }
                }
                /*if (nodeName[i] == "0")
                {
                    GameObject findlist = GameObject.Find("G1");
                    playerPositionRect[index]= new Rect(((findlist.transform.position.x + 7f * 15f / 19f + 4) * 32f + 6f) * 0.5f + 940, ((0 - (findlist.transform.position.y) - 1500 + 7f * 15f / 19f + 4) * 32f + 6) * 0.5f, 40, 40);
                }*/
            }           
        }
        for (int i = 0; i < mismatch_component_node.Count; i++)
        {
            nodeName.Add(mismatch_component_node[i][0]);
            int index1 = nodeName.FindIndex(s => s.Equals(mismatch_component_node[i][1]));
            int index2 = nodeName.FindIndex(s => s.Equals(mismatch_component_node[i][2]));

            if (index1 != -1 && index2 != -1)
                _rect = new Rect((playerPositionRect[index1].x + playerPositionRect[index2].x) / 2, (playerPositionRect[index1].y + playerPositionRect[index2].y) / 2, 40, 40);
            else if (index1 != -1)
                _rect = new Rect(playerPositionRect[index1].x, playerPositionRect[index1].y + 50.0f, 40, 40);
            else if (index2 != -1)
                _rect = new Rect(playerPositionRect[index2].x, playerPositionRect[index2].y + 50.0f, 40, 40);
            playerPositionRect.Add(_rect);
            currentDrag.Add(new Vector2());
            selectList.Add(false);
            countPG++;
        }       
    }

    void Update()
    {
        for (int i = 0; i < newObject.component_node.Count; i++)
        {
            if (newObject.component_node[i].Count == 3)
            {
                int index1 = nodeName.FindIndex(s => s.Equals(newObject.component_node[i][0]));
                int index2 = nodeName.FindIndex(s => s.Equals(newObject.component_node[i][1]));
                int index3 = nodeName.FindIndex(s => s.Equals(newObject.component_node[i][2]));
                if (index2 != -1 && index3 != -1)
                {
                    if (playerPositionRect[index2].x < playerPositionRect[index3].x)
                    {
                        DrawLine(mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index1].x + 1, 720 - playerPositionRect[index1].y - 15, 1.0f)), mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index2].x + 15, 720 - playerPositionRect[index2].y - 15, 1.0f)), Color.blue);
                        DrawLine(mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index1].x + 29, 720 - playerPositionRect[index1].y - 15, 1.0f)), mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index3].x + 15, 720 - playerPositionRect[index3].y - 15, 1.0f)), Color.blue);
                    }
                    else
                    {
                        DrawLine(mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index1].x + 29, 720 - playerPositionRect[index1].y - 15, 1.0f)), mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index2].x + 15, 720 - playerPositionRect[index2].y - 15, 1.0f)), Color.blue);
                        DrawLine(mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index1].x + 1, 720 - playerPositionRect[index1].y - 15, 1.0f)), mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index3].x + 15, 720 - playerPositionRect[index3].y - 15, 1.0f)), Color.blue);
                    }
                }
                else if (index3 == -1)
                {
                    DrawLine(mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index1].x + 15, 720 - playerPositionRect[index1].y - 15, 1.0f)), mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index2].x + 15, 720 - playerPositionRect[index2].y - 15, 1.0f)), Color.blue);
                }
                else if (index2 == -1)
                {
                    DrawLine(mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index1].x + 1, 720 - playerPositionRect[index1].y - 15, 1.0f)), mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index3].x + 15, 720 - playerPositionRect[index3].y - 15, 1.0f)), Color.blue);
                }
            }
            if (newObject.component_node[i].Count == 2)
            {
                int index1 = nodeName.FindIndex(s => s.Equals(newObject.component_node[i][0]));
                int index2 = nodeName.FindIndex(s => s.Equals(newObject.component_node[i][1]));
                if (index2 != -1)
                    DrawLine(mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index1].x + 15, 720 - playerPositionRect[index1].y - 15, 1.0f)), mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index2].x + 15, 720 - playerPositionRect[index2].y - 15, 1.0f)), Color.blue);
            }
        }
        if (Input.GetKey(KeyCode.M))
        {
            for (int i = 0; i < playerPositionRect.Count; i++)
            {
                for (int j = i + 1; j < playerPositionRect.Count; j++)
                {
                    if (playerPositionRect[i].x < playerPositionRect[j].x)
                    {
                        Rect rect = playerPositionRect[i];
                        playerPositionRect[i] = playerPositionRect[j];
                        playerPositionRect[j] = rect;
                        string tmp = nodeName[i];
                        nodeName[i] = nodeName[j];
                        nodeName[j] = tmp;
                        Vector2 vec = currentDrag[i];
                        currentDrag[i] = currentDrag[j];
                        currentDrag[j] = vec;
                        bool bol = selectList[i];
                        selectList[i] = selectList[j];
                        selectList[j] = bol;
                    }
                }
            }
            for (int i = 0; i < playerPositionRect.Count; i++)
            {
                for (int j = i + 1; j < playerPositionRect.Count; j++)
                {
                    if (playerPositionRect[i].x != playerPositionRect[j].x && playerPositionRect[i].x - playerPositionRect[j].x < 35.0f && playerPositionRect[j].y - playerPositionRect[i].y < 35.0f)  
                    {
                        float distance = playerPositionRect[i].x - playerPositionRect[j].x;                    
                        for (int k = j; k < playerPositionRect.Count; k++)
                        {
                            playerPositionRect[k] = new Rect(playerPositionRect[k].x - (36 - distance), playerPositionRect[k].y, playerPositionRect[k].width, playerPositionRect[k].height);
                        }
                        break;
                    }                   
                }
            }
            for (int i = 0; i < playerPositionRect.Count-1; i++)
            {
                if ((playerPositionRect[i].x - playerPositionRect[i+1].x) > 37.0f)
                {
                    float distance = playerPositionRect[i].x - playerPositionRect[i+1].x;
                    for (int k = i + 1; k < playerPositionRect.Count; k++)
                    {
                        playerPositionRect[k] = new Rect(playerPositionRect[k].x - (36 - distance), playerPositionRect[k].y, playerPositionRect[k].width, playerPositionRect[k].height);
                    }              
                }
            }
        }
    }
    void OnGUI()
    {
        Texture2D textureRed = new Texture2D(50, 50);
        Texture2D textureGreen = new Texture2D(50, 50);
        Texture2D textureBlue = new Texture2D(50, 50);
        Texture2D textureYellow = new Texture2D(50, 50); ///電感 yellow
        Texture2D textureRedBlue = new Texture2D(50, 50);
        Texture2D textureGreenBlue = new Texture2D(50, 50); ///電容 greenblue
        Texture2D textureNoFind = new Texture2D(50, 50); ///遺失
        Texture2D textureGnd = new Texture2D(50, 50); 

        //Texture2D 
        textureRed.LoadImage(imageRed.bytes);
        textureGreen.LoadImage(imageGreen.bytes);
        textureBlue.LoadImage(imageBlue.bytes);
        textureYellow.LoadImage(imageYellow.bytes);  
        textureRedBlue.LoadImage(imageRedBlue.bytes);
        textureGreenBlue.LoadImage(imageGreenBlue.bytes);
        textureNoFind.LoadImage(imageNoFind.bytes);
        textureGnd.LoadImage(imageGnd.bytes);
        for (int i = 0; i < countPG + countL + countC + countN; i++)
        {
            if (nodeName[i][0] == 'P')
                GUI.Button(playerPositionRect[i], new GUIContent(nodeName[i], textureRed));
            else if (nodeName[i][0] == 'G')
            {
                GUI.DrawTexture(playerPositionRect[i], textureGnd);
                GUI.Button(playerPositionRect[i], new GUIContent(nodeName[i], textureRed));
            }
            else if (nodeName[i][0] == 'L')
            {
                for (int j = 0; j < newObject.component_node.Count; j++)
                {
                    if (nodeName[i] == newObject.component_node[j][0] && (newObject.component_node[j][1] == "0" || newObject.component_node[j][2] == "0"))
                        GUI.DrawTexture(playerPositionRect[i], textureGnd);
                }
                for (int j = 0; j < mismatch_component_node.Count; j++)
                {
                    if (nodeName[i] == mismatch_component_node[j][0])
                        GUI.DrawTexture(playerPositionRect[i], textureNoFind);
                }
                GUI.Button(playerPositionRect[i], new GUIContent(nodeName[i], textureYellow));
            }
            else if (nodeName[i][0] == 'C')
            {
                for (int j = 0; j < newObject.component_node.Count; j++)
                {
                    if (nodeName[i] == newObject.component_node[j][0] && (newObject.component_node[j][1] == "0" || newObject.component_node[j][2] == "0"))
                        GUI.DrawTexture(playerPositionRect[i], textureGnd);
                }
                for (int j = 0; j < mismatch_component_node.Count; j++)
                {
                    if (nodeName[i] == mismatch_component_node[j][0])
                        GUI.DrawTexture(playerPositionRect[i], textureNoFind);
                }
                GUI.Button(playerPositionRect[i], new GUIContent(nodeName[i], textureGreenBlue));
            }
            else
                GUI.Button(playerPositionRect[i], new GUIContent(nodeName[i], textureGreen));
            if (selectList[i] == true)
                GUI.Button(playerPositionRect[i], new GUIContent(nodeName[i], textureRedBlue));
        }

        Vector2 screenMousePosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        for (int i = 0; i < countPG + countL + countC + countN; i++)
        {
            if (selectOBJ.selected == nodeName[i])
                selected = i;
            if (selectOBJ.selected == "")
                selected = 0;
            if (currentDrag[i].sqrMagnitude != 0 || playerPositionRect[i].Contains(screenMousePosition))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    currentDrag[i] = screenMousePosition;
                    selected = i;
                    selectOBJ.selected = nodeName[i];
                }
                else if (Input.GetMouseButton(0))
                {
                    Rect tempRect = new Rect();
                    tempRect = playerPositionRect[selected];
                    tempRect.x += (screenMousePosition.x - currentDrag[selected].x);
                    tempRect.y += (screenMousePosition.y - currentDrag[selected].y);
                    playerPositionRect[selected] = tempRect;
                    currentDrag[selected] = screenMousePosition;
                }
                else
                {
                    Vector2 tempVector2 = currentDrag[i];
                    tempVector2.x = 0;
                    tempVector2.y = 0;
                    currentDrag[i] = tempVector2;
                }
            }
        }
        for (int i = 0; i < countPG + countL + countC + countN; i++)
        {
            if (i == selected)
                selectList[i] = true;
            else
                selectList[i] = false;
        }
    }
    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.02f)
    {
        GameObject myLine = new GameObject();
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = materialBlue;
        lr.startColor = color;
        lr.endColor = color;
        lr.useWorldSpace = false;
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }
}
