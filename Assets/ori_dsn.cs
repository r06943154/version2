using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ori_dsn : MonoBehaviour {
    string allTextmapping_list;
    public Material mat;
	void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
	{
		GameObject myLine = new GameObject();
        myLine.transform.name = "dsn_line";
		myLine.transform.position = start;
		myLine.AddComponent<LineRenderer>();
		LineRenderer lr = myLine.GetComponent<LineRenderer>();
		lr.material = mat;

        lr.startColor = color;
        lr.endColor = color;
        lr.material.color = color;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

	// Use this for initialization
	void Start () {
        allTextmapping_list = System.IO.File.ReadAllText("mapping_list.txt");
        bool firstPoint = true;
		float startPointX = 777, startPointY = 777;//, endPointX, endPointY;
		int leftOverEndPoint=0;
		string allText;
		allText = System.IO.File.ReadAllText ("abc.dsn");
		string[] line;
		string temp="";
		line = allText.Split (new char[]{ '\n' });
		//find maximum x y croodrinate
		float maxX=0, maxY=0, minX=0, minY=0;
		for (int i = 0; i < line.Length - 1; i++) {			
			line [i] = line [i].Trim ();
			line [i] = line [i].Replace ("     "," ");
			line [i] = line [i].Replace ("    "," ");
			line [i] = line [i].Replace ("   "," ");
			line [i] = line [i].Replace ("  "," ");

			string[] word = line [i].Split (' ');
            //Debug.Log(word);
			if (word [0] == "30") {
				if (firstPoint == true) {
					maxX = float.Parse (word [11]);
					minX = float.Parse (word [11]);
					maxY = float.Parse (word [12]);
					minY = float.Parse (word [12]);
					firstPoint = false;
				} else {
					if (float.Parse (word [11]) > maxX)
						maxX = float.Parse (word [11]);
					if (float.Parse (word [11]) < minX)
						minX = float.Parse (word [11]);
					if (float.Parse (word [12]) > maxY)
						maxY = float.Parse (word [12]);
					if (float.Parse (word [12]) < minY)
						minY = float.Parse (word [12]);
				}
			}
			if (word [0] == "31") {
				if (float.Parse(word [2]) > maxX)
					maxX = float.Parse(word [2]);
				if (float.Parse(word [2]) < minX)
					minX = float.Parse(word [2]);
				if (float.Parse(word [3]) > maxY)
					maxY = float.Parse(word [3]);
				if (float.Parse(word [3]) < minY)
					minY = float.Parse(word [3]);
			}
			if(word[0]=="60")leftOverEndPoint=int.Parse(word[3]);
			if(word[0]=="70"){
				for(int j= 1; j < (word.Length - 1); j = j + 2) { 
					if (startPointX == 777 && startPointY == 777) {
						startPointX = float.Parse (word [j]);
						startPointY = float.Parse (word [j + 1]);
						leftOverEndPoint--;
					}else{
						DrawLine(new Vector3((startPointX-minX)*14.0f/(maxX-minX)-7f,(startPointY-minY)*14.0f/(maxY-minY)-1500f-7f,15.0f),new Vector3((float.Parse(word[j])-minX)*14.0f/(maxX-minX)-7f,(float.Parse(word[j+1])-minY)*14.0f/(maxY-minY)-1500f-7f,15.0f),Color.green, 1);
						startPointX = float.Parse (word [j]);
						startPointY = float.Parse (word [j + 1]);
						leftOverEndPoint--;
					}
					if(leftOverEndPoint<=0){
						startPointX=777;
						startPointY=777;
					}
				}
			}
			if (word [0] == "41") {
				temp = word [4].Trim('"');
			}
			if (word [0].Length>5) {
				GameObject myText = new GameObject (temp);
				TextMesh textMesh = myText.AddComponent<TextMesh>();
				textMesh.text = temp;
				textMesh.color = Color.black;Font ArialFont = (Font)Resources.GetBuiltinResource (typeof(Font), "Arial.ttf");
				textMesh.font = ArialFont;
				myText.transform.position = new Vector3((float.Parse(word[3])-minX)*14.0f/15f*19f/(maxX-minX)-7f/15f*19f,(float.Parse(word[4])-minY)*14.0f/15f*19f/(maxY-minY)-1500f-7f/15f*19f,19.0f);
                myText.AddComponent<BoxCollider>();
            }
		}
	}

	// Update is called once per frame
	void Update () {      
        string[] everyLine = allTextmapping_list.Split(new char[] { '\r', '\n' });
        for (int i = 0; i < everyLine.Length - 2; i += 2)
        {
            string[] everyWord = everyLine[i].Split(new char[] { '\t', ' ' });
            GameObject myText = GameObject.Find(everyWord[0]);
            TextMesh textMesh = myText.GetComponent<TextMesh>();
            textMesh.color = Color.black;
            for (int j = 1; j < everyWord.Length; j++)
            {
                if (everyWord[j] == selectOBJ.selected)
                {
                    GameObject myText2 = GameObject.Find(everyWord[0]);
                    TextMesh textMesh2 = myText.GetComponent<TextMesh>();
                    textMesh.color = Color.red;
                }
            }
        }        
    }   
}
