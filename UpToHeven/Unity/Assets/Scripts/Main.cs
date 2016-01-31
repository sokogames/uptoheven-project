using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	public float gameSpeedScale = 1.0f;
	public const int GAME_PLAY = 0;
	
	public Controller[] controllers;
	// Use this for initialization
	void Start () {

		Time.timeScale = gameSpeedScale;

		GameController controller = (GameController)controllers [GAME_PLAY];
		//controller.stairway.stepPartPref = globalData.stepPartPref;
		controller.init ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
