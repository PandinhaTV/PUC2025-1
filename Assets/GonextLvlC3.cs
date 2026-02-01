using UnityEngine;

public class GonextLvlC3 : MonoBehaviour
{
    public void Interact()
    {
        
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.Session, SceneDatabase.Scenes.Corridor3)
            .WithOverlay()
            .Perform();
    }

    public string GetDescription()
    {
        return "Go to next level";
    }
}
