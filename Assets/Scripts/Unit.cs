using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	public Vector3[] poi;

	public float speed;

	NavMeshAgent navA;

	// Use this for initialization
	void Start () {
		navA = GetComponent<NavMeshAgent>();
		navA.speed = speed;

		GetComponentsInChildren<MeshRenderer>()[1].material.color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 1);
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(transform.position,navA.pathEndPosition) < 2)
			navA.destination = (poi[0]);
		
	}
}
