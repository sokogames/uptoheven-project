using UnityEngine;
using System.Collections;

public class GameController : Controller {

	public const string GAME_STATE_MENU = "game menu"; 
	public const string GAME_STATE_PLAY = "game play";

	public static string GameState;

	public Player player;
	public Stairway stairway;
	public ObscaleController obscaleController;
	public int distanceToNextStep = 10;
	public float stepHeight = 0.2f;	

	public float playerJumpTime;
	public float playerRedyForJumpTime;

	public float gameOverDelta = 3.0f;

	public Light light;

	public void init(){

		//init player
		GameObject playerBody = (GameObject)Instantiate (player.playerBody, new Vector3 (), Quaternion.Euler(new Vector3(0,0,0)));

		GameObject.DestroyImmediate(player.transform.Find ("body").gameObject);
		playerBody.name = "body";
		playerBody.transform.parent = player.transform;
		playerBody.transform.localPosition = new Vector3 (0, 0, 0);

		//init stairway
		GameController.GameState = GameController.GAME_STATE_MENU;
	}
	// Use this for initialization
	void Start () {
								
	}
	
	// Update is called once per frame
	void Update () {

		if (stairway.totalSteps - player.currentStepPostion < distanceToNextStep) {
			stairway.addRandomChunk();
		}
		if (player.enemyTouched) {
			Invoke("GameOver",gameOverDelta);
		}
		if (player.transform.position.y < (player.currentStepPostion - 10) * stepHeight) {
			Invoke("GameOver",1.0f);
		}

	}
	public void GameOver(){
		Debug.Log ("game over");
		Application.LoadLevel(0);
	}
	public void StartGame(){

		GameController.GameState = GameController.GAME_STATE_PLAY;
		GameObject.Find ("StartButton").SetActive (false);
		stairway.StartFall ();
		light.intensity = 1.7f;
		Debug.Log ("click");
	}
}
