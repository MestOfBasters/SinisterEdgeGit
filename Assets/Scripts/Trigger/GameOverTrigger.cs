using UnityEngine;
using System.Collections;

public class GameOverTrigger : MonoBehaviour {

    private bool isDone = false;

    void OnTriggerEnter(Collider other)
    {
        if (isDone)
            return;

        if (other.tag == "Player")
        {
            if (other == Game.player.gameObject.GetComponent<CapsuleCollider>())
            {
                EndIt();
            }
        }
    }

    void EndIt()
    {
        
        Game.handler.NewGameState(Game.handler.gameOverState);
        isDone = true;
    }
}
