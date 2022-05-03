using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class animationStateController : MonoBehaviour
{

    Animator animator;
    int isWalkingHash;
    int isAttackingHash;
    int isDefendingHash;

    public Rigidbody rb;
    public bool cubeIsOnTheGround = true;

    public float health;
    public float maxHealth;
    public Slider healthBar;

    public GameObject sword;

    // Start is called before the first frame update
    void Start()
    {
        sword.GetComponent<Collider>().enabled = false;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isAttackingHash = Animator.StringToHash("isAttacking");
        isDefendingHash = Animator.StringToHash("isDefending");
        healthBar.value = health;
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isAttacking = animator.GetBool(isAttackingHash);
        bool isDefending = animator.GetBool(isDefendingHash);
        bool leftPressed = Input.GetKey("a");
        bool rightPressed = Input.GetKey("d");
        bool attackPressed = Input.GetKey("j");
        bool defendPressed = Input.GetKey("s");

        if ((rb != null) && rb.IsSleeping())
        {
            rb.WakeUp();
        }

        if (!isWalking && leftPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }
        if (!leftPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if (!isWalking && rightPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }
        if (!rightPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if (VirtualInputManager.Instance.MoveRight)
        {
            this.gameObject.transform.Translate(Vector3.forward * 5f * Time.deltaTime);
            this.gameObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }

        if (VirtualInputManager.Instance.MoveLeft)
        {
            this.gameObject.transform.Translate(Vector3.forward * 5f * Time.deltaTime);
            this.gameObject.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
        }

        if (Input.GetButtonDown("Jump") && cubeIsOnTheGround)
        {
            rb.AddForce(new Vector3(0, 8, 0), ForceMode.Impulse);
            cubeIsOnTheGround = false;
        }

        // attack if statements
        if (!isAttacking && attackPressed)
        {
            animator.SetBool(isAttackingHash, true);
        }
        if (isAttacking && !attackPressed)
        {
            animator.SetBool(isAttackingHash, false);
        }

        if (!isDefending && defendPressed)
        {
            animator.SetBool(isDefendingHash, true);
        }
        if (isDefending && !defendPressed)
        {
            animator.SetBool(isDefendingHash, false);
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        } 

    }

    public void swordActivated()
    {
        sword.GetComponent<Collider>().enabled = true;
    }

    public void swordDeactivated()
    {
        sword.GetComponent<Collider>().enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        

        if (collision.gameObject.name == "wall2" || collision.gameObject.name == "Platform")
        {
            cubeIsOnTheGround = true;
        }

    }

    private void OnTriggerEnter(Collider trigger)
    {
        bool defendPressed = Input.GetKey("s");
        if (defendPressed)
        {
            Debug.Log("Defend Triggered");
        }
        if (!defendPressed)
        {
            if (trigger.gameObject.tag == "GolemFist" && (trigger.GetType() != typeof(BoxCollider)))
            {
                Debug.Log("Got Hit" + trigger);
                health -= 10;
                healthBar.value = changingHealthBar();
                Debug.Log(healthBar.value + " : " + health);
            }
            Debug.Log("Defend Not Triggered");
        }

    }

    float changingHealthBar()
    {
        return (health / 100);
    }
    

}
