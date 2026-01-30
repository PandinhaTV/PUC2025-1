
using UnityEngine;

public class CoreManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Core Setup for the game
        //Load everything like audio Managers, Sava System
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.Session, SceneDatabase.Scenes.Room1)
            .WithOverlay()
            .Perform();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnClickCharacter()
    {
        
    }
}
