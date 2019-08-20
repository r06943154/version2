using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;

public class draw : NetworkBehaviour
{
    private int count;
    private int count1;
    private Vector3 tempCam_position;
    private Vector3 tempCam_position1;

    void Start()
    {
        count = 0;
        count1 = 0;
        tempCam_position = new Vector3(0, 0, 0);
        tempCam_position1 = new Vector3(0, 0, 0);
    }

    [SyncVar]
    public string select;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Joystick1Button1))
        {
            select = selectOBJ.selected;
            for (int k = 1; k < newObject.port.Count + 1; k++)
            {
                if (select == "P" + k)
                {
                    if (isServer)
                    {
                        RpcSelect("P" + k);
                        RpcSpawn3();
                        RpcSpawnPort(k);
                    }
                    if (isClient)
                        CmdSelect("P" + k);
                    Spawn3();
                    SpawnPort(k);
                    CmdSpawn3();
                    CmdSpawnPort(k);
                    
                    
                    //localSelect("P" + k);
                }
            }
            for (int k = 1; k < newObject.inductance.Count + 1; k++)
            {
                if (select == "L" + k)
                {
                    if (isServer)
                    {
                        RpcSelect("L" + k);
                        RpcSpawn3();
                        RpcSpawnInductance(k);
                    }
                    if (isClient)
                        CmdSelect("L" + k);                    
                    Spawn3();
                    SpawnInductance(k);
                    CmdSpawn3();
                    CmdSpawnInductance(k);

                    //localSelect("L" + k);
                }
            }
            for (int k = 1; k < newObject.capacitance.Count + 1; k++)
            {
                if (select == "C" + k)
                {
                    if (isServer)
                    {
                        RpcSelect("C" + k);
                        RpcSpawn3();
                        RpcSpawnCapacitance(k);
                    }
                    if (isClient)
                        CmdSelect("C" + k);                    
                    Spawn3();
                    SpawnCapacitance(k);
                    CmdSpawn3();
                    CmdSpawnCapacitance(k);                                     
                    //localSelect("C" + k);
                }
            }
            for (int k = 1; k < newObject.equal_node.Count + 1; k++)
            {
                if (select == k.ToString())
                {
                    if (isServer)
                    {
                        RpcSelect(k.ToString());
                        RpcSpawn3();
                        RpcSpawnNet(k);
                    }
                    if (isClient)
                        CmdSelect(k.ToString());                    
                    Spawn3();
                    SpawnNet(k);
                    CmdSpawn3();
                    CmdSpawnNet(k);
                    
                    //localSelect(k.ToString());
                }
            }
        }
        if (isLocalPlayer)
        {
            select = selectOBJ.selected;

            GameObject oriCam = GameObject.Find("Camera");

            if (Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.Joystick1Button2))
            {
                if (count == 0)
                {
                    tempCam_position = transform.position;
                    transform.position = oriCam.transform.position;
                    oriCam.transform.position = tempCam_position;
                    CmdOriCam(tempCam_position);
                    count = 1;
                }
                else
                {
                    tempCam_position = transform.position;
                    transform.position = oriCam.transform.position;
                    //oriCam.transform.position = tempCam_position;
                    CmdOriCam(tempCam_position);
                    count = 0;
                }
            }

            GameObject Player2 = GameObject.Find("Player2");

            if (Input.GetKey(KeyCode.J))
            {
                if (count1 == 0)
                {
                    tempCam_position1 = transform.position;
                    transform.position = Player2.transform.position;
                    count1 = 1;
                }
                else
                {
                    transform.position = tempCam_position1;
                    count1 = 0;
                }
                Debug.Log("keydown J ");
            }
        }
    }
    [Command]
    void CmdOriCam(Vector3 k)
    {
        GameObject oriCam = GameObject.Find("Camera");
        oriCam.transform.position = k;
        NetworkServer.Spawn(oriCam);
    }
    [ClientRpc]
    void RpcOriCam(Vector3 k)
    {
        GameObject oriCam = GameObject.Find("Camera");
        oriCam.transform.position = k;
    }
    [Command]
    void CmdSelect(string select)
    {
        selectOBJ.selected = select;
    }
    [ClientRpc]
    void RpcSelect(string select)
    {
        selectOBJ.selected = select;

    }
    void localSelect(string select)
    {
        selectOBJ.selected = select;
    }
    [Command]
    void CmdSpawnPort(int k)
    {
        int j = int.Parse(newObject.port[k - 1]);
        GameObject sel = GameObject.Find("" + j);
        BoxCollider[] bc = sel.GetComponents<BoxCollider>();
        GameObject rec = GameObject.Find("sel" + j);
        //rec.transform.position = bc[0].center + sel.transform.position;
        rec.transform.position = sel.transform.position + new Vector3(0.008f, 0.008f, -0.008f);
        //NetworkServer.Spawn(rec);
    }
    [ClientRpc]
    void RpcSpawnPort(int k)
    {
        int j = int.Parse(newObject.port[k - 1]);
        GameObject sel = GameObject.Find("" + j);
        BoxCollider[] bc = sel.GetComponents<BoxCollider>();
        GameObject rec = GameObject.Find("sel" + j);
        //rec.transform.position = bc[0].center + sel.transform.position;
        rec.transform.position = sel.transform.position + new Vector3(0.008f, 0.008f, -0.008f);
        //NetworkServer.Spawn(rec);
    }
    void SpawnPort(int k)
    {
        int j = int.Parse(newObject.port[k - 1]);
        GameObject sel = GameObject.Find("" + j);
        BoxCollider[] bc = sel.GetComponents<BoxCollider>();
        GameObject rec = GameObject.Find("sel" + j);
        //rec.transform.position = bc[0].center + sel.transform.position;
        rec.transform.position = sel.transform.position + new Vector3(0.008f, 0.008f, -0.008f);
        //NetworkServer.Spawn(rec);
    }
    [Command]
    void CmdSpawnInductance(int k)
    {
        for (int i = 0; i < newObject.inductance[k - 1].Count - 1; i++)
        {
            int j = int.Parse(newObject.inductance[k - 1][i]);
            GameObject sel = GameObject.Find("" + j);
            BoxCollider[] bc = sel.GetComponents<BoxCollider>();
            GameObject rec = GameObject.Find("sel" + j);
            //rec.transform.position = bc[0].center + sel.transform.position;
            rec.transform.position = sel.transform.position + new Vector3(0.008f, 0.008f, -0.008f);
            //NetworkServer.Spawn(rec);
        }
    }
    [ClientRpc]
    void RpcSpawnInductance(int k)
    {
        for (int i = 0; i < newObject.inductance[k - 1].Count - 1; i++)
        {
            int j = int.Parse(newObject.inductance[k - 1][i]);
            GameObject sel = GameObject.Find("" + j);
            BoxCollider[] bc = sel.GetComponents<BoxCollider>();
            GameObject rec = GameObject.Find("sel" + j);
            //rec.transform.position = bc[0].center + sel.transform.position;
            rec.transform.position = sel.transform.position + new Vector3(0.008f, 0.008f, -0.008f);
            //NetworkServer.Spawn(rec);
        }
    }
    void SpawnInductance(int k)
    {
        for (int i = 0; i < newObject.inductance[k - 1].Count - 1; i++)
        {
            int j = int.Parse(newObject.inductance[k - 1][i]);
            GameObject sel = GameObject.Find("" + j);
            BoxCollider[] bc = sel.GetComponents<BoxCollider>();
            GameObject rec = GameObject.Find("sel" + j);
            //rec.transform.position = bc[0].center + sel.transform.position;
            rec.transform.position = sel.transform.position + new Vector3(0.008f, 0.008f, -0.008f);
            //NetworkServer.Spawn(rec);
        }
    }
    [Command]
    void CmdSpawnCapacitance(int k)
    {
        for (int i = 0; i < newObject.capacitance[k - 1].Count; i++)
        {
            int j = int.Parse(newObject.capacitance[k - 1][i]);
            GameObject sel = GameObject.Find("" + j);
            BoxCollider[] bc = sel.GetComponents<BoxCollider>();
            GameObject rec = GameObject.Find("sel" + j);
            //rec.transform.position = bc[0].center + sel.transform.position;
            rec.transform.position = sel.transform.position + new Vector3(0.008f, 0.008f, -0.008f);
            //NetworkServer.Spawn(rec);
        }
    }
    [ClientRpc]
    void RpcSpawnCapacitance(int k)
    {
        for (int i = 0; i < newObject.capacitance[k - 1].Count; i++)
        {
            int j = int.Parse(newObject.capacitance[k - 1][i]);
            GameObject sel = GameObject.Find("" + j);
            BoxCollider[] bc = sel.GetComponents<BoxCollider>();
            GameObject rec = GameObject.Find("sel" + j);
            //rec.transform.position = bc[0].center + sel.transform.position;
            rec.transform.position = sel.transform.position + new Vector3(0.008f, 0.008f, -0.008f);
            //NetworkServer.Spawn(rec);
        }
    }
    void SpawnCapacitance(int k)
    {
        for (int i = 0; i < newObject.capacitance[k - 1].Count; i++)
        {
            int j = int.Parse(newObject.capacitance[k - 1][i]);
            GameObject sel = GameObject.Find("" + j);
            BoxCollider[] bc = sel.GetComponents<BoxCollider>();
            GameObject rec = GameObject.Find("sel" + j);
            //rec.transform.position = bc[0].center + sel.transform.position;
            rec.transform.position = sel.transform.position + new Vector3(0.008f, 0.008f, -0.008f);
            //NetworkServer.Spawn(rec);
        }
    }
    [Command]
    void CmdSpawnNet(int k)
    {
        for (int i = 0; i < newObject.equal_node.Count; i++)
        {
            if (newObject.equal_node[i][1] == k.ToString())
            {
                int j = int.Parse(newObject.equal_node[i][0]);
                GameObject sel = GameObject.Find("" + j);
                BoxCollider[] bc = sel.GetComponents<BoxCollider>();
                GameObject rec = GameObject.Find("sel" + j);
                //rec.transform.position = bc[0].center + sel.transform.position;
                rec.transform.position = sel.transform.position + new Vector3(0.008f, 0.008f, -0.008f);
                //NetworkServer.Spawn(rec);
            }
        }
    }
    [ClientRpc]
    void RpcSpawnNet(int k)
    {
        for (int i = 0; i < newObject.equal_node.Count; i++)
        {
            if (newObject.equal_node[i][1] == k.ToString())
            {
                int j = int.Parse(newObject.equal_node[i][0]);
                GameObject sel = GameObject.Find("" + j);
                BoxCollider[] bc = sel.GetComponents<BoxCollider>();
                GameObject rec = GameObject.Find("sel" + j);
                //rec.transform.position = bc[0].center + sel.transform.position;
                rec.transform.position = sel.transform.position + new Vector3(0.008f, 0.008f, -0.008f);
                //NetworkServer.Spawn(rec);
            }
        }
    }
    void SpawnNet(int k)
    {
        for (int i = 0; i < newObject.equal_node.Count; i++)
        {
            if (newObject.equal_node[i][1] == k.ToString())
            {
                int j = int.Parse(newObject.equal_node[i][0]);
                GameObject sel = GameObject.Find("" + j);
                BoxCollider[] bc = sel.GetComponents<BoxCollider>();
                GameObject rec = GameObject.Find("sel" + j);
                //rec.transform.position = bc[0].center + sel.transform.position;
                rec.transform.position = sel.transform.position + new Vector3(0.008f, 0.008f, -0.008f);
                //NetworkServer.Spawn(rec);
            }
        }
    }
    [Command]
    void CmdSpawn3()
    {
        for (int i = 0; i < newObject.Rect_Num; i++)
        {
            GameObject rec = GameObject.Find("sel" + i);
            rec.transform.position = new Vector3(999999f, 999999f, 999999f);
            //NetworkServer.Spawn(rec);
        }
    }
    [ClientRpc]
    void RpcSpawn3()
    {
        for (int i = 0; i < newObject.Rect_Num; i++)
        {
            GameObject rec = GameObject.Find("sel" + i);
            rec.transform.position = new Vector3(999999f, 999999f, 999999f);
            //NetworkServer.Spawn(rec);
        }
    }
    void Spawn3()
    {
        for (int i = 0; i < newObject.Rect_Num; i++)
        {
            GameObject rec = GameObject.Find("sel" + i);
            rec.transform.position = new Vector3(999999f, 999999f, 999999f);
            //NetworkServer.Spawn(rec);
        }
    }
}
