using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathsController : MonoBehaviour {

	public Transform[] GetPathPoints () {
		Transform[] points = new Transform[transform.childCount];
		for (var i = 0; i < transform.childCount; i++) {
			points [i] = transform.GetChild (i);
		}
		return points;
	}
}
