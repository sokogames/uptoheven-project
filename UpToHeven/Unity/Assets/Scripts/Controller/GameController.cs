using UnityEngine;
using System.Collections;

public class GameController : Controller {

	public Player player;
	public Stairway stairway;
	public ObscaleController obscaleController;

	public float playerJumpTime;
	public float playerRedyForJumpTime;

	public ActionQueue actions;

	public void init(){

		//init player
		GameObject playerPref = (GameObject)Instantiate (player.playerPref, new Vector3 (), Quaternion.Euler(new Vector3(0,0,0)));
		playerPref.transform.parent = player.transform;
		playerPref.transform.localPosition = Vector3.zero;
		playerPref.transform.localScale = new Vector3 (1, 1, 1);

		actions.actor = player;
		//init stairway
		stairway.Generate (obscaleController);
	}
	// Use this for initialization
	void Start () {
								
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			actions.AddAction(new Action (ActionType.readyForJump, playerRedyForJumpTime, MovingDirection.left));
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			actions.AddAction(new Action (ActionType.readyForJump, playerRedyForJumpTime, MovingDirection.right));
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			actions.AddAction(new Action (ActionType.readyForJump, playerRedyForJumpTime, MovingDirection.back));
		}
		if(Input.GetKeyDown(KeyCode.Space)){
			actions.AddAction(new Action (ActionType.readyForJump, playerRedyForJumpTime, MovingDirection.forward));
		}
		if (Input.GetKeyUp (KeyCode.LeftArrow)
		   ||
			Input.GetKeyUp (KeyCode.RightArrow)
		   ||
			Input.GetKeyUp (KeyCode.DownArrow)
		   ||
			Input.GetKeyUp (KeyCode.Space)
		   ) {
			actions.AddAction(new Action (ActionType.jump, playerRedyForJumpTime));
		}
	}
}
