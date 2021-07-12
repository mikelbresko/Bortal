using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PController : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;
    private bool isGrounded;
    public bool enteredTrigger;
    public Text updateText;
    private GameController GameController;
    public GameObject GameMaster;
    public Animator anim;
    public Animator sceneAnim;
    public CapsuleCollider col;
    private int maxTime;
    public int StartTime;
    private bool StopInput;
    int time;
    private Vector3 startPos;
    public GameObject explosion;
    private bool exploding;

    private int levelToLoad;

    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        levelToLoad = 1;
        rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        if (sceneName == "FirstLevel")
        {
            anim.SetBool("getUp", true);
            StartCoroutine(StandUpWait(3.5f));
        }
        else
        {
            StartCoroutine(StandUpWait(.5f));
        }

        anim.SetBool("isRunning", false);
        anim.SetBool("isWalkingB", false);
        anim.SetBool("wantsJump", false);
        anim.SetBool("isJumping", false);
        anim.SetBool("Dead", false);
        isGrounded = true;
        updateText.text = "";
        GameController = GameObject.FindWithTag("GameMaster").GetComponent<GameController>();
        StopInput = true;
        maxTime = 1 + GameController.currentLevel;
        time = StartTime;
        startPos = transform.position;
        exploding = false;

    }

    public void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    public void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * 2, ForceMode.Acceleration);
    }

    void Update()
    {
        if (exploding)
        {
            explosion.transform.position = transform.position;
        }
        if (StopInput == false)
        {
            //---------------------------------
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                if (!anim.GetBool("wantsJump"))
                {
                    anim.SetBool("isRunning", false);
                    anim.SetBool("isWalkingB", false);
                    isGrounded = false;
                    rb.AddForce(new Vector3(0, 150, 0), ForceMode.Impulse);
                    anim.SetBool("wantsJump", true);
                    anim.SetBool("isJumping", true);

                    StartCoroutine(WaitingJump());
                    anim.SetBool("isJumping", false);
                }
            }

            //---------------------------------
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
                anim.SetBool("isRunning", true);
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                anim.SetBool("isRunning", false);
            }
            //---------------------------------
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(-1 * Vector3.forward * Time.deltaTime * speed);
                anim.SetBool("isWalkingB", true);
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                anim.SetBool("isWalkingB", false);
            }
            //---------------------------------
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(0, -2, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(0, 2, 0);
            }
            //---------------------------------
            if (Input.GetKey(KeyCode.R))
            {
                GameController.ResetLevel();
            }
            if (Input.GetMouseButtonDown(1) && (time < maxTime))
            {
                Vector3 currentpos = rb.position;
                rb.transform.position = currentpos + new Vector3(500f, 0f, 0f);
                time++;
            }

            if (Input.GetMouseButtonDown(0) && (time > 0))
            {
                Vector3 currentpos = rb.position;
                rb.transform.position = currentpos - new Vector3(500f, 0f, 0f);
                time--;
            }

            if (Input.GetKey(KeyCode.B))
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
            }
        }
    }

    public int getTime()
    {
        return time;
    }


    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Key"))
        {
            enteredTrigger = true;
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            anim.SetBool("Dead", true);
            StartCoroutine(LoseWait(4));
        }

        if (other.gameObject.CompareTag("Explode"))
        {
            anim.SetBool("Dead", true);
            Instantiate(explosion, transform.position, Quaternion.identity);
            exploding = true;
            StartCoroutine(LoseWait(4));
            
        }

        if (other.gameObject.CompareTag("Win"))
        {
            FadeToLevel(levelToLoad);
            levelToLoad++;
            other.gameObject.SetActive(false);
            StartCoroutine(WinWait(3));

        }
    }

    public void TimeToDie(float i)
    {
        StartCoroutine(LoseWait(i));
    }


    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(1);
        //loseText.text = "";
    }
    IEnumerator WaitingJump()
    {
        yield return new WaitForSeconds(1.2f);
        anim.SetBool("wantsJump", false);
    }

    IEnumerator LoseWait(float sec)
    {

        updateText.text = "YOU LOSE";
        StopInput = true;
        //rb.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(sec);
        //rb.transform.position = startPos; // new Vector3(0f, 0f, 0f);
        //time = StartTime;
        //rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        //updateText.text = "";
        //anim.SetBool("Dead", false);
        //anim.SetBool("isRunning", false);
        //anim.SetBool("isWalkingB", false);
        //anim.SetBool("wantsJump", false);
        //StopInput = false;
        GameController.ResetLevel();

    }
    IEnumerator WinWait(float sec)
    {
        anim.SetBool("toIdle", true);
        updateText.text = "Crystal Acquired";
        StopInput = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(sec);
        StopInput = false;
        //rb.transform.position = new Vector3(0f, 0f, 0f);
        //rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        //updateText.text = "";
        //GameController.ResetLevel();
        anim.SetBool("toIdle", false);
        GameController.NextLevel();
    }

    IEnumerator StandUpWait(float sec)
    {
        yield return new WaitForSeconds(sec);
        StopInput = false;
        anim.SetBool("getUp", false);
    }
    public void FadeToLevel(int levelIndex)
    {
        sceneAnim.SetTrigger("FadeOut");

    }
}