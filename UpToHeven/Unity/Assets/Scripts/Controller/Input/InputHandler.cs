using UnityEngine;
using System.Collections;

public enum InputCommand{Left,Right,Up,Down,Realese,None, Tap, TouchBegin, Canceled};

public class InputHandler : MonoBehaviour {

	public float touchSension = 40.0f;
	delegate InputCommand CommandDelgate();

	CommandDelgate command;

	void Start(){
		if(SystemInfo.deviceType == DeviceType.Desktop){
			//we are on a desktop device, so don't use touch
			command = DesktopCommand;
		}
		//if it isn't a desktop, lets see if our device is a handheld device aka a mobile device
		else if(SystemInfo.deviceType == DeviceType.Handheld){
			//we are on a mobile device, so lets use touch input
			command = MobileCommand;
		}
	}
	public InputCommand InputHandle(){
	
		return command();

	}

	private InputCommand MobileCommand(){

		if (Input.touchCount == 0)
			return InputCommand.None;

		Touch touch = Input.GetTouch (0);

		if (touch.phase == TouchPhase.Ended) {


			if (touch.tapCount > 0) {
				Debug.Log("tap not taouch");
				return InputCommand.Tap;
			}

			Debug.Log ("ended");
			return InputCommand.Realese;	
		}

		if (touch.phase == TouchPhase.Began) {
			return InputCommand.TouchBegin;
		}
		
		if (touch.phase == TouchPhase.Moved)
		{

			Vector2 delta = touch.deltaPosition;

			if(Mathf.Abs (delta.x) > Mathf.Abs (delta.y) * 0.5f){

				if(delta.x < 0){
					return InputCommand.Left;
				}
			
				if(delta.x > 0){
					return InputCommand.Right;
				}

			}else{

				if(delta.y > 0){
					return InputCommand.Up;
				}

				if(delta.y < 0){
					return InputCommand.Down;
				}
			}
		}

		if (touch.phase == TouchPhase.Canceled) {
			return InputCommand.Canceled;
		}

		return InputCommand.None;
	}
	private InputCommand DesktopCommand(){

		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			return InputCommand.Left;
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			return InputCommand.Right;
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			return InputCommand.Down;
		}
		if(Input.GetKeyDown(KeyCode.Space)){
			return InputCommand.Up;
		}
		if (Input.GetKeyUp (KeyCode.LeftArrow)
		    ||
		    Input.GetKeyUp (KeyCode.RightArrow)
		    ||
		    Input.GetKeyUp (KeyCode.DownArrow)
		    ||
		    Input.GetKeyUp (KeyCode.Space)
		    ) {
			return InputCommand.Realese;
		}
		return InputCommand.None;
	}

}
