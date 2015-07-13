using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, IActor {

	public Moving movingScript;
	public GameObject playerPref;

	private Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void Jump(){
		movingScript.Jump ();
		anim.SetTrigger("jump");
	}
	public void ReadyForJump(MovingDirection toDirection){


		anim.SetTrigger("readyForJump");

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
}
