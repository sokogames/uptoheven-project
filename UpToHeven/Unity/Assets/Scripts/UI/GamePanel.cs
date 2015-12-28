using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour {

	public Player player;
	private Text text;

	// Use this for initialization
	void Start () {
		text = transform.GetComponentInChildren<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = Mathf.Max (1,(player.maxPosition)).ToString();
	}
}
