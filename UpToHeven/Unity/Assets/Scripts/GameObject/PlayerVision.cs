using UnityEngine;
using System.Collections;

public class PlayerVision : MonoBehaviour {

	public string barrierTagName;
	public float visionDistance;
	public Vector3 offset;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Debug.DrawRay(transform.position + offset,fwd);
		Debug.DrawRay(transform.position,fwd);
	}
	public bool hasBarrier(){
	
		Vector3 fwd = transform.TransformDirection(Vector3.forward);

		Ray ray = new Ray(transform.position + offset ,fwd);
		RaycastHit hitInfo;	

		if (Physics.Raycast(ray ,out hitInfo, visionDistance )) {

			if(hitInfo.collider.gameObject.tag == barrierTagName)
				return true;
		}

		ray = new Ray(transform.position + offset ,fwd);
		if (Physics.Raycast(ray ,out hitInfo, visionDistance )) {
			
			if(hitInfo.collider.gameObject.tag == barrierTagName)
				return true;
		}

		return false;
	}
}
