using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
//using UnityEditor; //make prefab_1

public class drawline : NetworkBehaviour
{

    void Start()
    {
        for (int i = 0; i < newObject.Rect_Num; i++)
        {
            //Debug.Log(newObject.Rect_Num);
            /*GameObject rec; //2019.6
            if(i < 100)
                rec = Instantiate(Resources.Load("" + i)) as GameObject;
            else
                rec = Instantiate(Resources.Load("100")) as GameObject;*/
            GameObject rec = Instantiate(GameObject.Find("" + i)) as GameObject;
            Destroy(rec.transform.GetChild(0).gameObject);
            //GameObject rec = Instantiate(Resources.Load("selectcube")) as GameObject; //make prefab_2
            GameObject sel = GameObject.Find("" + i);
            BoxCollider[] bc = sel.GetComponents<BoxCollider>();
            rec.transform.name = "sel" + i;
            //rec.transform.localScale= bc[0].size + new Vector3(0.1f, 0.1f, 0.1f);
            rec.transform.localScale = new Vector3(1.01f, 1.01f, 1.01f);
            rec.transform.position = new Vector3(999999f, 999999f, 999999f);
            MeshRenderer mr = rec.GetComponent<MeshRenderer>();
            //mr.materials[0] = (Material)Resources.Load("New Material.mat");
            mr.material.color = new Color(1f, 0f, 1f, 1f);
            //Object prefab = PrefabUtility.CreatePrefab("Assets/Resources/" + i + ".prefab", rec); //make prefab_3          
            ClientScene.RegisterPrefab(rec);
            //selectedCube.Add(rec); 
            //NetworkServer.Spawn(rec);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
