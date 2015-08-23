using UnityEngine;
using System.Collections;

public class GameController : Controller {

	public Player player;
	public Stairway stairway;
	public ObscaleController obscaleController;
	public int distanceToNextStep = 10;

	public float playerJumpTime;
	public float playerRedyForJumpTime;

	public ActionManager actions;

	public void init(){

		//init player
		GameObject playerBody = (GameObject)Instantiate (player.playerBody, new Vector3 (), Quaternion.Euler(new Vector3(0,0,0)));

		GameObject.DestroyImmediate(player.transform.Find ("body").gameObject);
		playerBody.name = "body";
		playerBody.transform.parent = player.transform;
		playerBody.transform.localPosition = new Vector3 (0, 0.5f, 0);

		//init stairway

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
			Invoke("GameOver",2.0f);
		}

	}
	void GameOver(){
		Application.LoadLevelAsync(0);
	}
}
