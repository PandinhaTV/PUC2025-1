using UnityEngine;

public class GoNextLvlR3 : MonoBehaviour
{
    public void Interact()
    {
        
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.Session, SceneDatabase.Scenes.Room3)
            .WithOverlay()
            .Perform();
    }

    public string GetDescription()
    {
        return "Go to next level";
    }
}
