using UnityEngine;

public class ClickToStart : MonoBehaviour
{
    // Player
    private CubinhoMovement player;

    
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<CubinhoMovement>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Clicked();
    }


    public void Clicked() 
    {
        if (GameManager.instance.IsLevelsPopUpVisible()) 
        { 
            GameManager.instance.HideLevelsPopUp();
            return;
        }

        if (!player.isPlayPressed)
        {
            player.animator.SetTrigger("transform");
            player.isPlayPressed = true;
        }
    }
}
