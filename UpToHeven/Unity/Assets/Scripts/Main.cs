using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	public float gameSpeedScale = 1.0f;
	public const int GAME_PLAY = 0;

	public GlobalData globalData;
	public Controller[] controllers;
	// Use this for initialization
	void Start () {

		Time.timeScale = gameSpeedScale;

		GameController controller = (GameController)controllers [GAME_PLAY];
		controller.player.playerBody = globalData.PlayerPref;
		//controller.stairway.stepPartPref = globalData.stepPartPref;
		controller.init ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
