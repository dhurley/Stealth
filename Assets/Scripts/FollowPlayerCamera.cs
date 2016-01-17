using UnityEngine;
using System.Collections;

public class FollowPlayerCamera : MonoBehaviour {

    public GameObject player;
    public float distance = 8f;

	void Start () {
	}
	
	void Update () {
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, player.transform.position.z - distance);
	}
}
