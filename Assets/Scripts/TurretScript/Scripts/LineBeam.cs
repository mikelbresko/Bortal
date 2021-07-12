using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class LineBeam : MonoBehaviour
{
    public float maxLength;

    private LineRenderer lineRenderer;

    private float closestPoint;

    private Gradient gradient;

    private float prototypeLength;

    public Vector3 target;

    private int countdown = 0;

    public int timeToLaser;

    public AudioSource beep;

    public AudioSource fire;

    public AudioSource audioSource;

    public float lineDrawSpeed;

    private float counter;

    private bool alreadyPlayedBeep;

    private bool alreadyPlayedShot;

    public Color c1 = Color.yellow;

    public Color c2 = Color.red;

    public Animator anim;

    public virtual void Start()
    {
        this.target = GameObject.FindGameObjectWithTag("PlayerTarget").transform.position;
        this.lineRenderer = this.GetComponent<LineRenderer>();
        this.fire.volume = 0.25f;
        this.beep.volume = 0.25f;
        this.timeToLaser = 2;

        if (!this.lineRenderer)
        {
            Debug.LogWarning(("The 'LineBeam' script on " + this.gameObject.name) + " requires a line renderer! Deactivating.");
            this.enabled = false;
        }
        this.lineRenderer.SetWidth(0.25f,0.25f);
        lineDrawSpeed = 6f;
        this.counter = 0;
        //lineRenderer.numPositions = 2;
        this.lineRenderer.useWorldSpace = true;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 1.0f), new GradientColorKey(c2, 0.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
        //this.lineRenderer.loop = true;
        this.alreadyPlayedBeep = false;
        this.alreadyPlayedShot = false;
    }

    public virtual void Update()
    {
        bool wasObstructed = this.closestPoint < this.maxLength; //	Check for obstructions
        //if (wasObstructed)
        //{
        //    Debug.Log("Laser hit something");
        //}
        this.closestPoint = this.maxLength;
        Debug.DrawLine(this.transform.position, this.target, Color.red);
        Debug.DrawRay(this.transform.position, this.transform.forward, Color.cyan);
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward,out hit, this.maxLength))
        {
            //Debug.Log(hit.collider.tag);
           
            if ((hit.point - this.transform.position).sqrMagnitude < (this.closestPoint * this.closestPoint))
            {
                this.closestPoint = (hit.point - this.transform.position).magnitude;
            }
            if (hit.collider.tag == "Player")
            {
                this.lineRenderer.SetPosition(0, this.transform.position);
                this.lineRenderer.SetPosition(1, hit.collider.transform.position + new Vector3(0, 1, 0));
                //this.lineRenderer.SetPosition(1, hit.collider.transform.position);
                //startCoroutine
                StartCoroutine(LaserBeam(hit.collider.gameObject));
            }
            else
            {
                this.lineRenderer.SetPosition(0, this.transform.position);
                this.lineRenderer.SetPosition(1, this.transform.position);

                //this.lineRenderer.SetPosition(1, hit.collider.transform.position);
                Debug.Log("Reset countdown");
                //reset timeToLaser
                countdown = 0;
            }
        }
        
    }

    IEnumerator LaserBeam(GameObject player)
    {
        //yield return new WaitForSeconds(3);

        if (countdown < timeToLaser)
        {
            if (!alreadyPlayedBeep)
            {
                alreadyPlayedBeep = true;
                beep.Play();
                //beep.volume = 0.5f;
                countdown++;
                Debug.Log(countdown);
                yield return new WaitForSeconds(1);
                
                alreadyPlayedBeep = false;
                
            }
        }
        else
        {
            if (!alreadyPlayedShot)
            {
                this.lineRenderer.SetColors(Color.red, Color.red);
                //lineRenderer.material = new Material(Shader.Find("Assets/Resources/Lava_Flowing_Shader/Materials/mtl_lava_diffuse.mat"));
                alreadyPlayedShot = true;
                fire.Play();
                anim.SetBool("Dead", true);
                player.GetComponent<PController>().TimeToDie(4);
                yield return null;
            }
        }
        yield return null;

    }

    public LineBeam()
    {
        this.maxLength = 100f;
    }

}