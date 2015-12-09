using UnityEngine;
using System.Collections;


public enum SlerpDirection {left,right,forward,backward};

[RequireComponent (typeof (Rigidbody))]
public class JumpSlerpObject : MonoBehaviour {

	public float distance = 3;
	public float jumpTime = 1;
	public float stepHeight = 0.2f;
	// Use this for initialization
	private Rigidbody rigidBody;
	private Vector3 toPostion;
	private Vector3 fromPosition;
	private ObjectVision objectVision;

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
			return;
		}

		Vector3 center = (transform.position + toPostion) * 0.5F;
		center -= new Vector3(0, 1, 0);
		Vector3 riseRelCenter = fromPosition - center;
		Vector3 setRelCenter = toPostion - center;
		float fracComplete = (Time.time - startTime) / jumpTime;
		transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
		transform.position += center;

	}

	public void Jump(SlerpDirection playerFacedTo){

		if (jumping)
			return;

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

		rigidBody.isKinematic = true;
		rigidBody.detectCollisions = true;

		switch(playerFacedTo){
		case SlerpDirection.left:
			toPostion = transform.position + new Vector3(-distance,0,0);
			break;
		case SlerpDirection.right: 
			toPostion = transform.position + new Vector3(distance,0,0);
			break;
		case SlerpDirection.backward: 
			toPostion = transform.position + new Vector3(0,-stepHeight,-distance);
			break;
		case SlerpDirection.forward: 
			toPostion = transform.position + new Vector3(0,stepHeight,distance);	
			break;
		}

		fromPosition = transform.position;
		startTime = Time.time;
		jumping = true;

	}
	public void Jump(){
		SlerpDirection direction = SlerpDirection.forward;
		float angleRange = 5;
		float angle = transform.rotation.eulerAngles.y;
		
		if(angle > - angleRange && angle < angleRange){
			direction = SlerpDirection.forward;
		}
		if(angle > 270 - angleRange && angle < 270 + angleRange){
			direction = SlerpDirection.left;
		}
		if(angle > 180 - angleRange && angle < 180 + angleRange){
			direction = SlerpDirection.backward;
		}
		if(angle > 90 - angleRange && angle < 90 + angleRange){
			direction = SlerpDirection.right;
		}

		Jump (direction);
	}
	public bool Done(){	
		return !jumping;
	}
}
