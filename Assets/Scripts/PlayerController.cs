using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
public class PlayerController : MonoBehaviour
{

    // Use this for initialization
    public NavMeshAgent nAgent;
    public Vector3 _target;
    RaycastHit hit;
	RaycastHit hitGround;
    private CardboardHead head;
    public LayerMask rayInteractionMask;
    public Image uiStateImage;
    public Sprite[] uiStateSprites;
//    public NoiseEffect oldCamFxLeft;
//    public NoiseEffect oldCamFxRight;
    private PlayerSoundHandler sh;
    public Vector3 offset;
    public Animator animFlicker;
    public Animator animGrain;
    private AudioSource grainAudio;
    public Image grainImage;
    private bool isInteracting;
    public AnimationCurve walkSpeed;
	public AnimationCurve walkSpeedVirtController;
    private Interaction item;
    public AudioSource onSightSound;
    public bool canMove = true;
    public bool canTOTALMove = true;
    public bool remoteControll = false;
    public bool hardRemoteC = false;
	public bool virtualControl = false;

    private Text endOfDemoText;

	public GameObject[] lightCones;

    private Vector3 fwd;

	private Vector3 hitPoint;
	private float maxHit = 2f;
	private float minHit = 1.5f;

    private  PlayerHealth plHealth;
    public Vector3 Target
    {
        get { return _target; }
        set
        {
            _target = value;



            nAgent.SetDestination(_target);


        }
    }
    void Start()
    {
        plHealth = GetComponent<PlayerHealth>();
        nAgent = GetComponent<NavMeshAgent>();

        head = Camera.main.GetComponent<StereoController>().Head;
        nAgent.updateRotation = false;
        sh = GetComponentInChildren<PlayerSoundHandler>();
        grainAudio = animGrain.gameObject.GetComponent<AudioSource>();

        if (Game.master.controllerSupport)
        {
            JoyStickStart();
			nAgent.acceleration = 200f;
        }else{
			nAgent.acceleration = 8f;
		}

        if (GameObject.FindGameObjectWithTag("DemoEnd") && Application.loadedLevel == 3)
            endOfDemoText = GameObject.FindGameObjectWithTag("DemoEnd").GetComponent<Text>();

        if (hardRemoteC)
        {
            uiStateSprites[3] = uiStateSprites[4];
			lightCones[0].SetActive(false);
			lightCones[1].SetActive(true);
			nAgent.acceleration = 200f;
			Game.handler.saveIconAnim.transform.localPosition = new Vector3 (Game.handler.saveIconAnim.transform.localPosition.x,-20.35f,Game.handler.saveIconAnim.transform.localPosition.z);
        }
        else
        {
            uiStateSprites[3] = uiStateSprites[5];
			lightCones[0].SetActive(false);
			lightCones[1].SetActive(false);
			Game.handler.saveIconAnim.transform.localPosition = new Vector3 (Game.handler.saveIconAnim.transform.localPosition.x,-21.62993f,Game.handler.saveIconAnim.transform.localPosition.z);
			
        }

		if(!SystemInfo.supportsAccelerometer){

           uiStateSprites[3] = uiStateSprites[6];
			
		}
		GetComponent<PlayerSight> ().SetSight(hardRemoteC);

		if(!Game.master.virtualReality)
		if(PlayerPrefs.GetFloat("ctrlSense")!=null && PlayerPrefs.GetFloat("ctrlSense")!= 0f)
				rotBoost = PlayerPrefs.GetFloat("ctrlSense");
    }

    public void ResetFlickerSound()
    {
        isFlickerSoundCD = false;
    }

    public bool isFlickerSoundCD = false;

    public IEnumerator Flicker()
    {       
            //Insight Sound
            if(!isFlickerSoundCD)
                onSightSound.Play();
            isFlickerSoundCD = true;
            Invoke("ResetFlickerSound",30f);
            //grain Sound
            grainAudio.Play();
            //start grain & flicker
            animGrain.SetTrigger("Grain");
            yield return new WaitForSeconds(0.3f);
            animFlicker.SetTrigger("Flicker1");
            
    }

