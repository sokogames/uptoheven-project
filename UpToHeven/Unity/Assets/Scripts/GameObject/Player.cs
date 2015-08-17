using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, IActor {

	public Moving movingScript;
	public GameObject playerPref;
	public int currentStepPostion = 0;
	public int _currentStepPostion = 0;

	public bool enemyTouched = false;

	private Animator anim;
	private bool landed = true;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void Jump(){
		//anim.SetTrigger("readyForJump");
		movingScript.Jump ();
		landed = false;
	}
	public void ReadyForJump(MovingDirection toDirection){

		if (toDirection == MovingDirection.forward) {
			_currentStepPostion ++;
		}
		if (toDirection == MovingDirection.back) {
			_currentStepPostion --;
		}

		anim.SetTrigger ("readyForJump");
		if(movingScript.currentDirection == toDirection){
			return;
		}

		while(movingScript.currentDirection != toDirection){
			movingScript.RotateLeft();
		}
	}

	public void Left(){
		while(movingScript.currentDirection != MovingDirection.left){
			movingScript.RotateLeft();
		}
		movingScript.Jump ();
	}
	public void Right(){
		while(movingScript.currentDirection != MovingDirection.right){
			movingScript.RotateRight();
		}
		movingScript.Jump ();
	}
	public void Down(){
		while(movingScript.currentDirection != MovingDirection.back){
			movingScript.RotateRight();
		}
		movingScript.Jump ();
	}
	public void Landed(){

		if (landed)
			return;

		landed = true;
		anim.SetTrigger("landed");

		if (currentStepPostion != _currentStepPostion) {
			currentStepPostion = _currentStepPostion;
		}
	}
	void OnCollisionEnter(Collision collision){
		if (collision.collider.gameObject.tag == "DynamicObscale") {
			enemyTouched = true;
			anim.SetTrigger("dead");
		}
	}
}
