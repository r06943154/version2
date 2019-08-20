using UnityEngine;
using System.Collections;

public class followPlayer : MonoBehaviour {
	public Transform PlayerPos;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.LookAt (PlayerPos.position);
	}
}
