using UnityEngine;
using System.Collections;

public class PlayerList : MonoBehaviour {

	public GameObject[] playerList;
	public bool[] isPlayerActive;
	public Vector2 scrollerRectBoxSize;
	public Vector3 playersLookAngle;
	public float size;
	public float bigSize;
	public float moveSpeed;
	public float rotationSpeed;
	public float zoomSpeed;
	public Color disableColor;

	private Vector3 startPosition;
	private int _index = 0;
	private Vector3 toPosition;
	private Vector3 toSize;
	private GameController gameController;

	private Transform currentTransform{
		get{
			
			return scroller.Find ("player" + index).transform;
		}
	}

	public int index{
		get{
			return _index;
		}

		set{
			
			_index = Mathf.Clamp (value, 0, playerList.Length - 1);
			toPosition = startPosition - Vector3.right * _index * scrollerRectBoxSize.x;
			currentTransform.gameObject.GetComponent<Renderer> ().material.color 
			= isPlayerActive [_index] ? Color.white : disableColor;
		}
	}

	private Transform scroller;
	// Use this for initialization
	void Start () {

		scroller = transform.Find ("Scroller");
	
		if (scroller == null) {
			Debug.Log ("Object with name not found");
			return;
		}

		startPosition = scroller.position;

		InitScrollerRect ();
		UpdateIsActive ();

		toSize = Vector3.one * bigSize;

		index = 0;

		gameController = GameObject.Find ("_main").GetComponent<GameController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(GameController.GameState == GameController.GAME_STATE_CHOOSE_PLAYER){
			if(SystemInfo.deviceType == DeviceType.Desktop){
				//we are on a desktop device, so don't use touch
				if (Input.GetKeyDown (KeyCode.LeftArrow)) {
					MoveLeft ();
				}	
				if (Input.GetKeyDown (KeyCode.RightArrow)) {
					MoveRight ();
				}
				if (Input.GetKeyDown (KeyCode.Return)) {
					ChoosePlayer ();
				}
			}
			//if it isn't a desktop, lets see if our device is a handheld device aka a mobile device
			else if(SystemInfo.deviceType == DeviceType.Handheld){
				//we are on a mobile device, so lets use touch input
				MobileInput();
			}
		}

		if (Vector3.Distance (scroller.position, toPosition) > 0.05f) {
			scroller.position = Vector3.Lerp (scroller.position, toPosition, Time.deltaTime * moveSpeed);
		}

		if (isPlayerActive [index]) {
			currentTransform.Rotate (0, rotationSpeed, 0);
		}

		if(Vector3.Distance(currentTransform.localScale,toSize) > 0.05f){
			currentTransform.localScale = Vector3.Lerp (currentTransform.localScale, toSize, Time.deltaTime * zoomSpeed);
		}
	}

	void InitScrollerRect(){
		for (int i = 0; i < playerList.Length; i++) {
			Vector3 position = new Vector3 ((i + 0.5f) * scrollerRectBoxSize.x, scrollerRectBoxSize.y * 0.5f,0);
			GameObject obj = (GameObject)Instantiate (playerList [i],	position, Quaternion.identity);
			obj.transform.parent = scroller;
			obj.name = "player" + i;
			obj.layer = scroller.gameObject.layer;
			ApplyStartingTransform (obj.transform);
		}
	}

	void UpdateIsActive(){
		for(int i = 0; i < isPlayerActive.Length; i++){
			scroller.Find ("player" + i).GetComponent<Renderer> ().material.color 
			= isPlayerActive [i] ? Color.gray :	disableColor;				
		}
	}

	public void MoveLeft(){
		ApplyStartingTransform (currentTransform);
		index++;
	}

	public void MoveRight(){
		ApplyStartingTransform (currentTransform);
		index--;
	}
		
	void ApplyStartingTransform(Transform tr){
		tr.localScale = Vector3.one * size;
		tr.rotation = Quaternion.Euler (playersLookAngle);
		tr.gameObject.GetComponent<Renderer> ().material.color 
		= isPlayerActive [index] ? Color.gray :	disableColor;	
	}
	void ChoosePlayer(){
		if (isPlayerActive [index]) {
			gameController.ChoosePlayer (GetCurrentPrefab());
		}
	}

	void MobileInput(){

		if (Input.touchCount == 0)
			return;
		
		Touch touch = Input.GetTouch (0);

		if (touch.phase == TouchPhase.Ended) {
			if (touch.tapCount > 0) {
				ChoosePlayer ();
			}
		}

		if (touch.phase == TouchPhase.Moved)
		{
			if (Vector3.Distance (scroller.position, toPosition) > 0.05f)
				return;

			Vector2 delta = touch.deltaPosition;

			if(delta.magnitude < 10.0f) return;

			if(Mathf.Abs (delta.x) > Mathf.Abs (delta.y) * 0.5f){

				if(delta.x < 0){
					MoveLeft ();
				}

				if(delta.x > 0){
					MoveRight ();
				}

			}		
		}
	}
	public GameObject GetCurrentPrefab(){
		return playerList [index];
	}
	public void Show(bool show){
		foreach(Transform tr in transform){
			tr.gameObject.SetActive (show);
		}
	}
}
