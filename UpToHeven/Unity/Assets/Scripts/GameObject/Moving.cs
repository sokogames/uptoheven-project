using UnityEngine;
using System.Collections;

public enum MovingDirection
{
	left,right,forward,back
};

public class Moving : MonoBehaviour {


	private MovingDirection[] directions = {MovingDirection.forward, MovingDirection.right, MovingDirection.back, MovingDirection.left};
	
	private int _directionID;
	public int directionID {
		get{
			return _directionID; 
		}
		
		set{
			_directionID = value > 3 ? 0 : value < 0 ? 3 : value;	

			switch(directions[_directionID]){
				case MovingDirection.forward: transform.rotation = Quaternion.Euler(0, 0, 0); break;
				case MovingDirection.right: transform.rotation = Quaternion.Euler(0, 90, 0); break;
				case MovingDirection.left: transform.rotation = Quaternion.Euler(0, -90, 0); break;
				case MovingDirection.back: transform.rotation = Quaternion.Euler(0, 180, 0); break;
			}
		}
	}
	public MovingDirection currentDirection{
		get{
			return directions[directionID];
		}
	}
	// Use this for initialization

	public float jumpHeight = 4.0f;
	public float jumpDistanceOnSide = 1.7f;
	public float jumpDistanceUp = 2.0f;
	public float jumpDistanceDown = 1.5f;

	public ObjectVision playerVision;
	
	private Quaternion toRotation;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void FixedUpdate(){
		
		correctPosition ();

	}

	public void RotateLeft(){
		directionID--;
	}
	public void RotateRight(){
		directionID++;
	}
	public void Reverse(){
		directionID++;
		directionID++;
	}

	public bool Jump(){

		if (playerVision.hasBarrier ())
			return false;
		
		if (GetComponent<Rigidbody>().velocity.magnitude > 0.02f) return false;

		float jumpX = 0, jumpZ = 0;
		
		switch (directions [directionID]) {
		case MovingDirection.left: jumpX = -jumpDistanceOnSide; jumpZ = 0; break;
		case MovingDirection.right: jumpX = jumpDistanceOnSide; jumpZ = 0; break;
		case MovingDirection.forward: jumpX = 0; jumpZ = jumpDistanceUp; break;
		case MovingDirection.back: jumpX = 0; jumpZ = -jumpDistanceDown; break;
		}
		
		GetComponent<Rigidbody>().velocity = new Vector3(jumpX,jumpHeight,jumpZ);

		return true;
	}

	void correctPosition(){
		
		Vector3 velocity = GetComponent<Rigidbody> ().velocity;
		
		GetComponent<Rigidbody> ().velocity = velocity;
		
		if (velocity.magnitude < 0.1f && velocity.magnitude != 0.0f) {

			transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), transform.position.y , Mathf.RoundToInt(transform.position.z));
		}
	}
	public void ToDirection(MovingDirection toDirection){
		while (currentDirection != toDirection) {
			RotateLeft();
		}
	}
}
