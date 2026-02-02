using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    public GameObject ContinueButton;
    public InputActionReference jumpAction;
    public VideoPlayer Video;
    public AudioSource Audio;

    private void OnEnable()
    {
        jumpAction.action.Enable();
    }
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Video.loopPointReached += OnVideoEnd;
        if (SaveManager.Instance.existsfile == true)
        {
            Debug.Log("File Exists");
            ContinueButton.SetActive(true);
        }
        else
        {
            Debug.Log("File Doesn't Exists");
            ContinueButton.SetActive(false);
        }

        if (jumpAction.action.WasPressedThisFrame())
        {
            NewGame();
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        NewGame();
    }
    // Update is called once per frame
    void Update()
    {
       
    }

    /*void Continue()
    {
        SceneController.Instance.NewTransition()
            .Load(SceneDatabase.Slots.Session, SceneDatabase.Scenes.)
            .WithOverlay().Perform();
    }*/
    void NewGame()
    {
        SaveManager.Instance.CreateProfile("Player1");
        
            SceneController.Instance.NewTransition().Load(
                        SceneDatabase.Slots.Session,
                        SceneDatabase.Scenes.Corridor1)
                        .WithOverlay()
                        .Perform();
        
        
        
        
    }
}
