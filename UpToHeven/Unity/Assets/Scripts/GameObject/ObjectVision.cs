using UnityEngine;
using System.Collections;

public class ObjectVision : MonoBehaviour {

	public string barrierTagName;
	public string edgeTagName;
	public float visionDistance;
	public Vector3 barrierCheckOffset;
	public Vector3 edgeCheckOffset;

	private Vector3 vDiagonal = new Vector3(0, -1, 1);
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR

		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Debug.DrawRay(transform.position + barrierCheckOffset,fwd);
		Debug.DrawRay(transform.position,fwd);

		Vector3 diagonal = transform.TransformDirection(vDiagonal);
		Debug.DrawRay(transform.position + edgeCheckOffset,diagonal);

		#endif
	}
	public bool hasBarrier(){
	
		Vector3 fwd = transform.TransformDirection(Vector3.forward);

		Ray ray = new Ray(transform.position + barrierCheckOffset ,fwd);
		RaycastHit hitInfo;	

		if (Physics.Raycast(ray ,out hitInfo, visionDistance )) {

			if(hitInfo.collider.gameObject.tag == barrierTagName)
				return true;
		}

		ray = new Ray(transform.position + barrierCheckOffset ,fwd);
		if (Physics.Raycast(ray ,out hitInfo, visionDistance )) {
			
			if(hitInfo.collider.gameObject.tag == barrierTagName)
				return true;
		}

		return false;
	}
	public bool isOnEdge(){

		Vector3 diagoanl = transform.TransformDirection(vDiagonal);
		
		Ray ray = new Ray(transform.position + edgeCheckOffset ,diagoanl);
		RaycastHit hitInfo;	
		
		if (Physics.Raycast(ray ,out hitInfo, visionDistance )) {
			
			if(hitInfo.collider.gameObject.tag == edgeTagName)
				return false;
		}
		
		return true;
	}
}
