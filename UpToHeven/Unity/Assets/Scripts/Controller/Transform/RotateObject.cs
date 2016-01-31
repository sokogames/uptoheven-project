using UnityEngine;
using System.Collections;

public delegate void RotationEnded(JumperDirection direction);
public delegate void RotationStarted(JumperDirection direction);

public class RotateObject : MonoBehaviour {

	public event RotationEnded OnRotationEnded;
	public event RotationStarted OnRotationStarted;

	public float speed  = 8;

	private static float MIN_CHECK_VALUE = 10.0f; 
	private JumperDirection currentDirection;

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
			if(OnRotationEnded != null){
				OnRotationEnded(currentDirection);
			}
		}



	}

	public void RotateTo(JumperDirection direction){

		currentDirection = direction;

		switch (currentDirection) {
		
		case JumperDirection.left: 
			FaceLeft();
			break;
		case JumperDirection.right: 
			FaceRight();
			break;
		case JumperDirection.forward: 
			FaceForward();
			break;
		case JumperDirection.backward: 
			FaceBackward();
			break;
		default: break;
		}
		if (OnRotationStarted != null) {
			OnRotationStarted (currentDirection);
		}
	
	}

	public void RotateLeft(){
		toRotation = transform.rotation * Quaternion.Euler (0, -91.0f, 0);
	}
	public void RotateRight(){
		toRotation = transform.rotation * Quaternion.Euler (0, 91.0f, 0);
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
		toRotation = Quaternion.Euler (0, -91.0f, 0);
	}
	public void FaceRight(){
		toRotation = Quaternion.Euler (0, 91.0f, 0);
	}
	public bool Done(){
		return done;
	}
}
