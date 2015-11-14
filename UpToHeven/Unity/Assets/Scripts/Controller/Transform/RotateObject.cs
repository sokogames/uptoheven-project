using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {
	
	public float speed  = 20;

	private static float MIN_CHECK_VALUE = 10.0f; 

	private Quaternion _toRotation;
	public Quaternion toRotation{
		get{
			return _toRotation;
		}
		set{
			done = false;
			_toRotation = value;
		}
	}
	private bool done;

	void Start () {
		toRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {

		if (done) {
			return;
		}

		if (Quaternion.Angle (transform.rotation, toRotation) > RotateObject.MIN_CHECK_VALUE) {

			transform.rotation = Quaternion.Lerp (transform.rotation, toRotation, Time.deltaTime * speed);

		} else {
			transform.rotation = toRotation;
			done = true;
		
		}



	}
	public void RotateLeft(){
		toRotation = transform.rotation * Quaternion.Euler (0, 270.0f, 0);
	}
	public void RotateRight(){
		toRotation = transform.rotation * Quaternion.Euler (0, 90.0f, 0);
	}
	public void Reverse(){
		toRotation = transform.rotation * Quaternion.Euler (0, 180.0f, 0);
	}
	public void FaceForward(){
		toRotation = Quaternion.Euler (0, 0.0f, 0);
	}
	public void FaceBackward(){
		toRotation = Quaternion.Euler (0, 180.0f, 0);
	}
	public void FaceLeft(){
		toRotation = Quaternion.Euler (0, -90.0f, 0);
	}
	public void FaceRight(){
		toRotation = Quaternion.Euler (0, 90.0f, 0);
	}
	public bool Done(){
		return done;
	}
}
