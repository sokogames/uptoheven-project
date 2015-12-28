using UnityEngine;
using System.Collections;

public delegate void JumperEnded();

public enum JumperDirection {left, right , forward , backward};

public interface IJumper{

	void Jump (JumperDirection direction);

	void Squat();

	void UnSquat();

	void QuickJump (JumperDirection direction);

	void Rotate (JumperDirection direction);
	
};

[RequireComponent(typeof(JumpSlerpObject))]
[RequireComponent(typeof(RotateObject))]
[RequireComponent(typeof(ObjectVision))]
public class Player : MonoBehaviour, IJumper {

	public event JumperEnded OnJumperEnded;

	public GameObject playerBody;

	public int currentStepPostion = 0;
	public int maxPosition = 1;

	public float quickUnsquatTime = 0.1f;

	public bool enemyTouched = false;

	private Rigidbody rigidBody;
	private AudioSource audioSource;
	
	private JumpSlerpObject jumpSlerpObject;
	private RotateObject rotateObject;
	private ObjectVision objectVision;

	public AudioClip explosionClip;

	private Animator animator;
	// Use this for initialization
	void Start () {

		rigidBody = GetComponent<Rigidbody> ();
		audioSource = GetComponent<AudioSource> ();
		jumpSlerpObject = GetComponent<JumpSlerpObject> ();
		rotateObject = GetComponent<RotateObject> ();
		objectVision = GetComponent<ObjectVision> ();

		jumpSlerpObject.OnJumpSlerpEnded += OnJumpEnded;
		rotateObject.OnRotationEnded += OnRotateEnded;
		rotateObject.OnRotationStarted += OnRotateStarted;

		animator = GetComponent<Animator> ();
	}

	private void OnJumpEnded(JumperDirection direction){
		if (rotateObject.Done()) {
			OnJumperEnded();
		}

		if (direction == JumperDirection.forward) {
			currentStepPostion ++;
			maxPosition++;
		}
		if (direction == JumperDirection.backward) {
			currentStepPostion --;
		}
	}
	private void OnRotateEnded(JumperDirection direction){
		if (jumpSlerpObject.Done()) {
			OnJumperEnded();
		}
	}
	private void OnRotateStarted(JumperDirection direction){
		
	}
	// Update is called once per frame
	void Update () {

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
		GetComponent<Rigidbody> ().velocity = Vector3.zero;
		GetComponent<Rigidbody> ().useGravity = false;
	}
	void Dead(){

		if (audioSource.isPlaying) {
			audioSource.Stop();
		}
		audioSource.clip = explosionClip;
		audioSource.Play ();

		Destroy (transform.FindChild("body").gameObject, 0.1f);
		transform.FindChild ("particle").gameObject.SetActive (true);
	}

	public void Jump (JumperDirection direction){

		rotateObject.RotateTo (direction);

		internalJump (direction);

		UnSquat ();

	}

	public void Squat(){
		animator.SetBool ("Squeeze",true);
	}

	public void UnSquat(){
		animator.SetBool ("Squeeze",false);
	}

	public void QuickJump (JumperDirection direction){
		Squat ();
		rotateObject.RotateTo (direction);
		internalJump (direction);
		Invoke ("UnSquat",quickUnsquatTime);
	}
	public void Rotate(JumperDirection direction){

		if (!jumpSlerpObject.Done())
			return;

		rotateObject.RotateTo (direction);
	}
	private void internalJump(JumperDirection direction){
		if(!objectVision.hasBarrier(direction) && !objectVision.isOnEdge(direction)){
			
			jumpSlerpObject.Jump (direction);
		}
	}
}
