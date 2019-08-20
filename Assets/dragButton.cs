using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class dragButton : NetworkBehaviour
{
	public TextAsset imageRed;
	public TextAsset imageGreen;
	public TextAsset imageBlue;
	public TextAsset imageGreenBlue;
	public TextAsset imageRedBlue;
	public TextAsset imageYellow;
    public Material materialBlue;
    private Transform playCameraTransform;
    private Camera mainCam;

    List<Rect> playerPositionRect = new List<Rect> ();
    public static List<string> nodeName = new List<string> ();
	List<Vector2> currentDrag = new List<Vector2> ();
	List<bool> selectList = new List<bool> ();
   
	int countPG = 0;
	int countC = 0;
	int countL = 0;
	int countN = 0;
    public static int selected = 0;
    public static string[] PortIdX,GndIdX;

	public static char[] charsToTrim = { 'P', 'G', 'C', 'L' };

	

	void DrawLine (Vector3 start, Vector3 end, Color color, float duration = 0.02f)
	{
		GameObject myLine = new GameObject ();
        myLine.AddComponent<LineRenderer>();
		LineRenderer lr = myLine.GetComponent<LineRenderer> ();
		lr.material = materialBlue;
        lr.startColor = color;
        lr.endColor = color;
        lr.useWorldSpace = true;
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
        lr.SetPosition (0, start);
		lr.SetPosition (1, end);
		GameObject.Destroy (myLine, duration);
	}

	// Use this for initialization
	void Start ()
	{
        playCameraTransform = transform.Find("Main Camera");
        mainCam = playCameraTransform.GetComponent<Camera>();
        string textPort = System.IO.File.ReadAllText ("PORT.txt");
		string textGnd = System.IO.File.ReadAllText ("GND.txt");
		
		PortIdX = textPort.Split (new char[]{ '\t' });
		GndIdX = textGnd.Split (new char[]{ '\t' });

		string gnd = "";
		string[] a;
		string b;
		b = System.IO.File.ReadAllText ("netlist_output.net");
		a = b.Split (new char[]{ '\r', '\n' });

		for (int i = 0; i < a.Length; i++) {
            string[] c = a [i].Split (new char[]{ ' ', '\t' });
			if (c [0] == "GROUND" || c [0] == "Port") {
                string allText;
                allText = System.IO.File.ReadAllText("mapping_list.txt");
                string[] everyLine = allText.Split(new char[] { '\r', '\n' });
                Rect _rect = new Rect(10, 10, 30, 30);
                for (int k = 0; k < everyLine.Length - 2; k += 2)
                {
                    string[] everyWord = everyLine[k].Split(new char[] { '\t', ' ' });
                    GameObject myText = GameObject.Find(everyWord[0]);
                    for (int j = 1; j < everyWord.Length; j++)
                    {
                        if (everyWord[j] == c[1])
                        {
                            GameObject myText2 = GameObject.Find(everyWord[0]);
                            _rect = new Rect((int)((myText2.transform.position.x + 7f * 15f / 19f + 4) * 32f + 6f)*0.5f+940, (int)((0 - (myText2.transform.position.y) - 1500 + 7f * 15f / 19f + 4) * 32f + 6)*0.5f, 30, 30);
                        }
                    }
                }
                playerPositionRect.Add(_rect);
                nodeName.Add (c [1]);
                currentDrag.Add (new Vector2 ());
				selectList.Add (false);
				if (c [0] == "GROUND")
					gnd = c [2];
				countPG++;

				int index = nodeName.FindIndex (s=>s.Equals(c [2]));            
                if (index == -1) {
                    playerPositionRect.Add(_rect);
                    nodeName.Add (c [2]);
					currentDrag.Add (new Vector2 ());
					selectList.Add (false);
					countN++;
				}
                else{
                    if (playerPositionRect[index].width == playerPositionRect[index].height)
                        playerPositionRect[index] = new Rect((playerPositionRect[index].x * ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30)) + _rect.x) / ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30) + 1)
                            , (playerPositionRect[index].y * ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30)) + _rect.y) / ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30) + 1)
                            , playerPositionRect[index].width + 1
                            , playerPositionRect[index].height);
                    else
                        playerPositionRect[index] = new Rect((playerPositionRect[index].x * ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30)) + _rect.x) / ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30) + 1)
                            , (playerPositionRect[index].y * ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30)) + _rect.y) / ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30) + 1)
                            , playerPositionRect[index].width
                            , playerPositionRect[index].height + 1);
                }
            }
			if (c [0] == "C") {
                string allText;
                allText = System.IO.File.ReadAllText("mapping_list.txt");
                string[] everyLine = allText.Split(new char[] { '\r', '\n' });
                Rect _rect = new Rect(10, 10, 60, 60);                
                for (int k = 0; k < everyLine.Length - 1; k += 2){
                    string[] everyWord = everyLine[k].Split(new char[] { '\t', ' ' });
                    GameObject myText = GameObject.Find(everyWord[0]);
                    for (int j = 1; j < everyWord.Length; j++){
                        if (everyWord[j] == c[1]){
                            GameObject myText2 = GameObject.Find(c[1]);
                            _rect = new Rect((int)((myText2.transform.position.x + 7f * 15f / 19f + 4) * 32f + 6f)*0.5f+950, (int)((0 - (myText2.transform.position.y) - 1500 + 7f * 15f / 19f + 4) * 32f + 6)*0.5f, 30, 30);
                        }
                    }
                }
                playerPositionRect.Add(_rect);
                nodeName.Add (c [1]);
				currentDrag.Add (new Vector2 ());
				selectList.Add (false);
				countC++;
				int index = nodeName.FindIndex (s=>s.Equals(c [2]));
                if (index == -1) {
					playerPositionRect.Add (new Rect (countN * 70 + 10, 320, 30, 30));
                    nodeName.Add (c [2]);
					currentDrag.Add (new Vector2 ());
					selectList.Add (false);
					countN++;
				}
                else{
                    if (playerPositionRect[index].width == playerPositionRect[index].height)
                        playerPositionRect[index] = new Rect((playerPositionRect[index].x * ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30)) + _rect.x) / ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30) + 1)
                            , (playerPositionRect[index].y * ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30)) + _rect.y) / ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30) + 1)
                            , playerPositionRect[index].width + 1
                            , playerPositionRect[index].height);
                    else
                        playerPositionRect[index] = new Rect((playerPositionRect[index].x * ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30)) + _rect.x) / ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30) + 1)
                            , (playerPositionRect[index].y * ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30)) + _rect.y) / ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30) + 1)
                            , playerPositionRect[index].width
                            , playerPositionRect[index].height + 1);
                }
                index = nodeName.FindIndex (s=>s.Equals(c [3]));
                if (index == -1) {
				    playerPositionRect.Add (new Rect (countN * 70 + 10, 320, 30, 30));
                    nodeName.Add (c [3]);
					currentDrag.Add (new Vector2 ());
					selectList.Add (false);
					countN++;

				}
                else{
                    if (playerPositionRect[index].width == playerPositionRect[index].height)
                        playerPositionRect[index] = new Rect((playerPositionRect[index].x * ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30)) + _rect.x) / ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30) + 1)
                            , (playerPositionRect[index].y * ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30)) + _rect.y) / ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30) + 1)
                            , playerPositionRect[index].width + 1
                            , playerPositionRect[index].height);
                    else
                        playerPositionRect[index] = new Rect((playerPositionRect[index].x * ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30)) + _rect.x) / ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30) + 1)
                            , (playerPositionRect[index].y * ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30)) + _rect.y) / ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30) + 1)
                            , playerPositionRect[index].width
                            , playerPositionRect[index].height + 1);
                }
            }
			if (c [0] == "L") {
                string allText;
                allText = System.IO.File.ReadAllText("mapping_list.txt");
                string[] everyLine = allText.Split(new char[] { '\r', '\n' });
                Rect _rect = new Rect(10, 10, 30, 30);
                for (int k = 0; k < everyLine.Length - 2; k += 2)
                {
                    string[] everyWord = everyLine[k].Split(new char[] { '\t', ' ' });
                    GameObject myText = GameObject.Find(everyWord[0]);

                    for (int j = 1; j < everyWord.Length; j++)
                    {
                        if (everyWord[j] == c[1])
                        {
                            GameObject myText2 = GameObject.Find(everyWord[0]);
                            _rect = new Rect((int)((myText2.transform.position.x + 7f * 15f / 19f + 4) * 32f + 6f)*0.5f+950, (int)((0 - (myText2.transform.position.y) - 1500 + 7f * 15f / 19f + 4) * 32f + 6)*0.5f, 30, 30);
                        }
                    }
                }
                playerPositionRect.Add(_rect);
                nodeName.Add (c [1]);
				currentDrag.Add (new Vector2 ());
				selectList.Add (false);
				countL++;
				int index = nodeName.FindIndex (s=>s.Equals(c [2]));
				if (index == -1) {
                    playerPositionRect.Add (new Rect (countN * 70 + 10, 320, 30, 30));
                    nodeName.Add (c [2]);
			    	currentDrag.Add (new Vector2 ());
					selectList.Add (false);
			    	countN++;
				}
                else{
                    if (playerPositionRect[index].width == playerPositionRect[index].height)
                        playerPositionRect[index] = new Rect((playerPositionRect[index].x * ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30)) + _rect.x) / ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30) + 1)
                            , (playerPositionRect[index].y * ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30)) + _rect.y) / ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30) + 1)
                            , playerPositionRect[index].width + 1
                            , playerPositionRect[index].height);
                    else
                        playerPositionRect[index] = new Rect((playerPositionRect[index].x * ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30)) + _rect.x) / ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30) + 1)
                            , (playerPositionRect[index].y * ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30)) + _rect.y) / ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30) + 1)
                            , playerPositionRect[index].width
                            , playerPositionRect[index].height + 1);
                }
                index = nodeName.FindIndex (s=>s.Equals(c [3]));
				if (index == -1) {
                    playerPositionRect.Add (new Rect (countN * 70 + 10, 320, 30, 30));
                    nodeName.Add (c [3]);
				    currentDrag.Add (new Vector2 ());
					selectList.Add (false);
				    countN++;
				}
                else{
                    if (playerPositionRect[index].width == playerPositionRect[index].height)
                        playerPositionRect[index] = new Rect((playerPositionRect[index].x * ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30)) + _rect.x) / ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30) + 1)
                            , (playerPositionRect[index].y * ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30)) + _rect.y) / ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30) + 1)
                            , playerPositionRect[index].width + 1
                            , playerPositionRect[index].height);
                    else
                        playerPositionRect[index] = new Rect((playerPositionRect[index].x * ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30)) + _rect.x) / ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30) + 1)
                            , (playerPositionRect[index].y * ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30)) + _rect.y) / ((playerPositionRect[index].width - 30) + (playerPositionRect[index].width - 30) + 1)
                            , playerPositionRect[index].width
                            , playerPositionRect[index].height + 1);
                }
            }
		}
	}

    // Update is called once per frame
   
    void Update() {
        string gnd = "";
        string[] a;
        string b;
        b = System.IO.File.ReadAllText("netlist_output.net");
        a = b.Split(new char[] { '\r', '\n' });
        int count = 0;
        //DrawLine(mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[0].x + 15, 720 - playerPositionRect[0].y - 15, 1.0f)), mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[1].x + 15, 720 - playerPositionRect[1].y - 15, 1.0f)), Color.blue, linecount);
        for (int i = 0; i < a.Length; i++)
        {                  //calculate # of component
            string[] c = a[i].Split(new char[] { ' ', '\t' });
            if (c[0] == "GROUND" || c[0] == "Port")
            {
                int index1 = nodeName.FindIndex(s => s.Equals(c[1]));
                int index = nodeName.FindIndex(s => s.Equals(c[2]));
                if (index >= 0 && index1 >= 0)
                {             //if -1 = not find 
                    DrawLine(mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index1].x + 15, 720 - playerPositionRect[index1].y - 15, 1.0f)),
                    mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index].x + 15, 720 - playerPositionRect[index].y - 15, 1.0f)), Color.blue, i);
                    //DrawLine(mainCam.ScreenToWorldPoint(new Vector3(100.0f, 500.0f, 1.0f)), mainCam.ScreenToWorldPoint(new Vector3(200.0f, 500.0f, 1.0f)), Color.blue, linecount);
                }
                if (c[0] == "GROUND")
                    gnd = c[2];
                count++;
            }
            if (c[0] == "C" || c[0] == "L")
            {
                int index1 = nodeName.FindIndex(s => s.Equals(c[1]));
                int index2 = nodeName.FindIndex(s => s.Equals(c[2]));
                int index3 = nodeName.FindIndex(s => s.Equals(c[3]));
                if (playerPositionRect[index2].x < playerPositionRect[index3].x)
                {
                    if (index2 >= 0 && index1 >= 0)
                    {
                        DrawLine(mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index1].x + 15, 720 - playerPositionRect[index1].y - 15, 1.0f)),
                        mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index2].x + 15, 720 - playerPositionRect[index2].y - 15, 1.0f)), Color.blue, i);
                    }
                    if (index3 >= 0 && index1 >= 0)
                    {
                        //DrawLine(Camera.main.ScreenToWorldPoint(new Vector3(playerPositionRect[index1].x + 15, 720 - playerPositionRect[index1].y - 15, 1.0f)),
                        //Camera.main.ScreenToWorldPoint(new Vector3(playerPositionRect[index3].x + 15, 720 - playerPositionRect[index3].y - 15, 1.0f)), Color.blue, linecount);
                        DrawLine(mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index1].x + 15, 720 - playerPositionRect[index1].y - 15, 1.0f)),
                        mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index3].x + 15, 720 - playerPositionRect[index3].y - 15, 1.0f)), Color.blue, i);
                    }
                }
                else
                {
                    if (index2 >= 0 && index1 >= 0)
                    {
                        DrawLine(mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index1].x + 15, 720 - playerPositionRect[index1].y - 15, 1.0f)),
                        mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index2].x + 15, 720 - playerPositionRect[index2].y - 30, 1.0f)), Color.blue, i);
                    }
                    if (index3 >= 0 && index1 >= 0)
                    {
                        DrawLine(mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index1].x + 15, 720 - playerPositionRect[index1].y - 15, 1.0f)),
                        mainCam.ScreenToWorldPoint(new Vector3(playerPositionRect[index3].x + 15, 720 - playerPositionRect[index3].y - 15, 1.0f)), Color.blue, i);
                    }
                }
                count += 2;
            }
            
        }

        /*if (nodeName[selected][0] == 'P')
        {
            sel = GameObject.Find(PortIdX[(int.Parse(nodeName[selected].Trim(charsToTrim)) - 1)]);
            BoxCollider[] bc = sel.GetComponents<BoxCollider>();
            selectedCube[0].transform.position = bc[0].center + sel.transform.position;
            selectedCube[0].transform.localScale = bc[0].size + new Vector3(0.1f, 0.1f, 0.1f);//為了附蓋整個矩形+ new Vector3(0.1f,0.1f,0.1f);
            MeshRenderer mr = selectedCube[0].GetComponent<MeshRenderer>();
            mr.materials[0] = (Material)Resources.Load("New Material.mat");
            mr.material.color = new Color(1f, 0f, 1f, 1f);
            for (int i = 1; i < selectedCube.Count; i++)
                selectedCube[i].transform.position = new Vector3(999999f, 999999f, 999999f);
        }
        else if (nodeName[selected][0] == 'G')
        {
            sel = GameObject.Find(GndIdX[(int.Parse(nodeName[selected].Trim(charsToTrim)) - 1)]);
            BoxCollider[] bc = sel.GetComponents<BoxCollider>();
            selectedCube[0].transform.position = bc[0].center + sel.transform.position;
            selectedCube[0].transform.localScale = bc[0].size + new Vector3(0.1f, 0.1f, 0.1f);
            MeshRenderer mr = selectedCube[0].GetComponent<MeshRenderer>();
            mr.materials[0] = (Material)Resources.Load("New Material.mat");//, typeof(Material));
            mr.material.color = new Color(1f, 0f, 1f, 1f);
            for (int i = 1; i < selectedCube.Count; i++)
                selectedCube[i].transform.position = new Vector3(999999f, 999999f, 999999f);
        }
        else if (nodeName[selected][0] == 'C')
        {
            for (int i = 0; i < newObject.capacitance[(int.Parse(nodeName[selected].Trim(charsToTrim)) - 1)].Count; i++)
            {
                sel = GameObject.Find(newObject.capacitance[(int.Parse(nodeName[selected].Trim(charsToTrim)) - 1)][i]);
                BoxCollider[] bc = sel.GetComponents<BoxCollider>();
                selectedCube[i].transform.position = bc[0].center + sel.transform.position;
                selectedCube[i].transform.localScale = bc[0].size + new Vector3(0.1f, 0.1f, 0.1f);
                MeshRenderer mr = selectedCube[i].GetComponent<MeshRenderer>();
                mr.materials[0] = (Material)Resources.Load("New Material.mat");//, typeof(Material));
                mr.material.color = new Color(1f, 0f, 1f, 1f);
            }
            for (int i = 2; i < selectedCube.Count; i++)
                selectedCube[i].transform.position = new Vector3(999999f, 999999f, 999999f);
        }
        else if (nodeName[selected][0] == 'L')
        {
            for (int i = 0; i < newObject.inductance[(int.Parse(nodeName[selected].Trim(charsToTrim)) - 1)].Count - 1; i++)
            {
                sel = GameObject.Find(newObject.inductance[(int.Parse(nodeName[selected].Trim(charsToTrim)) - 1)][i]);
                BoxCollider[] bc = sel.GetComponents<BoxCollider>();
                selectedCube[i].transform.position = bc[0].center + sel.transform.position;
                selectedCube[i].transform.localScale = bc[0].size + new Vector3(0.1f, 0.1f, 0.1f);
                MeshRenderer mr = selectedCube[i].GetComponent<MeshRenderer>();
                mr.materials[0] = (Material)Resources.Load("New Material.mat");//, typeof(Material));
                mr.material.color = new Color(1f, 0f, 1f, 1f);
            }
            for (int i = newObject.inductance[(int.Parse(nodeName[selected].Trim(charsToTrim)) - 1)].Count - 1; i < selectedCube.Count; i++)
                selectedCube[i].transform.position = new Vector3(999999f, 999999f, 999999f);
        }
        else
        {
            string textinput = System.IO.File.ReadAllText("Output_net.txt");
            string[] linebyline = textinput.Replace("\n", "").Split(new char[] { '\r' });
            int j = 0;
            for (int i = 0; i < linebyline.Length - 1; i++)
            {
                string[] wordbyword = linebyline[i].Split('\t');

                if (nodeName[selected] == wordbyword[1])
                {
                    sel = GameObject.Find(wordbyword[0]);
                    BoxCollider[] bc = sel.GetComponents<BoxCollider>();
                    selectedCube[j].transform.position = bc[0].center + sel.transform.position;
                    selectedCube[j].transform.localScale = bc[0].size + new Vector3(0.1f, 0.1f, 0.1f);
                    MeshRenderer mr = selectedCube[j].GetComponent<MeshRenderer>();
                    mr.materials[0] = (Material)Resources.Load("New Material.mat");
                    mr.material.color = new Color(1f, 0f, 1f, 1f);
                    j++;
                }
            }
            for (int i = j; i < selectedCube.Count; i++)
                selectedCube[i].transform.position = new Vector3(999999f, 999999f, 999999f);
        }
        if (selectOBJ.selected == "")
        {
            for (int i = 0; i < selectedCube.Count; i++)
                selectedCube[i].transform.position = new Vector3(999999f, 999999f, 999999f);
        }*/

    }
    
    
    void OnGUI()
    {
        Texture2D textureRed = new Texture2D(50, 50);
        Texture2D textureGreen = new Texture2D(50, 50);
        Texture2D textureBlue = new Texture2D(50, 50);
        Texture2D textureYellow = new Texture2D(50, 50);
        Texture2D textureRedBlue = new Texture2D(50, 50);
        Texture2D textureGreenBlue = new Texture2D(50, 50);
        textureRed.LoadImage(imageRed.bytes);
        textureGreen.LoadImage(imageGreen.bytes);
        textureBlue.LoadImage(imageBlue.bytes);
        textureYellow.LoadImage(imageYellow.bytes);
        textureRedBlue.LoadImage(imageRedBlue.bytes);
        textureGreenBlue.LoadImage(imageGreenBlue.bytes);

        for (int i = 0; i < countPG + countL + countC + countN; i++)
        {
            if (nodeName[i][0] == 'P')
                GUI.Button(playerPositionRect[i], new GUIContent(nodeName[i], textureRed));         
            else if (nodeName[i][0] == 'G')
                GUI.Button(playerPositionRect[i], new GUIContent(nodeName[i], textureRed));
            else if (nodeName[i][0] == 'L')
                GUI.Button(playerPositionRect[i], new GUIContent(nodeName[i], textureYellow));
            else if (nodeName[i][0] == 'C')
                GUI.Button(playerPositionRect[i], new GUIContent(nodeName[i], textureGreenBlue));
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
}
