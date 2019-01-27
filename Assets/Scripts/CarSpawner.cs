using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour {

	public float spawnRate = 20.0f;
	public GameObject carPrefab;
	public PathsController pathController;

	void Start () {
		InvokeRepeating ("SpawnCar", 0, spawnRate);
	}

	void SpawnCar() {
		GameObject car = Instantiate (carPrefab, transform.position, carPrefab.transform.rotation) as GameObject;
		car.GetComponent<CarController>().SetPathController (pathController);
		car.GetComponent<CarController> ().StartMovement ();
		//Invoke ("car.GetComponent<CarController> ().StartMovement", 1f);
	}
}
