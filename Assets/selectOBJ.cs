using UnityEngine;
using UnityEngine.Networking;

public class selectOBJ :  NetworkBehaviour{
    public static string selected = "";
    public Camera mainCam;
    // Use this for initialization
    void Start () {
      
    }
    
    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Joystick1Button1))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hitInfo);

            if (hit)
            {
                for (int i = 0; i < newObject.capacitance.Count; i++)
                {
                    for (int m = 0; m < newObject.capacitance[i].Count; m++)
                    {
                        if (newObject.capacitance[i][m] == hitInfo.transform.gameObject.name)
                        {
                            selected = "C" + (i + 1).ToString();
                            Debug.Log(selected);
                        }
                    }
                }
                for (int i = 0; i < newObject.inductance.Count; i++)
                {
                    for (int m = 0; m < newObject.inductance[i].Count; m++)
                    {
                        if (newObject.inductance[i][m] == hitInfo.transform.gameObject.name)
                        {
                            selected = "L" + (i + 1).ToString();
                            Debug.Log(selected);
                        }
                    }                    
                }
                for (int i = 0; i < newObject.port.Count; i++)
                {
                    if (newObject.port[i] == hitInfo.transform.gameObject.name)
                        selected = "P" + (i + 1).ToString();
                }
                for (int i = 0; i < newObject.mapping_list.Count; i++)
                {
                    if (hitInfo.transform.name == newObject.mapping_list[i][0])
                    {
                        selected = newObject.mapping_list[i][1];
                    }
                }
            }
            else
            {
                Debug.Log("No hit");
            }
        }
    }
}
