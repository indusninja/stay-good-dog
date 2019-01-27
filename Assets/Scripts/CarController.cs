using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    public float accelleration = 10f;
    public float maxSpeed = 10f;
    public float turnForce = 10f;
    public Transform[] pathPoints;
    public PathsController controller;

    //public RandomSoundPlayer carSounds;

    public Material[] materials;

    public bool loop = false;

    private Rigidbody rb;

    private bool running = false;

    public int nextPathPoint = 0;

    public float collisionForce = 10f;

    public float groundCheckHeight = 2.0f;

    public LayerMask layerMask;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "carNavPoint")
        {
            for (var i = 0; i < pathPoints.Length; i++)
            {
                if (pathPoints[i] == collider.transform)
                {
                    nextPathPoint = i + 1;
                }
            }
            if (collider.transform == pathPoints[pathPoints.Length - 1])
            {
                if (loop)
                {
                    nextPathPoint = 0;
                }
                else
                {
                    running = false;
                    Destroy(gameObject);
                }
            }
        }
    }


    public void StopRunning()
    {
        running = false;
    }

    public void SetPathController(PathsController pc)
    {
        controller = pc;
        /*
		Material randomMat = materials [Random.Range (0, materials.Length)];
		GetComponent<MeshRenderer> ().material = randomMat;
        */

        pathPoints = controller.GetPathPoints();
        rb = GetComponent<Rigidbody>();
        StartMovement();
    }

    // Use this for initialization
    public void SetPathPoints(Transform[] path)
    {
        pathPoints = path;
    }

    public void StartMovement()
    {
        running = true;

        /*
        if (carSounds != null && !carSounds.audioSource.isPlaying)
        {
            carSounds.PlaySound();
        }
        */
    }

    void FixedUpdate()
    {
        if (running)
        {
            CheckIfGrounded();
            // Rotate car towards next point with add torque
            Vector3 targetDelta = pathPoints[nextPathPoint].position - transform.position;
            float angleDiff = Vector3.Angle(transform.forward, targetDelta);
            Vector3 cross = Vector3.Cross(transform.forward, targetDelta);
            rb.AddTorque(cross * angleDiff * turnForce);

            // Add forward force
            rb.AddForce(transform.forward * accelleration);
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            }
        }
    }

    void CheckIfGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, layerMask))
        {
            if (hit.collider != null && hit.distance < groundCheckHeight)
            {
                Disable();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Car"))
        {
            /*
			Disable ();
			collision.gameObject.GetComponent<CarController> ().Disable ();
            */

            GetComponent<Rigidbody>().AddExplosionForce(collisionForce, transform.position, 100f);
            collision.transform.GetComponent<Rigidbody>().AddExplosionForce(collisionForce, transform.position, 100f);
            collision.transform.GetComponent<Rigidbody>().AddExplosionForce(50, collision.contacts[0].point, 10f);
        }
        else if (collision.transform.CompareTag("Player"))
        {
            DogController dog = collision.gameObject.GetComponent<DogController>();
            dog.Kill();
        }
    }

    public void Disable()
    {
        StopRunning();
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        DestroyCar();
    }

    public void DestroyCar()
    {
        StartCoroutine(DestroyAfter());
    }

    public IEnumerator DestroyAfter(float seconds = 5)
    {
        yield return new WaitForSeconds(seconds);
        running = false;
        Destroy(gameObject);
    }
}
