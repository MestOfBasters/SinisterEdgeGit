using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class IntroHandler : MonoBehaviour
{

    public Text everbyte;
    public Text presents;
    public Image sinisterEdge;
	// Use this for initialization
	void Start ()
	{
        StartCoroutine(UiFadeText(everbyte, 4f, 2f, 1f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void loadNextScene(string _scene)
    {
       
        Application.LoadLevel(_scene);
    }
    public IEnumerator UiFadeObject(Image _img, float _delay, float _time, float _alpha, bool _toggleRayCastTarget)
    {
        yield return new WaitForSeconds(_delay);
        _img.CrossFadeAlpha(_alpha, _time, false);
        if (_toggleRayCastTarget)
            _img.raycastTarget = false;
    }
    public IEnumerator UiFadeText(Text _img, float _delay, float _time, float _alpha)
    {
        
        yield return new WaitForSeconds(_delay);
//        Debug.Log("fading text");
        _img.CrossFadeAlpha(_alpha, _time, false);
    }
}
