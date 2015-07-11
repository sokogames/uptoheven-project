using UnityEngine;
using System.Collections;

public class GameController : Controller {

	public Player player;
	public Stairway stairway;
	public ObscaleController obscaleController;
	
	public void init(){
		//init player
		GameObject playerPref = (GameObject)Instantiate (player.playerPref, new Vector3 (), Quaternion.Euler(new Vector3(0,0,0)));
		playerPref.transform.parent = player.transform;
		playerPref.transform.localPosition = Vector3.zero;
		playerPref.transform.localScale = new Vector3 (1, 1, 1);
		//init stairway
		stairway.Generate (obscaleController);
	}
	// Use this for initialization
	void Start () {
								
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			player.Left();
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			player.Right();
		}
		if(Input.GetKeyDown(KeyCode.Space)){
			player.Jump();
		}
	}
}
