using UnityEngine;
using System.Collections;

public delegate void JumpSlerpEnded(JumperDirection direction);

[RequireComponent (typeof (Rigidbody))]
public class JumpSlerpObject : MonoBehaviour {

	public event JumpSlerpEnded OnJumpSlerpEnded;

	public float distance = 1;
	public float jumpTime = 0.25f;
	public float stepHeight = 0.2f;
	// Use this for initialization
	private Rigidbody rigidBody;
	private Vector3 toPostion;
	private Vector3 fromPosition;
	private ObjectVision objectVision;
	private JumperDirection currentDirection;

	private float startTime;
	private bool jumping = false;

	void Start () {
		rigidBody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (!jumping)
			return;

		if (Vector3.Distance (transform.position, toPostion) < 0.1f) {
		
			transform.position = toPostion;
			rigidBody.isKinematic = false;
			jumping = false;	
			if(OnJumpSlerpEnded != null){
				OnJumpSlerpEnded(currentDirection);
			}
			return;
		}

		Vector3 center = (transform.position + toPostion) * 0.5F;
		center -= new Vector3(0, 1.2f, 0);
		Vector3 riseRelCenter = fromPosition - center;
		Vector3 setRelCenter = toPostion - center;
		float fracComplete = (Time.time - startTime) / jumpTime;
		transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
		transform.position += center;

	}

	public void Jump(JumperDirection direction){

		currentDirection = direction;

		if (jumping) {
			Debug.Log ("still in the air!!!");
			return;
		}

		rigidBody.isKinematic = true;
		rigidBody.detectCollisions = true;

		switch(currentDirection){
		case JumperDirection.left:
			toPostion = transform.position + new Vector3(-distance,0,0);
			break;
		case JumperDirection.right: 
			toPostion = transform.position + new Vector3(distance,0,0);
			break;
		case JumperDirection.backward: 
			toPostion = transform.position + new Vector3(0,-stepHeight,-distance);
			break;
		case JumperDirection.forward: 
			toPostion = transform.position + new Vector3(0,stepHeight,distance);	
			break;
		}

		fromPosition = transform.position;
		startTime = Time.time;
		jumping = true;

	}

	public bool Done(){	
		return !jumping;
	}
}
