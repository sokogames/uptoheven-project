using UnityEngine;
using System.Collections;

public class Stairway : MonoBehaviour {

	public GameObject firstChunk;
	public int totalSteps;
	public int chunkSizeInStepts = 10;

	public Vector3 chunkPosIncremental;

	public GameObject[] chunks;



	private GameObject currentChunk;
	// Use this for initialization
	void Start () {
		currentChunk = firstChunk;

		totalSteps = chunkSizeInStepts;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject addRandomChunk(){
		int max = chunks.Length;

		int value = Random.Range (0, max);

		return instantiateChunk (value);

	}
	public GameObject instantiateChunk(int index){

		//Debug.Log (index);

		GameObject chunk = (GameObject)Instantiate (chunks [index], Vector3.zero, Quaternion.identity);

		chunk.transform.parent = this.transform;


		chunk.transform.localPosition = currentChunk.transform.position + chunkPosIncremental;

		currentChunk.GetComponent<Chunk> ().nextChunk = chunk.GetComponent<Chunk> ();

		currentChunk = chunk;

		totalSteps += chunkSizeInStepts;

		return chunk;
	}
}
