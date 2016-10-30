using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    Rigidbody rb;
    Animator anim;

    public float walkSpeed = 3;
    public float jumpSpeed = 5;

    public float platformHeight { get; private set; }

    int onlyDefault;

    public float calciumJumpSpeed = 6;
    public float calciumWalkSpeed = 4;
    public float calciumTime = 1;

    private float calciumTimer = 0;

    private AudioSource[] sources;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        onlyDefault = 1 << LayerMask.NameToLayer("Default");

        sources = GetComponents<AudioSource>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!Physics.CapsuleCast(transform.position + Vector3.up * .14f, transform.position + Vector3.up * 1.7f, .14f, Vector3.right, .1f, onlyDefault)) {
            float w = calciumTimer > 0 ? calciumWalkSpeed : walkSpeed;
            rb.MovePosition(transform.position + new Vector3(w * Time.fixedDeltaTime, 0, 0));
        }

	}

    bool JumpPressed() {
        if (Input.GetButtonDown("Jump")) {
            return true;
        } else if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) {
            return true;
        } else {
            return false;
        }
    }

    void Update() {
        if (calciumTimer > 0) {
            calciumTimer -= Time.deltaTime;
        }

        if (JumpPressed() && isGrounded()) {
            float j = calciumTimer > 0 ? calciumJumpSpeed : jumpSpeed;

            rb.AddForce(Vector3.up * j, ForceMode.VelocityChange);

            sources[0].Play();
        }

        anim.SetBool("Falling", !isGrounded());

        if (transform.position.y < -40) {
            SceneManager.LoadScene("Lose", LoadSceneMode.Single);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }

    bool isGrounded() {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position + Vector3.up * .5f, .14f, Vector3.down, out hit, .4f)) {
            platformHeight = hit.collider.transform.position.y;
            ScoreDisplay.distance = transform.position.x;
            return true;
        } else {
            return false;
        }
    }

    void OnCollisionEnter(Collision col) {
        if (col.collider.CompareTag("Dog")) {
            SceneManager.LoadScene("Lose", LoadSceneMode.Single);
        } 
    }

    void OnTriggerEnter(Collider col) {
        if (col.CompareTag("Calcium")) {
            calciumTimer += calciumTime;
            Destroy(col.gameObject);
            sources[1].Play();
        }
    }

    void OnGUI() {
        if (transform.position.y < -20) {
            GUI.color = new Color(0, 0, 0, (-transform.position.y - 20) / 10);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        }
    }
}
