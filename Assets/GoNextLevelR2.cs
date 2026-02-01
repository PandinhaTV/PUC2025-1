using UnityEngine;

public class GoNextLevelR2 : MonoBehaviour
{
    public void Interact()
    {
        
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.Session, SceneDatabase.Scenes.Room2)
            .WithOverlay()
            .Perform();
    }

    public string GetDescription()
    {
        return "Go to next level";
    }
}