    public void HandleSlenderDamage()
    {
        if (!plHealth.FullHealth)
        {
            if (!grainImage.enabled)
                grainImage.enabled = true;

            if (!grainAudio.isPlaying)
                grainAudio.Play();

            grainAudio.volume = plHealth.GetLostLifePercentage();

            Color _c = grainImage.color;
            _c.a = plHealth.GetLostLifePercentage();
            grainImage.color = _c;
        }
        else
        {

            if (grainImage.enabled)
            {
                //Debug.LogError("fx aus! bei dmg:" +  plHealth.GetLostLifePercentage());
                animGrain.SetTrigger("GrainOff");
                grainImage.enabled = false;
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        //if (Time.timeSinceLevelLoad <= 2.0f)
        //    return;

        if (!canTOTALMove)
            return;

        HandleSlenderDamage();

        fwd = head.transform.TransformDirection(Vector3.forward);
        if (remoteControll)
        {
            JoyRotate();

            //if (hardRemoteC)
            //{
            //    JoyCheckRoll();
            //}
        }

        if (!canMove)
        {
            if (nAgent.hasPath)
                nAgent.ResetPath();

            return;
        }

        if (remoteControll)
            JoyMove();

        if (Physics.Raycast(transform.position + offset, fwd, out hit, 50f))
        {
            if (hit.collider.tag == "Arealock" && hit.distance < 3.8f){

                if (!endOfDemoText.enabled)
                    endOfDemoText.enabled = true;
            }
            else
            {
				if(endOfDemoText)
	                if (endOfDemoText.enabled)
	                    endOfDemoText.enabled = false;

            }
            

            //Debug.DrawLine(transform.position + offset, hit.point, Color.green);
            if (hit.distance > nAgent.stoppingDistance)
            {

                
                //set walkspeed
                if(!remoteControll)
                    nAgent.speed = walkSpeed.Evaluate(head.transform.rotation.eulerAngles.x);

                // wenn interactable -> lade Interaction Component
                if (hit.transform.tag == "Interactable")
                {

                    item = hit.transform.gameObject.GetComponent<Interaction>();


                }

                //if (item && !isInteracting && hit.transform.tag == "Interactable")
                //{

                //    nAgent.ResetPath();
                //    nAgent.SetDestination(item.transform.position);
                    

                //}

                //wenn interactable & im interaction radius des Objects SONST reguläres laufen
                if (hit.transform.tag == "Interactable" && hit.transform.GetComponent<Interaction>() &&
                    hit.distance <= item.interactRadius)
                {
                    // Debug.Log("interacting with:"+hit.transform.name+" from "+hit.distance+" unity away");


					if(item.disappearAfterUse && item.interactDone){
						nAgent.ResetPath();
						isInteracting = true;

						return;
					}
					if(remoteControll){
		                if(item.interactType != InteractionType.Obstacles)
							if(item.LegitControlStop()){

		                    	nAgent.ResetPath();
							}

					}else {

							nAgent.ResetPath();
						
					}


                    //Debug.Log("Interacting");
                    //fire event
                    isInteracting = true;

                	item.MyInteraction();
                    //show state sprite

					if (item.interactDone || item.interactType == InteractionType.None || item.interactType == InteractionType.Obstacles)
                        uiStateImage.sprite = uiStateSprites[2]; //Auge
                    else 
						if (item.interactType == InteractionType.Mechanic && (int)Game.handler.storyState >= (int)item.activeAtStoryState)
                        uiStateImage.sprite = uiStateSprites[3]; //Drehen
					else if(item.requireUI)
						uiStateImage.sprite = item.requireUI;    //Custom
					else
                        uiStateImage.sprite = uiStateSprites[1];    //Gear

                    uiStateImage.enabled = true;


                }
                else
                {
                    if (uiStateImage.enabled)
                        uiStateImage.enabled = false;

                    if (head.transform.rotation.eulerAngles.x < 60f &&
                head.transform.rotation.eulerAngles.x > 2f)
                    {


                        if (remoteControll)
                            return;
                        //Debug.Log("Walking");
                        if (isInteracting)
                        {
                            item = null;
                            isInteracting = false;

                        }

                        //						Target = transform.position + head.transform.forward.normalized *(hitGround.distance - nAgent.stoppingDistance);


                        if (hit.distance < minHit)
                        {
                            return;
                        }
                        else
                            if (hit.distance < maxHit)
                            {

                                hitPoint = hit.point - head.transform.forward.normalized;
                            }
                            else
                            {
                                hitPoint = transform.position + head.transform.forward.normalized * maxHit;

                            }

                        if (Physics.Raycast(hitPoint,
                                            -this.transform.up, out hitGround, 10f))
                        {
                            //Debug.DrawLine(hitPoint, hitGround.point, Color.magenta);
                            Target = hitGround.point;
                        }

                        //new
                        //						if (Physics.Raycast(hit.point - head.transform.forward.normalized,
                        //						                    - this.transform.up, out hitGround, 10f))
                        //						{
                        //							Debug.DrawLine(hit.point - head.transform.forward.normalized, hitGround.point, Color.magenta);
                        //							Target = hitGround.point;
                        //						}


                        //show state sprite
                        //uiStateImage.sprite = uiStateSprites[0];

                    }
                    else
                    {

						isInteracting = false;

                        if (remoteControll)
                            return;
                        
                        nAgent.ResetPath();

                    }
                }

            }
            else
            {
				isInteracting = false;
                if (remoteControll)
                    return;
                
                nAgent.ResetPath();
                
            }
            //Target = hit.point;

        }
        else
		{
			isInteracting = false;
            if (remoteControll)
                return;

            
            nAgent.ResetPath();
            

        }




        if (!nAgent.pathPending) {

			if (nAgent.remainingDistance <= nAgent.stoppingDistance && !isInteracting) {
				if (!nAgent.hasPath || nAgent.velocity.sqrMagnitude == 0f) {
					// Done
					uiStateImage.enabled = false;
					//           Debug.Log("arrived");
				}
			}
		}

    }

    //Controller

//    private string currentButton;
    private float[] axisInput = new float[4];

    void JoyStickStart()
    {
        for (int i = 0; i < axisInput.Length; i++)
            axisInput[i] = 0.0f;

        //// Get the currently pressed Gamepad Button name
        //var values = System.Enum.GetValues(typeof(KeyCode));
        //for (int x = 0; x < values.Length; x++)
        //{
        //    if (Input.GetKeyDown((KeyCode)values.GetValue(x)))
        //    {
        //        currentButton = values.GetValue(x).ToString();
        //    }
        //}

    }

    private void JoyMove()
    {


		if (virtualControl) {
			axisInput [0] = CrossPlatformInputManager.GetAxis ("horiV1");
			axisInput [1] = CrossPlatformInputManager.GetAxis ("vertiV1");
		
		} else {

			axisInput [0] = Input.GetAxis ("hori1");
			axisInput [1] = Input.GetAxis ("verti1");
		}


//		Debug.Log (CrossPlatformInputManager.GetAxis ("horiV1") + "_" + CrossPlatformInputManager.GetAxis ("horiV2"));
        
//        if (Mathf.Max(Mathf.Abs(axisInput[0]), Mathf.Abs(axisInput[1])) <= 0.3f)
//        {
//            nAgent.ResetPath();
//            return;
//        }


		if (virtualControl) {
			nAgent.speed =  walkSpeedVirtController.Evaluate(Mathf.Max(Mathf.Abs(axisInput[0]), Mathf.Abs(axisInput[1])));

		} else {
			
			nAgent.speed = Mathf.Max(Mathf.Abs(axisInput[0]), Mathf.Abs(axisInput[1])) * moveBoost;
		}
        //moveSpeed
        

        if (Physics.Raycast(this.transform.position + offset + (head.transform.forward * axisInput[1] + head.transform.right * axisInput[0]).normalized,
                            -this.transform.up, out hitGround, 10f))
        {


            //Debug.DrawLine(this.transform.position + offset + (head.transform.forward * axisInput[1] + head.transform.right * axisInput[0]).normalized, hitGround.point, Color.yellow);
            Target = hitGround.point;
        }


        



    }
    public Transform neckX;
    public Transform neckY;
	public float rotBoost = 2.5f; 
    public float moveBoost = 1.9f; 

	
	public GameObject virtController;

    private void JoyRotate()
    {

		if (virtualControl) {
			axisInput [2] = CrossPlatformInputManager.GetAxis ("horiV2");
			axisInput [3] = CrossPlatformInputManager.GetAxis ("vertiV2");
			
		} else {
			
			axisInput [2] = Input.GetAxis ("hori2");
			axisInput [3] = Input.GetAxis ("verti2");
		}

        neckX.Rotate(0, axisInput[2] * rotBoost, 0);

        if (!hardRemoteC)
            return;

        if (neckY.transform.rotation.eulerAngles.x < 60f || head.transform.rotation.eulerAngles.x > 320f)
            neckY.Rotate(-axisInput[3] * rotBoost * 0.55f, 0, 0);

        if (neckY.transform.rotation.eulerAngles.x > 60f && head.transform.rotation.eulerAngles.x < 80f)
            neckY.localEulerAngles = new Vector3(59.5f, neckY.localEulerAngles.y, neckY.localEulerAngles.z);

        if (neckY.transform.rotation.eulerAngles.x > 300f && head.transform.rotation.eulerAngles.x < 320f)
            neckY.localEulerAngles = new Vector3(320.5f, neckY.localEulerAngles.y, neckY.localEulerAngles.z);
    }

    private void JoyCheckRoll()
    {
        //Input.acceleration
       //head.transform.localEulerAngles = new Vector3(0, 0, -Input.acceleration.x * rotBoost * 12f);


    }


    public float GetAxis(int _n)
    {
        if (_n <= 3)
            return axisInput[_n];
        else
            return 0f;
    }
}