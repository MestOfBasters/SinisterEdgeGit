using UnityEngine;
using System.Collections;

[System.Serializable]
public class InterAnimScript {

    //animation
    [HideInInspector]
    public Animation anim;
    public string animationFromList = "";
    public bool playAllAnimsFromList = false;

    public float permaAnimSpeed = 1f;

    public bool MakeMyAnimation()
    {
        if (!playAllAnimsFromList)
        {
            //play default Anim if available
            if (animationFromList != "") {

                anim[animationFromList].speed = permaAnimSpeed;
                anim.Play(animationFromList); }
            else { anim.Play(); }

            return true;
        }
        else
        {

            Debug.Log("MultiAnims Inc");
            return true;
        }

        return false;
    }

   
    
}
