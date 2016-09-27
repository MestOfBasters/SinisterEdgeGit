using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class DrawingScript  {

    public Sprite drawingToShow;
    
    [HideInInspector]
    public Animator targetAnimator;

    [HideInInspector]
    public string myGOName;

    public bool ShowDrawing()
    {
        //show Drawing
        //targetAnimator.gameObject.GetComponent<Image>().sprite = drawingToShow;
        //targetAnimator.gameObject.GetComponent<DrawingSoundHandler>().PlayDrawingUnfoldSound();

        Game.handler.playerInventory.AddPicture((myGOName));

        //targetAnimator.gameObject.GetComponent<Text>().text = Game.handler.playerInventory.collectedPictures.Count + " / 5";

        
        //targetAnimator.SetTrigger("Drawing1");

        return true;
        
    }
}
