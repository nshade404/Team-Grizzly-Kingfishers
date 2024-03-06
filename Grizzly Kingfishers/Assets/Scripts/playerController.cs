using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] CharacterController controller;

    [Header("-----Player Stats-----")]
    [Range(1, 5)][SerializeField] float speed;
    [Range(1, 2)][SerializeField] int jumps;
    [Range(1, 25)][SerializeField] int jumpSpeed;
    [Range(-1, -25)][SerializeField] int gravity;

    [Header("-----Gun Stats-----")]
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] int shootRate;

    int jumpCount;
    Vector3 moveDir;
    Vector3 playerVel;
    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.blue);
        Movement();

        if(Input.GetButton("Shoot") && !isShooting)
        {
            StartCoroutine(shoot());
        }

    }

    void Movement()
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        moveDir = Input.GetAxis("Horizontal") * transform.right
            + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDir * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && jumpCount < jumps)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
        }

        playerVel.y += gravity * Time.deltaTime;

        controller.Move(playerVel * Time.deltaTime);
    }

    IEnumerator shoot()
    {
        isShooting = true;

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
}
