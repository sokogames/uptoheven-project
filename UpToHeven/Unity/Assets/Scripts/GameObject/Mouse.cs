﻿using UnityEngine;
using System.Collections;

public class Mouse : MonoBehaviour {

	public GameObject movingPart;
	public GameObject mouse;
	public float liftSpeed;
	public float mouseSpeed;
	public float startLiftTime = 5.0f;
	public float mouseTime = 5.0f;
	public bool onlyOnce = false;
	//moving part
	private Vector3 movingPartPostion;

	static private float movingPartLiftDeep = -1.0f;
	static private float delta = 0.01f;

	private Vector3 movingPartLiftPostion; 
	private Vector3 toPosition;

	//mouse rotateion
	private Quaternion toRotate; 
	private float angle;

	private float delay;

	private bool isMouseOnAction {
		get {
			return _isMouseOnAction;
		}
		set {
			_isMouseOnAction = value;
			mouse.GetComponent<BoxCollider>().enabled = isMouseOnAction;
			this.GetComponent<BoxCollider>().enabled = !isMouseOnAction;
		}
	}
	private bool _isMouseOnAction;

	// Use this for initialization
	void Start () {
		isMouseOnAction = false;
		mouse.SetActive (false);
		movingPartPostion = movingPart.transform.position;
		toPosition = movingPartPostion;
		movingPartLiftPostion = movingPartPostion + new Vector3 (0,movingPartLiftDeep,0);
		delay = (int)Random.Range (1, 4) * startLiftTime;

		Invoke ("MovingPartLiftDown",delay);

		angle = mouse.transform.rotation.eulerAngles.y;
		toRotate = mouse.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance (toPosition, movingPart.transform.position);
		if (distance < delta && distance > 0.0f) {
			movingPart.transform.position = toPosition;
			if (movingPart.transform.position == movingPartLiftPostion) {
				//go up
				//Debug.Log ("down established");
				MovingPartLiftUp ();
			} else {

				isMouseOnAction = mouse.activeSelf;
				if(!onlyOnce){
					float liftDownTime = isMouseOnAction ? mouseTime : delay;
					Invoke("MovingPartLiftDown",liftDownTime);
				}
			}
		} else {
			movingPart.transform.position = Vector3.Lerp(movingPart.transform.position,toPosition,Time.deltaTime * liftSpeed);
		}

		float angleDistance = Quaternion.Angle (toRotate,mouse.transform.rotation);
		if (angleDistance > 0 && angleDistance < 1.0f) {
			mouse.transform.rotation = toRotate;
			RotateMouse();
		} else {
			mouse.transform.rotation = Quaternion.Slerp(mouse.transform.rotation,toRotate,Time.deltaTime * mouseSpeed);
		}
	}
	void MovingPartLiftDown(){
		isMouseOnAction = false;
		toPosition = movingPartLiftPostion;
	}
	void MovingPartLiftUp(){
		toPosition = movingPartPostion;
		RotateMouse ();
		mouse.transform.rotation = Quaternion.identity;
		mouse.SetActive (mouse.activeSelf ? false : true);
	}
	void RotateMouse(){
		angle += (int)Random.Range (1, 3) == 1 ? -90.0f : 90.0f;
		toRotate = Quaternion.Euler (0, angle, 0);
	}
}
