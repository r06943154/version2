#pragma strict

function Start () {
	Cursor.visible=false;
}

function Update () {
	var rect : RectTransform =GetComponent.<RectTransform>();
	rect.anchoredPosition = Input.mousePosition;
}
