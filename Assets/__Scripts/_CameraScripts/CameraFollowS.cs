using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowS : MonoBehaviour {

    private float _camEasing = 0.5f;

    private Vector3 _currentPos = Vector3.zero;

    public Transform myPosition;
    public Transform currentTarget;
    public Transform targetOffsetObj;

    public float targetOffsetX = -3f;

    public static CameraFollowS F;

    void Awake()
    {
        F = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        UpdatePosition();
    }

    void UpdatePosition(){
        Vector3 camPos = myPosition.position;


        _currentPos = transform.position;

        _currentPos.x = (1 - _camEasing) * _currentPos.x + _camEasing * camPos.x;
        _currentPos.y = (1 - _camEasing) * _currentPos.y + _camEasing * camPos.y;
        _currentPos.z = (1 - _camEasing) * _currentPos.z + _camEasing * camPos.z;


        transform.position = _currentPos;

        targetOffsetObj.position = currentTarget.position + transform.right*targetOffsetX;

        transform.LookAt(targetOffsetObj);
    }
}
