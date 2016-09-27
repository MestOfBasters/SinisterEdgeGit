using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class VisibleHandler : MonoBehaviour {

    public List<GameObject> toEnable = new List<GameObject>();
    public List<GameObject> toDisable = new List<GameObject>();

    private bool entered = false;
    private bool exited = true;

    private void EnableList()
    {
        if (toEnable.Count > 0)
        {
            for (int i = 0; i < toEnable.Count; i++)
            {
                toEnable[i].SetActive(true);
            }
        }
    }

    private void DisableList()
    {
        if (toDisable.Count > 0)
        {
            for (int i = 0; i < toDisable.Count; i++)
            {
                if(!toEnable.Contains((toDisable[i])))
                    toDisable[i].SetActive(false);
            }
        }
    }

    public void ChangeTheVisables(bool _entered, bool _exited)
    {
        if (_entered)
            entered = _entered;
        if (_exited)
            exited = _exited;

        if(!entered || !exited)
            return;

        EnableList();
        DisableList();

        toEnable.Clear();
        toDisable.Clear();

        entered = false;
        exited = false;
    }

	public void ResetDueToLoad () {
		
		
		toEnable.Clear();
		toDisable.Clear();
		
		entered = false;
		exited = true;
	}
}
