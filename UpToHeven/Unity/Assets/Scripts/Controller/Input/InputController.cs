using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	public InputHandler inputHandler;
	public ActionManager actions;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Update is called once per frame
			InputCommand command = inputHandler.InputHandle();
			
			switch (command) {
			case InputCommand.Left: 
				actions.AddAction(new RotateAction(actions.gameObject,RotateAction.ACTION_FACE_LEFT));
				break;
			case InputCommand.Right: 
				actions.AddAction(new RotateAction(actions.gameObject,RotateAction.ACTION_FACE_RIGHT));
				break;
			case InputCommand.Up: 
				actions.AddAction(new RotateAction(actions.gameObject,RotateAction.ACTION_FACE_FORWARD));
				break;
			case InputCommand.Down: 
				actions.AddAction(new RotateAction(actions.gameObject,RotateAction.ACTION_FACE_BACKWARD));
				break;
			case InputCommand.Realese:
				actions.AddAction(new JumpAction(actions.gameObject));
				actions.AddAction(new RotateAction(actions.gameObject,RotateAction.ACTION_FACE_FORWARD));
				break;
			default:
				break;
			}
			
	}
}
