using UnityEngine;
using System.Collections;

public class GameController : Controller {

	public Player player;
	public Stairway stairway;
	public ObscaleController obscaleController;
	public int distanceToNextStep = 10;

	public float playerJumpTime;
	public float playerRedyForJumpTime;

	public ActionQueue actions;

	private InputHandler inputHandler;

	public void init(){

		//init player
		GameObject playerPref = (GameObject)Instantiate (player.playerPref, new Vector3 (), Quaternion.Euler(new Vector3(0,0,0)));

		GameObject obj = player.GetComponent<GameObject> ();
		//obj = playerPref;
		playerPref.transform.parent = player.transform;
		playerPref.transform.localPosition = new Vector3(0,0.5f,0);
		playerPref.transform.localScale = new Vector3 (1, 1, 1);

		actions.actor = player;
		//init stairway
		stairway.Generate (obscaleController);
		inputHandler = (InputHandler)gameObject.AddComponent<InputHandler> ();

	}
	// Use this for initialization
	void Start () {
								
	}
	
	// Update is called once per frame
	void Update () {
		InputCommand command = inputHandler.InputHandle();

		if (player.enemyTouched) {
			player.enabled = false;
			Invoke("GameOver",1.5f);
		}

		switch (command) {
		case InputCommand.Left: 
			actions.AddAction(new Action (ActionType.readyForJump, playerRedyForJumpTime, MovingDirection.left));
			break;
		case InputCommand.Right: 
			actions.AddAction(new Action (ActionType.readyForJump, playerRedyForJumpTime, MovingDirection.right));
			break;
		case InputCommand.Down: 
			actions.AddAction(new Action (ActionType.readyForJump, playerRedyForJumpTime, MovingDirection.back));
			break;
		case InputCommand.Up: 
			actions.AddAction(new Action (ActionType.readyForJump, playerRedyForJumpTime, MovingDirection.forward));
			break;
		case InputCommand.Realese:
			actions.AddAction(new Action (ActionType.jump, playerRedyForJumpTime));
			break;
		default:
			break;
		}

		if (stairway.lastStepIndex - player.currentStepPostiion < distanceToNextStep) {
			stairway.AddNextStep();
		}

	}
	void GameOver(){
		Application.LoadLevelAsync(0);
	}
}
