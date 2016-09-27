using UnityEngine;
using System.Collections;

[System.Serializable]
public class TriggerAnimation {

	public Transform animationTarget;

    public string specificAnimation = "";
    
    private Animation myAnimation;


    public void AssignAnimation()
    {
        myAnimation = animationTarget.GetComponent<Animation>();
    }

    public bool PlayMyAnimation()
    {

        if (myAnimation != null)
        {
            if(specificAnimation != "")
                myAnimation.Play(specificAnimation);
            else

                myAnimation.Play();
            
        }

        return true;
    }
}
