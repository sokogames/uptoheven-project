using UnityEngine;
using System.Collections;

public class ObjectVision : MonoBehaviour {

	public string[] barrierTagName;
	public float visionDistance;
	public Vector3 barrierCheckOffset;
	public Vector3 edgeCheckOffset;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR

		#endif
	}
	public bool hasBarrier(JumperDirection direction){
	
		Ray ray = GetObstacleRay (direction);
		RaycastHit hitInfo;	

		if (Physics.Raycast(ray ,out hitInfo, visionDistance )) {

			if(isInTags(hitInfo.collider.gameObject.tag)){
				return true;
			}
		}

		ray = new Ray(transform.position + barrierCheckOffset ,ray.direction);
		if (Physics.Raycast(ray ,out hitInfo, visionDistance )) {
			
			if(isInTags(hitInfo.collider.gameObject.tag)){
				return true;
			}
		}

		return false;
	}

	private Vector3 GetVectorFromDirection(JumperDirection direction){
	
		Vector3 vector = Vector3.forward;

		switch(direction){
		
		case JumperDirection.left: 
			vector = Vector3.left;
			break;
		case JumperDirection.right: 
			vector = Vector3.right;
			break;
		case JumperDirection.forward: 
			vector = Vector3.forward;
			break;
		case JumperDirection.backward: 
			vector = Vector3.back;
			break;

		}

		return vector;//transform.TransformDirection(vector);
	}

	public bool hasBarrier(){

		return hasBarrier (JumperDirection.forward);
		
	}
	public bool isOnEdge(JumperDirection direction = JumperDirection.forward){

		Ray ray = GetOnEndgeRay (direction);
		RaycastHit hitInfo;	
		
		if (Physics.Raycast(ray ,out hitInfo, visionDistance )) {
			
			if(hitInfo.collider.gameObject != null)
				return false;
		}

		return true;
	}
	bool isInTags(string tagName){
	
		foreach (string tag in barrierTagName) {
			if(tag == tagName) return true;
		}

		return false;
	}
	private Ray GetOnEndgeRay(JumperDirection direction){
		Vector3 diagoanl = GetVectorFromDirection (direction) + Vector3.down;
		return new Ray(transform.position + edgeCheckOffset ,diagoanl);
	}
	private Ray GetObstacleRay(JumperDirection direction){
		Vector3 dir = GetVectorFromDirection(direction);
		return new Ray(transform.position + barrierCheckOffset ,dir);
	}
}
