// FPS Controller
// 1. Create a Parent Object like a 3D model
// 2. Make the Camera the user is going to use as a child and move it to the height you wish. 
// 3. Attach a Rigidbody to the parent
// 4. Drag the Camera into the m_Camera public variable slot in the inspector
// Escape Key: Escapes the mouse lock
// Mouse click after pressing escape will lock the mouse again


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DogController : MonoBehaviour
{
    private float speed = 50.0f;
    private float stoppingForce = -80.0f;
    private float maxVelocity = 6.0f;
    private float m_MovX;
    private float m_MovY;
    private Vector3 m_moveHorizontal;
    private Vector3 m_movVertical;
    private Vector3 m_velocity;
    private Rigidbody m_Rigid;
    private float m_yRot;
    private float m_xRot;
    private Vector3 m_rotation;
    private Vector3 m_cameraRotation;
    private float m_lookSensitivity = 3.0f;
    private bool m_cursorIsLocked = true;
    private Vector3 previousRotation = Vector3.zero;

    private SoundManager SoundManagerInstance;

    [Header("The Camera the player looks through")]
    public Camera m_Camera;

    // Use this for initialization
    private void Start()
    {
        m_Rigid = GetComponent<Rigidbody>();
        if (SoundManagerInstance == null)
        {
            SoundManagerInstance = GetComponentInChildren<SoundManager>();
        }
    }

    // Update is called once per frame
    public void Update()
    {
        m_MovX = Input.GetAxisRaw("Horizontal");
        m_MovY = Input.GetAxisRaw("Vertical");
        //Debug.Log(m_MovX);

        m_moveHorizontal = transform.right * m_MovX;
        m_movVertical = transform.forward * m_MovY;

        m_velocity = (m_moveHorizontal + m_movVertical).normalized * speed;

        //mouse movement 
        m_yRot = Input.GetAxisRaw("Mouse X");
        m_rotation = new Vector3(0, m_yRot, 0) * m_lookSensitivity;

        m_xRot = Input.GetAxisRaw("Mouse Y");
        m_cameraRotation = new Vector3(m_xRot, 0, 0) * m_lookSensitivity;

        //apply camera rotation
        
        //move the actual player here
        if (m_velocity != Vector3.zero)
        {
            //m_Rigid.MovePosition(m_Rigid.position + m_velocity * Time.fixedDeltaTime);
            m_Rigid.AddForce(m_velocity, ForceMode.Acceleration);
        }
        else if (m_Rigid.velocity.magnitude > 1f)
        {
            Vector3 stoppingForceVector = m_Rigid.velocity.normalized * stoppingForce;
            stoppingForceVector.y = 0;
            m_Rigid.AddForce(stoppingForceVector);
        }
        else
        {
            m_Rigid.velocity = new Vector3(0f, m_Rigid.velocity.y, 0f);
        }

        if (m_rotation != Vector3.zero)
        {
            //rotate the camera of the player
            m_Rigid.MoveRotation(m_Rigid.rotation * Quaternion.Euler(m_rotation));
        }

        if (m_Camera != null)
        {
            //negate this value so it rotates like a FPS not like a plane
            m_Camera.transform.Rotate(-m_cameraRotation);
        }

        if (m_Rigid.velocity.magnitude > maxVelocity)
        {
            m_Rigid.velocity = m_Rigid.velocity.normalized * maxVelocity;
        }

        SoundManagerInstance.SetCurrentWalkingSurface(
            BroadcastMovementState(previousRotation - m_Rigid.transform.rotation.eulerAngles, m_velocity));

        previousRotation = m_Rigid.transform.rotation.eulerAngles;

        InternalLockUpdate();
    }

    private WalkingSurfaceTypes BroadcastMovementState(Vector3 Rotation, Vector3 Velocity)
    {
        float traceDistance = 10;

        // if player moved or rotated, tell the surface to the sound manager
        // otherwise, tell the sound manager that the player stopped
        if (SoundManagerInstance != null)
        {
            if (/*Rotation.magnitude > 0.01f ||*/
                Velocity.magnitude > 0.01f)
            {
                RaycastHit hit;
                Material m_Material = null;
                bool IsIndoors = false;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, traceDistance))
                {
                    m_Material = hit.collider.gameObject.GetComponent<Renderer>().material;
                    if (m_Material != null &&
                        m_Material.mainTexture != null &&
                        m_Material.mainTexture.name.ToLower().Contains("apartment"))
                    {
                        IsIndoors = true;
                    }
                }
                if (IsIndoors)
                {
                    return WalkingSurfaceTypes.InteriorFloor;
                }
                else
                {
                    return WalkingSurfaceTypes.Grass;
                }
            }
        }

        return WalkingSurfaceTypes.None;
    }

    //controls the locking and unlocking of the mouse
    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_cursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_cursorIsLocked = true;
        }

        if (m_cursorIsLocked)
        {
            UnlockCursor();
        }
        else if (!m_cursorIsLocked)
        {
            LockCursor();
        }
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}