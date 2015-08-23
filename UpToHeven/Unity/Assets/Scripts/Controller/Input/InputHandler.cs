using UnityEngine;
using System.Collections;

public enum InputCommand{Left,Right,Up,Down,Realese,None};

public class InputHandler : MonoBehaviour {

	public float touchSension = 80.0f;
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
	private InputCommand MobileCommandOld(){
		Touch touch = Input.GetTouch (0);
		Vector2 fp = Vector2.zero;
		Vector2 lp = Vector2.zero;

		if (touch.phase == TouchPhase.Ended) {

			return InputCommand.Realese;	
		}

		if (touch.phase == TouchPhase.Began)
		{
			fp = touch.position;
		}

		if (touch.phase == TouchPhase.Moved)
		{
			lp = touch.position;
			
			if((fp.x - lp.x) > touchSension) // left swipe
			{
				return InputCommand.Left;
			}
			if((fp.x - lp.x) < -touchSension) // right swipe
			{
				return InputCommand.Right;
			}
			if((fp.y - lp.y) < -touchSension ) // up swipe
			{
				return InputCommand.Up;
			}
			if((fp.y - lp.y) > touchSension ) // down swipe
			{
				return InputCommand.Down;
			}
		}
		return InputCommand.None;
	}
	private InputCommand MobileCommand(){

		if (Input.touchCount != 1)
			return InputCommand.None;

		Touch touch = Input.GetTouch (0);
		
		if (touch.phase == TouchPhase.Ended) {
			
			return InputCommand.Realese;	
		}
		
		if (touch.phase == TouchPhase.Moved)
		{
			Vector2 delta = touch.deltaPosition;

			if(Mathf.Abs (delta.x) > Mathf.Abs (delta.y)){

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
