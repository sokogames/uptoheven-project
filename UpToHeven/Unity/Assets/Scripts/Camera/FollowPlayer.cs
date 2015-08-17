using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
	public Player player;
	public float lerpSpeed;
	public Vector3 offset;

	private Vector3 point;
	// Use this for initialization
	void Start () {
		transform.position = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		point = player.transform.position + offset;

		Vector3 newPos = Vector3.Lerp (gameObject.transform.position
		                               ,point
		                               , Time.deltaTime * lerpSpeed);

		transform.position = newPos;

	}
}
