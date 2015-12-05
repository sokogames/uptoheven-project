using UnityEngine;
using System.Collections;

public enum Direction {Left,Right,Forward,Backward,Undefined};

public class JumpObject : MonoBehaviour {

	public float jumpHeight = 4.0f;
	public float jumpDistanceOnSide = 1.3f;
	public float jumpDistanceUp = 1.5f;
	public float jumpDistanceDown = 1.2f;

	private static float MIN_CHECK_VALUE = 1.0f; 
	
	private bool done;

	private ObjectVision objectVision;
	private RotateObject rotateObject;
	private AudioSource audioSource;

	void Start () {
		audioSource = GetComponent<AudioSource> ();

		rotateObject = GetComponent<RotateObject> ();
		if (!rotateObject) {
			Debug.LogError("rotate object not assigned");
		}
		objectVision = null;
	}
	
	// Update is called once per frame
	void Update () {

		if (done) {
			return;
		}

		Vector3 velocity = GetComponent<Rigidbody> ().velocity;
		
		GetComponent<Rigidbody> ().velocity = velocity;
		
		if (velocity.magnitude < JumpObject.MIN_CHECK_VALUE) {

			transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), transform.position.y , Mathf.RoundToInt(transform.position.z));
			done = true;
		}
	}
	public void Jump(){

		GetComponent<SqueezeObject> ().unsqueezeObject ();

		if (objectVision == null) {
			objectVision = GetComponent<ObjectVision>();
			if (objectVision == null) {
				Debug.LogError("ObjectVision scritp not assigned");
			}
		}

		if (objectVision.hasBarrier ())
			return;
		if (objectVision.isOnEdge())
			return;

		if (GetComponent<Rigidbody>().velocity.magnitude > 0.02f) return;
		
		float jumpX = 0, jumpZ = 0;
		
		switch (JumpObject.GetTransformDirection(rotateObject.toRotation)) {
			case Direction.Left: jumpX = -jumpDistanceOnSide; jumpZ = 0; 
			break;
			case Direction.Right: jumpX = jumpDistanceOnSide; jumpZ = 0; 
			break;
			case Direction.Forward: jumpX = 0; jumpZ = jumpDistanceUp; 
			break;
			case Direction.Backward: jumpX = 0; jumpZ = -jumpDistanceDown; 
			break;
			default: Debug.Log (Direction.Undefined.ToString() + " is not right direction");
			break;
		}

		GetComponent<Rigidbody>().velocity = new Vector3(jumpX,jumpHeight,jumpZ);
		done = false;

		if (!audioSource.isPlaying) {
			audioSource.pitch = Random.Range(0.6f,1.4f);
			audioSource.Play();
		}

		return;
	}
	public bool Done(){	
		return done;
	}
	public static Direction GetTransformDirection(Quaternion rotation, float angleRange = 10.0f){
	
		float angle = rotation.eulerAngles.y;

		if(angle > - angleRange && angle < angleRange){
			return Direction.Forward;
		}
		if(angle > 270 - angleRange && angle < 270 + angleRange){
			return Direction.Left;
		}
		if(angle > 180 - angleRange && angle < 180 + angleRange){
			return Direction.Backward;
		}
		if(angle > 90 - angleRange && angle < 90 + angleRange){
			return Direction.Right;
		}

		return Direction.Undefined;
	}
}
