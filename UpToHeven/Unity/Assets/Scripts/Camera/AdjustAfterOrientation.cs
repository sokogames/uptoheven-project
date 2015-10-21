using UnityEngine;
using System.Collections;

public class AdjustAfterOrientation : MonoBehaviour {

	private Camera currentCamera;
	private float landscapeSize;
	public float portraitSize;
	// Use this for initialization
	void Start () {
		currentCamera = GetComponent<Camera> ();
		float scale = Mathf.Min((float)Screen.width,(float)Screen.height) / Mathf.Max((float)Screen.width,(float)Screen.height);
		landscapeSize = portraitSize * scale;														
	}
	
	// Update is called once per frame
	void Update () {
		if (isLandscape()) 															{
			currentCamera.orthographicSize = landscapeSize;
		} else {
			currentCamera.orthographicSize = portraitSize;
		}
	}
	bool isLandscape(){
		return Input.deviceOrientation == DeviceOrientation.LandscapeLeft || 
			Input.deviceOrientation == DeviceOrientation.LandscapeRight;
	}
}
