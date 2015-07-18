using UnityEngine;
using System.Collections;

public enum InputCommand{Left,Right,Up,Down,Realese,None};

public class InputHandler : MonoBehaviour {

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
			
			if((fp.x - lp.x) > 80) // left swipe
			{
				return InputCommand.Left;
			}
			else if((fp.x - lp.x) < -80) // right swipe
			{
				return InputCommand.Right;
			}
			else if((fp.y - lp.y) < -80 ) // up swipe
			{
				return InputCommand.Up;
			}else if((fp.y - lp.y) > 80 ) // down swipe
			{
				return InputCommand.Down;
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
