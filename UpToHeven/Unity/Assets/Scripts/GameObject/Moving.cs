using UnityEngine;
using System.Collections;

public class Moving : MonoBehaviour {

	public const string LEFT = "left";
	public const string RIGHT = "right";
	public const string FORWARD = "forward";
	public const string BACK = "back";
	
	private string[] directions;
	
	private int _directionID;
	public int directionID {
		get{
			return _directionID; 
		}
		
		set{
			_directionID = value > 3 ? 0 : value < 0 ? 3 : value;	
		}
	}
	// Use this for initialization

	public float jumpHeight = 4.0f;
	public float jumpDistanceOnSide = 1.7f;
	public float jumpDistanceUp = 2.0f;
	public float jumpDistanceDown = 1.5f;

	public ObjectVision playerVision;
	
	private Quaternion toRotation;
	
	void Start () {
		directionID = 0;
		directions = new string[4]{FORWARD, RIGHT, BACK, LEFT};
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void FixedUpdate(){
		
		correctPosition ();

	}

	public void RotateLeft(){
		transform.Rotate(new Vector3(0,-90,0));
		directionID--;
	}
	public void RotateRight(){
		transform.Rotate(new Vector3(0,90,0));
		directionID++;
	}
	public void Reverse(){
		transform.Rotate(new Vector3(0,180,0));
		directionID++;
		directionID++;
	}

	public bool Jump(){
		
		if (playerVision.hasBarrier ())
			return false;
		
		if (GetComponent<Rigidbody>().velocity.magnitude > 0.01f) return false;

		float jumpX = 0, jumpZ = 0;
		
		switch (directions [directionID]) {
		case LEFT: jumpX = -jumpDistanceOnSide; jumpZ = 0; break;
		case RIGHT: jumpX = jumpDistanceOnSide; jumpZ = 0; break;
		case FORWARD: jumpX = 0; jumpZ = jumpDistanceUp; break;
		case BACK: jumpX = 0; jumpZ = -jumpDistanceDown; break;
		}
		
		GetComponent<Rigidbody>().velocity = new Vector3(jumpX,jumpHeight,jumpZ);

		return true;
	}

	void correctPosition(){
		
		Vector3 velocity = GetComponent<Rigidbody> ().velocity;
		
		GetComponent<Rigidbody> ().velocity = velocity;
		
		if (velocity.magnitude < 0.1f) {
			transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), transform.position.y , Mathf.RoundToInt(transform.position.z));
		}
	}
}
