﻿using UnityEngine;
using System.Collections;

public class Fatalit : MonoBehaviour {

	public MonoBehaviour[] scriptsToDisable; 
	public string tagName;
	public float afterTime;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == tagName) {
			DisableSripts();
			StopAllCoroutines ();
			Invoke("DoFatalit",afterTime);
		}
	}
	void DisableSripts(){
		foreach(MonoBehaviour monoBehaviour in scriptsToDisable){
			Destroy(monoBehaviour);
		}
	}

	void DoFatalit(){
		Animator anim = GetComponent<Animator> ();
		anim.enabled = true;
		if (Mathf.Abs(transform.rotation.eulerAngles.y - 270.0f) < 10.0f) {
			anim.SetBool(Animator.StringToHash("isLeft"),true);
		}
	}
}
