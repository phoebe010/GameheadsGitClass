using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //use gameObject to reference a prefab 
    public GameObject bullet;
    public Transform bulletSpawnPoint;

    public Vector3 speed;
    public float turnSpeed;
    public float jumpforce = 1.0f;

    private bool isJumping = false;
    private float currentSpeed = 0.0f;
    private float distanceToGround = 0.0f;
    private int jumpCount = 0;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        distanceToGround = GetComponent<Collider>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        float currentTurnAmount = 0.0f;
        currentSpeed = 0.0f;

        if (Input.GetKey(KeyCode.A)){
            currentTurnAmount -= turnSpeed;
		}
        if (Input.GetKey(KeyCode.D)){
            currentTurnAmount += turnSpeed;
		}
        if (Input.GetKey(KeyCode.W)){
            currentSpeed = speed.x ;
		}
        if (Input.GetKey(KeyCode.S)){
            currentSpeed = -speed.x ;
		}

        if (Input.GetKey(KeyCode.F))
        {
            GameObject newBullet = GameObject.Instantiate(bullet, bulletSpawnPoint.position, new Quaternion());
		    Rigidbody bulletBody = newBullet.GetComponent<Rigidbody>();
            bulletBody.AddForce(transform.forward * 30, ForceMode.Impulse);
        }

        //Speed is a Vector3 defines how fast to move in 3D space
        gameObject.transform.Rotate(Vector3.up, currentTurnAmount * Time.deltaTime);

    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distanceToGround + 0.1f);

	}

    //comands for physics system sould be inside fixedupdate
    void FixedUpdate()
    {
        rb.AddForce(transform.forward * currentSpeed * Time.deltaTime, ForceMode.Impulse);

        bool isGrounded = IsGrounded();

        if(isGrounded)
        {
            jumpCount = 0;  
		}


        if (Input.GetKeyUp(KeyCode.Space) && (!isGrounded || jumpCount < 2))
        {
            rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse); 
            jumpCount = jumpCount + 1;
		}

        rb.angularVelocity = Vector3.zero;
    
	}

}
