﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public Moving movingScript;
	public GameObject playerPref;

	private Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Left(){
		movingScript.RotateLeft ();
	}
	public void Right(){
		movingScript.RotateRight ();
	}
	public void Jump(){
		if (movingScript.Jump ()) {
			anim.SetTrigger("jump");
		}
	}

}
