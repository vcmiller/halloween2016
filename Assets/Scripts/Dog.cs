using UnityEngine;
using System.Collections;

public class Dog : MonoBehaviour {
    public float walkSpeed = 3;
    public float jumpSpeed = 8;

    Rigidbody rb;
    bool canJump = true;
    Transform player;
    Animator anim;

    public bool falling;


    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }

    void FixedUpdate() {
        if (transform.position.x < player.position.x) {
            rb.MovePosition(transform.position + new Vector3(walkSpeed * Time.fixedDeltaTime, 0, 0));
        }

    }

    void Update() {
        if (isGrounded()) {
            if (shouldJump()) {
                if (canJump) {
                    rb.AddForce(Vector3.up * jumpSpeed, ForceMode.VelocityChange);
                    canJump = false;
                }
            } else {
                canJump = true;
            }
        }

        if (!Physics.Raycast(transform.position + Vector3.up * .2f, Vector3.down)) {
            Vector3 v = transform.position;
            v.y = player.position.y;
            transform.position = v;
        }

        falling = !isGrounded();
        anim.SetBool("Falling", falling);
    }

    bool shouldJump() {
        RaycastHit hit;

        if (!Physics.Raycast(transform.position + new Vector3(4f, .2f, 0), Vector3.down)) { // Jump up
            return true;
        } else if (Physics.Raycast(transform.position + new Vector3(4f, .2f, 0), Vector3.up)) { // Split
            return player.transform.position.y > transform.position.y + .2f; // If player is higher, they probably took the upper road
        } else if (Physics.Raycast(transform.position + new Vector3(1f, .2f, 0), Vector3.down, out hit) && hit.collider.CompareTag("Gap")) { // Gap
            return true;
        } else {
            return false;
        }
    }

    bool isGrounded() {
        RaycastHit hit;
        return Physics.SphereCast(transform.position + Vector3.up * .6f, .5f, Vector3.down, out hit, .2f);
    }
}
