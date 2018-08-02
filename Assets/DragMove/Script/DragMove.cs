using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragMove : MonoBehaviour {
	[Header("Setting")]
	public GameObject groundObject;
	protected bool mDidMove = false;
	protected bool mStarted = false;		

	protected Vector3 mLastMovePos;

	protected int groundCollisionLayer;
	
	// Callback
	public delegate void PositionChangeCallback(float x, float y);

	public PositionChangeCallback onPositionChange = null;

	void Start()
	{
		if(groundObject == null) {
			Debug.LogError("DragMove: groundObject is not defined");	
		} else {
			groundCollisionLayer = groundObject.layer;
		}
	}

	
	public void Reset()
	{
		mDidMove = false;
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetMouseButtonDown(0)) {
			mStarted = CheckForStart();
		}else if(Input.GetMouseButtonUp(0)) {
			mStarted = false;
		}

		if(mStarted) {
			if(Input.GetMouseButton(0)) {
				transform.position = PositionAtGround();
			}	
		}
	}

	bool CheckForStart() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		// ken: next 2 lines is used to debug only
		Vector3 startPos = ray.GetPoint(0);
		Debug.DrawRay(startPos, ray.direction * 1000, Color.green, 0.5f);
			
		bool isHit = Physics.Raycast(ray, out hit);
		
		
		return isHit && hit.transform == transform;		// rayCast hit on on the current object
		
	}


	Vector3 PositionAtGround() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		int collisionLayer = 1 << groundCollisionLayer;		// the ground collision mask 

		bool isHit = Physics.Raycast (ray, out hit, Mathf.Infinity, collisionLayer);
		if(isHit == false) {
			return transform.position;		// stay current position if cannot hit the ground 
		}
	
		return hit.point;
	}

}
