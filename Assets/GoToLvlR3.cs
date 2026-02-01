using UnityEngine;

public class GoToLvlR3 : MonoBehaviour
{
    public void Interact()
    {
        
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.Session, SceneDatabase.Scenes.Corridor4)
            .WithOverlay()
            .Perform();
    }

    public string GetDescription()
    {
        return "Go to next level";
    }
}
