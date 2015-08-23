using UnityEngine;
using System.Collections;

public enum TransformType {Rotation, Jump}

public enum RotationType {Left, Right, Reverse, ToLeft, ToRight, ToForward, ToBackward}

public enum FacedDirection{Left,Right,Forward,Back};

public class Rotate : MonoBehaviour{

	public ObjectVision playerVision;

	public float jumpHeight = 4.0f;
	public float jumpDistanceOnSide = 1.7f;
	public float jumpDistanceUp = 2.0f;
	public float jumpDistanceDown = 1.5f;

	public float minCheckValue = 0.1f;
	public float speed;
	public FacedDirection facedDirection;

	private Quaternion toRotation;

	private bool isDone =true;

	// Use this for initialization
	void Start () {
		facedDirection = FacedDirection.Forward;
		toRotation = transform.rotation;		
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void RotateTo(ActionMessage message){

		switch (message.rotationType) {
		
		case RotationType.Left: 
			RotateLeft();
			break;
		case RotationType.Right: 
			RotateRight();
			break;
		case RotationType.Reverse: 
			Reverse();
			break;
		case RotationType.ToLeft: 
			ToLeft();
			break;
		case RotationType.ToRight: 
			ToRight();
			break;
		case RotationType.ToForward: 
			ToForward();
			break;
		case RotationType.ToBackward: 
			ToBackward();
			break;
		}

		isDone = false;
	}
	public void Jump(ActionMessage message){
	

		Jump();

		isDone = false;
	}

	private void RotateLeft(){
		toRotation = transform.rotation * Quaternion.Euler (0, -90.0f, 0);
	}
	private void RotateRight(){
		toRotation = transform.rotation * Quaternion.Euler (0, 90.0f, 0);
	}
	private void Reverse(){
		toRotation = transform.rotation * Quaternion.Euler (0, 180.0f, 0);
	}
	private void ToLeft(){
		toRotation = Quaternion.Euler (0, -90.0f, 0);
	}
	private void ToRight(){
		toRotation = Quaternion.Euler (0, 90.0f, 0);
	}
	private void ToForward(){
		toRotation = Quaternion.Euler (0, 0.0f, 0);
	}
	private void ToBackward(){
		toRotation = Quaternion.Euler (0, 180.0f, 0);
	}
	private void Jump(){

		if (playerVision.hasBarrier ())
			return;
		
		if (GetComponent<Rigidbody>().velocity.magnitude > 0.02f) return;
		
		float jumpX = 0, jumpZ = 0;
		
		switch (facedDirection) {
		case FacedDirection.Left: jumpX = -jumpDistanceOnSide; jumpZ = 0; break;
		case FacedDirection.Right: jumpX = jumpDistanceOnSide; jumpZ = 0; break;
		case FacedDirection.Forward: jumpX = 0; jumpZ = jumpDistanceUp; break;
		case FacedDirection.Back: jumpX = 0; jumpZ = -jumpDistanceDown; break;
		}
		
		GetComponent<Rigidbody>().velocity = new Vector3(jumpX,jumpHeight,jumpZ);
		
		return;
	}

	private void FixedUpdate(){

		if (isDone)
			return;

		if (Quaternion.Angle (transform.rotation, toRotation) < 1) {
			facedDirection = calcFacedDirection ();
			Done ();
		}

		transform.rotation = Quaternion.Lerp (transform.rotation, toRotation, Time.deltaTime * speed);

		Vector3 velocity = GetComponent<Rigidbody> ().velocity;
		
		if(velocity.magnitude < minCheckValue&& velocity.magnitude != 0.0f){
			
			transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), transform.position.y , Mathf.RoundToInt(transform.position.z));
			Done();
		}
		


	
	}
	private FacedDirection calcFacedDirection(){

		float angle = transform.rotation.eulerAngles.y;

		if (angle > -10 && angle < 10)
			return FacedDirection.Forward;

		if (angle > 80 && angle < 100)
			return FacedDirection.Right;

		if (angle > 260 && angle < 280)
			return FacedDirection.Left;

		if (angle > 170 && angle < 190)
			return FacedDirection.Back;


		return FacedDirection.Forward;
	}
	private void Done(){
		isDone = true;
		gameObject.SendMessageUpwards ("StartNextAction", SendMessageOptions.RequireReceiver);
	}
}
