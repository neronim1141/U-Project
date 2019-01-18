using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
	[SerializeField]
	private float _speed=5;
	private Vector2 movementInput = Vector2.zero;
	private float forwardVelocity = 0;
	private float sidewaysVelocity = 0;
	[SerializeField]
	private float movementSmoothing = 0.25f;
	[SerializeField]
	private Transform head;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Move();
		Rotate();
	}
	private void Move(){

		movementInput.x = Mathf.SmoothDamp(movementInput.x, Input.GetAxisRaw("Horizontal"), ref forwardVelocity, movementSmoothing);
		movementInput.y = Mathf.SmoothDamp(movementInput.y, Input.GetAxisRaw("Vertical"), ref sidewaysVelocity, movementSmoothing);
		//transform.position += new Vector3(movementInput.x * speed * Time.deltaTime, 0, movementInput.y* speed * Time.deltaTime);
		transform.Translate(new Vector3(movementInput.x, 0,movementInput.y)*_speed*Time.deltaTime);
	}
	private void Rotate(){
		transform.eulerAngles=new Vector3(0,CameraControl.Rotation.y,0);
	}
}
