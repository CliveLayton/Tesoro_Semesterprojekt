using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;
using System.Xml.Serialization;
using System.Runtime.CompilerServices;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;

    public bool dialogueIsPlaying { get; private set; }

    private static DialogueManager instance;
    private GameInput inputActions;
    public bool isInteracting;

    private void Awake()
    {
        inputActions = new GameInput();

        if(instance != null)
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

        //handle continuing to the next Line in the dialogue when submit is pressed
        if (isInteracting)
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
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
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

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
        //go through the remaining choices the UI supports and make sure they're hidden
        for(int i = index; i< choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        //Event System requires we clear it first, then wait for at least one frame before we set current selected object
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.UI.Submit.performed += Interact;
        inputActions.UI.Submit.canceled += Interact;

        inputActions.UI.Cancel.performed += CancelStory;
        inputActions.UI.Cancel.canceled += CancelStory;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.UI.Submit.performed -= Interact;
        inputActions.UI.Submit.canceled -= Interact;

        inputActions.UI.Cancel.performed -= CancelStory;
        inputActions.UI.Cancel.canceled -= CancelStory;
    }

    void Interact(InputAction.CallbackContext context)
    {
        isInteracting = context.performed;
    }

    void CancelStory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartCoroutine(ExitDialogueMode());
        }
    }
}
