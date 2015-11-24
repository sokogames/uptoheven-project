using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	public InputHandler inputHandler;
	public GameObject actor;
	public ActionManager rotationActions;
	public ActionManager jumpActions;

	private bool previousTap;

	// Use this for initialization
	void Start () {
		previousTap = false;
	}
	
	// Update is called once per frame
	void Update () {
		// Update is called once per frame

		if (previousTap) {
			jumpActions.AddAction(new JumpAction(actor));
			previousTap = false;
		}	

		InputCommand command = inputHandler.InputHandle();

		if (command != InputCommand.None) {
			if (GameController.GameState != GameController.GAME_STATE_PLAY) {
				
				GameObject.Find ("_main").GetComponent<GameController> ().StartGame ();
			}
		}

		switch (command) {
		case InputCommand.Tap:
			rotationActions.AddAction(new RotateAction(actor,RotateAction.ACTION_FACE_FORWARD));
			previousTap = true;
			break;
		case InputCommand.Left: 
			rotationActions.AddAction(new RotateAction(actor,RotateAction.ACTION_FACE_LEFT));
			break;
		case InputCommand.Right: 
			rotationActions.AddAction(new RotateAction(actor,RotateAction.ACTION_FACE_RIGHT));
			break;
		case InputCommand.Up: 
			rotationActions.AddAction(new RotateAction(actor,RotateAction.ACTION_FACE_FORWARD));
			break;
		case InputCommand.Down: 
			rotationActions.AddAction(new RotateAction(actor,RotateAction.ACTION_FACE_BACKWARD));
			break;
		case InputCommand.Realese:
			jumpActions.AddAction(new JumpAction(actor));
			//actions.AddAction(new RotateAction(actions.gameObject,RotateAction.ACTION_FACE_FORWARD));
			break;
		default:
			break;
		}
	}
}
