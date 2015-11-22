using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public GameObject playerBody;

	public int currentStepPostion = 0;

	private int _currentStepPostion = 0;

	public bool enemyTouched = false;

	private Animator anim;
	private bool landed = true;
	private Rigidbody rigidBody;
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		rigidBody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (rigidBody.velocity != Vector3.zero && landed) {
			Jump();
		}

		if (rigidBody.velocity == Vector3.zero && !landed) {
			Landed();
		}
	}

	private void Jump(){
		landed = false;
		anim.SetTrigger("readyForJump");

	}
	private void Landed(){

		if (landed)
			return;

		landed = true;

		_currentStepPostion = (int)transform.position.z;
		currentStepPostion = Mathf.Max (_currentStepPostion, currentStepPostion);

		anim.SetTrigger("landed");
	}
	void OnCollisionEnter(Collision collision){
		if (collision.collider.gameObject.tag == "DynamicObscale") {
			enemyTouched = true;
			//anim.SetTrigger("dead");
			Dead();
			StopPhysics();
		}
	}
	void StopPhysics(){
		GetComponent<BoxCollider>().enabled = false;
		GetComponent<Rigidbody>().isKinematic = true;
	}
	void Dead(){
		Destroy (transform.FindChild("body").gameObject, 0.01f);
		transform.FindChild ("particle").gameObject.SetActive (true);
	}
}
