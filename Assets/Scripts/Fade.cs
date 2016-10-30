using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {
    public float distance = 8;

    Transform player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {
	    if (transform.position.x < player.position.x - distance) {
            Destroy(gameObject);
        }
	}
}
