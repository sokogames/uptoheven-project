﻿using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	public const int GAME_PLAY = 0;

	public GlobalData globalData;
	public Controller[] controllers;
	// Use this for initialization
	void Start () {
		GameController controller = (GameController)controllers [GAME_PLAY];
		controller.player.playerPref = globalData.PlayerPref;
		controller.stairway.stepPartPref = globalData.stepPartPref;
		controller.obscaleController.staticObscalesPrefs = globalData.staticObscalesPrefs;
		controller.obscaleController.dynamicObscalePrefs = globalData.dynamicObscalesPrefs;
		controller.init ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}