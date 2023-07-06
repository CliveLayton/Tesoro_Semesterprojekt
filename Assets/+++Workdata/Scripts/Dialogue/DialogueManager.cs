using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;
using System.Xml.Serialization;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]

    //link to the dialogue panel
    [SerializeField] private GameObject dialoguePanel;

    //link to the dialogue text
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]

    //array of choices for the dialogue
    [SerializeField] private GameObject[] choices;

    [SerializeField] private TextAsset textAsset;

    //array of text for the dialogue
    private TextMeshProUGUI[] choicesText;

    /// <summary>
    /// current ink Story
    /// </summary>
    private Story currentStory;

    /// <summary>
    /// bool for check of dialogue is active
    /// </summary>
    public bool dialogueIsPlaying { get; private set; }

    /// <summary>
    /// static DialogueManager variable
    /// </summary>
    private static DialogueManager instance;

    /// <summary>
    /// Input Action Asset
    /// </summary>
    private GameInput inputActions;

    /// <summary>
    /// bool for player is pressing interact button
    /// </summary>
    public bool isInteracting;

    public bool startDialogue, isInDialogue;

    public string dialoguePath;

    private void Awake()
    {
        inputActions = new GameInput();

        currentStory = new Story(textAsset.text);

        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Mangager in the scene");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        //get all of the choices text
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        //return right away if dialogue isn't playing
        if (!dialogueIsPlaying)
        {
            return;
        }
    }

    /// <summary>
    /// enters dialogue with the inkfile
    /// </summary>
    /// <param name="inkJSON">inkfile with text for the dialogue</param>
    public void EnterDialogueMode(string inkPath)
    {
        isInDialogue = true;
        currentStory.ChoosePathString(inkPath);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    /// <summary>
    /// exit dialogue and disable dialogue panel
    /// </summary>
    /// <returns>waits for 0.2 seconds</returns>
    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        isInDialogue = false;
    }


    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            //set text for the current dialogue line
            dialogueText.text = currentStory.Continue();
            //display choices, if any, for this dialogue line
            DisplayChoices();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    /// <summary>
    /// displays choices if current text has one 
    /// </summary>
    private void DisplayChoices()
    {


        List<Choice> currentChoices = currentStory.currentChoices;

        //go through the remaining choices the UI supports and make sure they're hidden
        for (int i = 0; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        if (currentChoices.Count == 0 || currentChoices == null)
        {
            return;
        }

        //defensive check to make sure our UI can support the number of choices coming in.
        if(currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: " + currentChoices.Count);
        }

        int index = 0;
        //enable and initialize the choices up to the amount of choices for this line of dialogue
        foreach(Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }


        StartCoroutine(SelectFirstChoice());
    }

    /// <summary>
    /// select first choice 
    /// </summary>
    /// <returns>wait for end of frame</returns>
    private IEnumerator SelectFirstChoice()
    {
        //Event System requires we clear it first, then wait for at least one frame before we set current selected object
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    /// <summary>
    /// select the choice from the index of choice objects
    /// </summary>
    /// <param name="choiceIndex"></param>
    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }

    /// <summary>
    /// enables input actions for dialogue
    /// </summary>
    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Interact.performed += Interact;
        inputActions.Player.Interact.canceled += Interact;
    }

    /// <summary>
    /// disables input actions for dialogue
    /// </summary>
    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.Interact.performed -= Interact;
        inputActions.Player.Interact.canceled -= Interact;
    }

    /// <summary>
    /// if player press interact button and current text has no choices, continues story
    /// </summary>
    /// <param name="context"></param>
    void Interact(InputAction.CallbackContext context)
    {
        if(context.performed && startDialogue && !isInDialogue)
        {
            EnterDialogueMode(dialoguePath);
        }
        else if(context.performed && isInDialogue && currentStory.currentChoices.Count == 0)
        {
            ContinueStory();
        }
    }
}
