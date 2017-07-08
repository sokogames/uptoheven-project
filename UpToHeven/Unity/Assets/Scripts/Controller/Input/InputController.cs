using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputController : MonoBehaviour {

	private JumperDirection finalDirection;
	private Queue<JumperDirection> jumperQueue;
	private bool pressed;
	public InputHandler inputHandler;
	
	public Player player;
	public int queueLenght; 
	// Use this for initialization
	void Start () {
		player.OnJumperEnded += OnJumperEnded;
		finalDirection = JumperDirection.forward;
		jumperQueue = new Queue<JumperDirection> ();
		pressed = false;
	}
	
	// Update is called once per frame
	void Update () {
		// Update is called once per frame


		InputCommand command = inputHandler.InputHandle();

		if (command != InputCommand.None) {
			if (GameController.GameState != GameController.GAME_STATE_PLAY) {
				
				return;
			}
		}

		if (player.IsDead) {
			return;
		}

		switch (command) {
		case InputCommand.Tap:
			player.QuickJump(JumperDirection.forward);
			break;
		case InputCommand.Left:
			player.Squat();
			finalDirection = JumperDirection.left;
			player.Rotate(finalDirection);
			break;
		case InputCommand.Right: 
			player.Squat();
			finalDirection = JumperDirection.right;
			player.Rotate(finalDirection);
			break;
		case InputCommand.Up: 
			player.Squat();
			finalDirection = JumperDirection.forward;
			player.Rotate(finalDirection);
			break;
		case InputCommand.Down: 
			player.Squat();
			finalDirection = JumperDirection.backward;
			player.Rotate(finalDirection);
			break;
		case InputCommand.Realese:
			player.Jump(finalDirection);
			break;
		case InputCommand.TouchBegin:
			player.Squat();
			break;
		case InputCommand.Canceled:
			player.UnSquat();
			break;
		default:
			break;
		}
		//if(command != InputCommand.None)
		//Debug.Log (command + " " + finalDirection);
	}

	void OnJumperEnded(){
		//Debug.Log ("Done man");	
	}
}
