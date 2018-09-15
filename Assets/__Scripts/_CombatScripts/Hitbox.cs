using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

    private float activeCountdown = 0f; 

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        activeCountdown -= Time.deltaTime;
        if (activeCountdown <= 0){
            gameObject.SetActive(false);
        }
	}

    public void Activate(float activeTime = 0.1f){
        activeCountdown = activeTime;
        gameObject.SetActive(true);
    }
}
