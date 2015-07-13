using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
	public Player player;
	public float lerpSpeed;
	public Vector3 offset;
	// Use this for initialization
	void Start () {
		transform.position = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPos = Vector3.Lerp (gameObject.transform.position, player.transform.position, Time.deltaTime * lerpSpeed);

		newPos = new Vector3(newPos.x , 0 , newPos.z );
		gameObject.transform.position = newPos + offset;

	}
}
