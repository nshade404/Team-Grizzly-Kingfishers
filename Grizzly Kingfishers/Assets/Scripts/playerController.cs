using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class playerController : MonoBehaviour, IDamage {
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;

    [Header("----- Player Stats -----")]
    [Range(0, 10)][SerializeField] int health;
    [Range(1, 5)][SerializeField] float speed;
    [Range(1, 3)][SerializeField] int jumps;
    [Range(5, 25)][SerializeField] int jumpSpeed;
    [Range(-15, -35)][SerializeField] int gravity;

    [Header("----- Gun Stats -----")]
    [Range(0, 5)][SerializeField] int shootDamage;
    [Range(0, 100)][SerializeField] int shootDist;
    [Range(0, 1)][SerializeField] float shootRate;

    int jumpCount;
    Vector3 moveDir;
    Vector3 playerVel;
    bool isShooting;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (!gameManager.instance.isPaused) {
#if UNITY_EDITOR 
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.blue);
#endif
            Movement();

            if (Input.GetButton("Shoot") && !isShooting)
            {
                StartCoroutine(Shoot());
            }
        }
    }

    void Movement() {
        if (controller.isGrounded) {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        // 1st person camera controls
        moveDir = Input.GetAxis("Horizontal") * transform.right
                + Input.GetAxis("Vertical") * transform.forward;

        // Topdown camera controls
        //moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        float locSpeed = speed;
        if (Input.GetButton("Sprint"))
            locSpeed *= 2;

        controller.Move(moveDir * locSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && jumpCount < jumps) {
            playerVel.y = jumpSpeed;
            jumpCount++;
        }

        // Gravity
        playerVel.y += gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);
    }

    IEnumerator Shoot() {
        isShooting = true;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist)) {
            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (hit.transform != transform && dmg != null) {
                Debug.Log("Are we getting into the hit.takedamage");
                dmg.takeDamage(shootDamage);
            }
        }
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void takeDamage(int amount) {
        health -= amount;
    }
}
