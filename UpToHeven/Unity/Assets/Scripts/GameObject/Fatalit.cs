using UnityEngine;
using System.Collections;

public class Fatalit : MonoBehaviour {

	public MonoBehaviour[] scriptsToDisable; 
	public string tagName;
	public float afterTime;


	private MovingDirection movingDirection;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == tagName) {
			movingDirection = GetComponent<Moving>().currentDirection;
			DisableSripts();
			StopAllCoroutines ();
			Invoke("DoFatalit",afterTime);
		}
	}
	void DisableSripts(){
		foreach(MonoBehaviour monoBehaviour in scriptsToDisable){
			Destroy(monoBehaviour);
		}
	}

	void DoFatalit(){
		Animator anim = GetComponent<Animator> ();
		anim.enabled = true;
		if (movingDirection == MovingDirection.left) {
			anim.SetBool(Animator.StringToHash("isLeft"),true);
		}
	}
}
