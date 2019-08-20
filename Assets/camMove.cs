using UnityEngine;
using System.Collections;

public class camMove : MonoBehaviour {
    public Camera mainCam;
    public float turnSpeed = 4.0f;		// Speed of camera turning when mouse moves in along an axis
	public float panSpeed = 4.0f;		// Speed of the camera when being panned
	public float zoomSpeed = 4.0f;		// Speed of the camera going back and forth

	private Vector3 mouseOrigin;	// Position of cursor when mouse dragging starts
	private bool isRotating;	// Is the camera being rotated?
	public string txtFile = "test";

	public Transform target;
	public float distance = 20.0f;
	public float zoomSpd = 2.0f;

	public float xSpeed = 240.0f;
	public float ySpeed = 123.0f;

	public int yMinLimit = -723;
	public int yMaxLimit = 877;

	private float x = 22.0f;
	private float y = 33.0f;
	// Use this for initialization
	void Start () {
		x = 22f;
		y = 33f;
	}
	public float speed = 1.0f;
	// Update is called once per frame
	
	void Update () {
        // Get the right mouse button
        if (Input.GetMouseButtonDown(1))
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isRotating = true;
		}

        // Disable movements on button release
        if (!Input.GetMouseButton(1)) isRotating = false;

		// Rotate camera along X and Y axis
		if (isRotating)
		{
			Vector3 pos = mainCam.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

			transform.RotateAround(transform.position, transform.right, -pos.y * turnSpeed);
			transform.RotateAround(transform.position, Vector3.up, pos.x * turnSpeed);
		}
		if(Input.GetKey(KeyCode.RightArrow))
		{
			transform.Translate(new Vector3(100*speed * Time.deltaTime,0,0));
		}
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			transform.Translate(new Vector3(-100 * speed * Time.deltaTime,0,0));
		}
		if(Input.GetKey(KeyCode.DownArrow))
		{
			transform.Translate(new Vector3(0,-100 * speed * Time.deltaTime,0));
		}
		if(Input.GetKey(KeyCode.UpArrow))
		{
			transform.Translate(new Vector3(0, 100 * speed * Time.deltaTime,0));
		}
		if(Input.GetKey(KeyCode.D))
		{
			transform.Translate(new Vector3(100 * speed * Time.deltaTime,0,0));
		}
		if(Input.GetKey(KeyCode.A))
		{
			transform.Translate(new Vector3(-100 * speed * Time.deltaTime,0,0));
		}
		if(Input.GetKey(KeyCode.S))
		{
			transform.Translate(new Vector3(0,0,-100 * speed * Time.deltaTime));
		}
		if(Input.GetKey(KeyCode.W))
		{
			transform.Translate(new Vector3(0,0, 100 * speed * Time.deltaTime));
		}
		if (target) {
			x -= Input.GetAxis("Horizontal") * xSpeed * 0.02f;
			y += Input.GetAxis("Vertical") * ySpeed * 0.02f;

			y = ClampAngle(y, yMinLimit, yMaxLimit);

			distance -= Input.GetAxis("Fire1") *zoomSpd* 0.02f;
			distance += Input.GetAxis("Fire2") *zoomSpd* 0.02f;

			Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
			Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

			transform.rotation = rotation;
			transform.position = position;
		}
        transform.Translate(this.transform.Find("Main Camera").rotation * new Vector3(100 * speed * Input.GetAxis("Horizontal") * Time.deltaTime, 100 * speed * Input.GetAxis("Fire3") * Time.deltaTime, 100 * speed * Input.GetAxis("Vertical") * Time.deltaTime));

    }
    public static float ClampAngle (float angle, float min, float max) {
		if (angle < -360.0f)
			angle += 360.0f;
		if (angle > 360.0f)
			angle -= 360.0f;
		return Mathf.Clamp (angle, min, max);
	}
}