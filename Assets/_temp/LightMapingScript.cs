using UnityEngine;
using System.Collections;

public enum LightMapChangeType { Replace, SpezificPos, SpezificPosAndDesOther, SpezificRot, None }
public class LightMapingScript : MonoBehaviour {

    public LightMapChangeType type = LightMapChangeType.None;
    public Transform transmogging;
    public Vector3 transParam;

    public bool invisibleAtStart = false;
	public bool local = false;

    public Vector3 lightParams;

    void Awake ()
    {


        switch (type)
        {
            case LightMapChangeType.Replace:

                this.transform.parent = transmogging.parent;
                this.transform.position = transmogging.position;
                this.transform.SetSiblingIndex(transmogging.GetSiblingIndex());
                Destroy(transmogging.gameObject);

                break;

            case LightMapChangeType.SpezificPos:

                this.transform.localPosition = transParam;
                

                break;

            case LightMapChangeType.SpezificPosAndDesOther:

				if(local)
                	this.transform.localPosition = transParam;
				else
                	this.transform.position = transParam;
                Destroy(transmogging.gameObject);


                break;

            case LightMapChangeType.SpezificRot:

                this.transform.localEulerAngles = transParam;


                break;

            default:

//                Debug.LogError("This LightType is not affected");
                break;
        }

        
    }

	void Start () {
		if (invisibleAtStart)
			this.gameObject.SetActive(false);
	}
}
