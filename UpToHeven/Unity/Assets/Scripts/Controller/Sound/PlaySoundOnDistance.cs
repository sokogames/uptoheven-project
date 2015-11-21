using UnityEngine;
using System.Collections;

public class PlaySoundOnDistance : MonoBehaviour {

	public string targetObjectName = "Player";
	public float checkDelta = 1;

	private Transform targetTransform;
	
	AudioSource audioSource;
	// Use this for initialization
	void Start () {

		targetTransform = GameObject.Find (targetObjectName).transform;
		audioSource = GetComponent<AudioSource> ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void PlaySound(){
		if (audioSource.isPlaying)
			return;

		//float pitch = Random.Range (0.5f, 2.5f);
		float voleme = 1 - Mathf.Min(Vector3.Distance(targetTransform.position,transform.position) / 5,1);

		//audioSource.pitch = pitch;
		audioSource.volume = voleme;

		if (voleme > 0.1f) {
			audioSource.Play ();
		}
	}
	public void Play(){
		if (Mathf.Abs(targetTransform.position.z - transform.position.z) < 1.2f) {
			
			PlaySound();
		}
	}
}
