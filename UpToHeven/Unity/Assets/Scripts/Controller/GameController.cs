using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : Controller {

	public const string GAME_STATE_MENU = "game menu"; 
	public const string GAME_STATE_PLAY = "game play";
	public const string GAME_STATE_CHOOSE_PLAYER = "game choose player";

	public static string GameState;

	public Player player;
	public Stairway stairway;

	public int distanceToNextStep = 10;
	public float stepHeight = 0.2f;	

	public float playerJumpTime;
	public float playerRedyForJumpTime;

	public float gameOverDelta = 3.0f;
	public GameObject playerList;

	public Text scoreText;

	public void init(){

		//init player
		CreatePlayerBody();

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
		scoreText.text = player.maxPosition.ToString ();

	}
	public void GameOver(){
		Debug.Log ("game over");
		Application.LoadLevel(0);
		GetComponent<DarkOrLight> ().StopDarkOrLight();
	}
	public void StartGame(){
		GameController.GameState = GameController.GAME_STATE_PLAY;
		stairway.StartFall ();
		GetComponent<DarkOrLight> ().StartDarkOrLight ();
		GameObject.Find ("MainMenuCanvas").SetActive(false);
		playerList.SetActive (false);
	}
	public void ViewPlayerList(){
		GameController.GameState = GameController.GAME_STATE_CHOOSE_PLAYER;
		playerList.GetComponent<PlayerList>().Show(true);	
	}
	public void ChoosePlayer(GameObject playerPrefab){
		CreatePlayerBody ();
		StartGame ();
	}
	void CreatePlayerBody(){
		
		GameObject playerBody = (GameObject)Instantiate (playerList.GetComponent<PlayerList>().GetCurrentPrefab(), new Vector3 (), Quaternion.Euler(new Vector3(0,0,0)));

		GameObject.DestroyImmediate(player.transform.Find ("body").gameObject);
		playerBody.name = "body";
		playerBody.transform.parent = player.transform;
		playerBody.GetComponent<Renderer> ().material.color = Color.white;
		playerBody.transform.localPosition = new Vector3 (0, 0, 0);
	
	}
}
