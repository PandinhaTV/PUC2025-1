using UnityEngine;

public class GotoLvlR4 : MonoBehaviour
{
    public void Interact()
    {
        
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.Session, SceneDatabase.Scenes.Room4)
            .WithOverlay()
            .Perform();
    }

    public string GetDescription()
    {
        return "Go to next level";
    }
}
