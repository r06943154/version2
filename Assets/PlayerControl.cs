using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour {
    private Transform playCameraTransform;
    private Camera playerCamera;
    private PlayerControl playControl;
    private camMove cammove;
    
	void Start () {
        transform.name = "Player";
        transform.position = new Vector3(600.0f,400.0f,-500.0f);
        if (!isLocalPlayer)
        {
            transform.name = "Player2";
            playCameraTransform = transform.Find("Main Camera");
            playerCamera = playCameraTransform.GetComponent<Camera>();
            playControl = GetComponent<PlayerControl>();
            cammove = GetComponent<camMove>();
            if (playerCamera)
            {
                playerCamera.enabled = false;
            }
            if (cammove)
            {
                cammove.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
