using UnityEngine;
using System.Collections;

public class ButtonScrollDown : MonoBehaviour {

	private const float delta = 0.1f;
	private const float scrollY = 40.0f;
	private const float screenSizeStandard = 500.0f;
	private const float speed = 10;
	private Vector3 toPosition;
	private bool scrollDown = false;
	private RectTransform rectTransform;
	
	// Use this for initialization
	void Start () {
		scrollDown = false;
		rectTransform = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(scrollDown){
			rectTransform.position = Vector3.Lerp(rectTransform.position,toPosition,Time.deltaTime * speed);
			if(Vector3.Distance(rectTransform.position,toPosition) <= delta){
				rectTransform.position = toPosition;
			}
		}
	}

	public void ScrollDown(){
		if (scrollDown)
			return;
		scrollDown = true;
		toPosition = rectTransform.position - new Vector3 (0, scrollY * Screen.height / screenSizeStandard, 0);
	}
}
