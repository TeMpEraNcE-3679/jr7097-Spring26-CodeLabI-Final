using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//How I start thinking about this game: I wanted to make a text-based game using scriptal object.
//I started the scriptal object script first and then decided to use dictioniary to store the variables.
//The bool variable I used 0&1 to represent. The 3rd thing is singleton.

public class GameManager : MonoBehaviour
{
    //singleton setup
    public static GameManager instance;

    //Scriptal Node Display set up
    public NodeSO startNode;
    public NodeSO currentNode;

    //I used this game to realize dictionary can only by either bool dictionary or int dictionary
    public Dictionary<string, int> Player = new Dictionary<string, int>();

    //Didn't get time to prepare the images, debugs took a long time
    public TMP_Text NarrativeText;
    public Image backgroundImage;

    //The nodes are change primarily via players choice, theres 2 choice at most
    public Button choiceButton0;
    public Button choiceButton1;

    //use these to put in the button's text
    public TMP_Text choiceText0;
    public TMP_Text choiceText1;


    //singleton set up
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    //make sure the choice is connected and the dictionary is filled in at start
    void Start()
    {
        if (choiceButton0 != null)
        {
            choiceButton0.onClick.AddListener(OnClickChoice0);
        }

        if (choiceButton1 != null)
        {
            choiceButton1.onClick.AddListener(OnClickChoice1);
        }

        Player["Days"] = 0;
        Player["Cans"] = 3;
        Player["HungryDays"] = 0;
        Player["HasEatenMeat"] = 0;

        currentNode = startNode;
        DisplayNode(currentNode);
    }

    //referencing what we had in class
    public void DisplayNode(NodeSO node)
    {
        currentNode = node;
        Debug.Log("Current Node: " + node.NodeName);

        if (backgroundImage != null)
        {
            backgroundImage.sprite = node.Background;
        }

        if (NarrativeText != null)
        {
            NarrativeText.text = node.narrativeText;
        }

        HideAllChoiceButtons();

        if (node.choices == null || node.choices.Length == 0)
        {
            return;
        }

        if (node.choices.Length > 0 && node.choices[0] != null)
        {
            if (choiceButton0 != null)
            {
                choiceButton0.gameObject.SetActive(true);
            }
            if (choiceText0 != null)
            {
                choiceText0.text = node.choices[0].choiceText;
            }
        }

        if (node.choices.Length > 1 && node.choices[1] != null)
        {
            if (choiceButton1 != null)
            {
                choiceButton1.gameObject.SetActive(true);
            } 
            if (choiceText1 != null) 
            {
                choiceText1.text = node.choices[1].choiceText;
            }
        }

    }

    //what happens after player make the decision: apply changes first, calculate the result next, then display
    public void AfterChoice(Choice choice)
    {
        ApplyResourceChange(choice);
        NodeSO resultNode = GetResultNode(choice);

        if (resultNode != null)
        {
            DisplayNode(resultNode);
        }
        else
        {
            Debug.Log("No Result Node.");
        }
            

    }

    //A lot of debugging happened here, I gradually add functions to reset the resource whenever the game restarts,
    //add another resource change slot, and refine the starvation setting
    public void ApplyResourceChange(Choice choice)
    {
        if (choice.resetAllResources)
        {
            ResetAllResources();
        }

        if (!string.IsNullOrEmpty(choice.changeResource))
        {
            if (!Player.ContainsKey(choice.changeResource))
            {
                Player[choice.changeResource] = 0;

            }

            if (!Player.ContainsKey(choice.changeResource1))
            {
                Player[choice.changeResource1] = 0;
                
            }

            //HungryDays 

            Player[choice.changeResource] += choice.changeAmount;
            if (choice.changeResource == "HungryDays" && Player["HungryDays"] < 0)
            {
                Player["HungryDays"] = 0;
            }
            Debug.Log(choice.changeResource + " changed to " + Player[choice.changeResource]);

            Player[choice.changeResource1] += choice.changeAmount1;

            Debug.Log(choice.changeResource1 + " changed to " + Player[choice.changeResource1]);
            if (choice.changeResource1 == "HungryDays" && Player["HungryDays"] < 0)
            {
                Player["HungryDays"] = 0;
            }
            Debug.Log("Current Days = " + Player["Days"]);
        }
    }

    //function to check if there need to be check and return the end result of checking
    private NodeSO GetResultNode(Choice choice)
    {
        if (choice.checks == null || choice.checks.Length == 0)
        {
            return choice.nextNode;
        }

        foreach (SystemCheck check in choice.checks)
        {
            if (ConditionCheck(check))
            {
                return check.nextNode;
            }
        }

        return choice.nextNode;
    }

    //function to check the conditions using the enum written in ScriptalObject script
    private bool ConditionCheck(SystemCheck check)
    {
        if (!Player.ContainsKey(check.checkResource))
        {
            Debug.LogWarning("Missing resource: " + check.checkResource);
            return false;
        }

        int currentValue = Player[check.checkResource];
        int targetValue = check.checkValue;

        switch (check.checkType)
        {
            case CheckType.Equal:
                return currentValue == targetValue;

            case CheckType.NotEqual:
                return currentValue != targetValue;

            case CheckType.Less:
                return currentValue < targetValue;

            case CheckType.LessOrEqual:
                return currentValue <= targetValue;

            case CheckType.Greater:
                return currentValue > targetValue;

            case CheckType.GreaterOrEqual:
                return currentValue >= targetValue;
        }

        return false;
    }

    //not show choice buttons if it isn't there
    private void HideAllChoiceButtons()
    {
        if (choiceButton0 != null)
        {
            choiceButton0.gameObject.SetActive(false);
        }

        if (choiceButton1 != null)
        {
            choiceButton1.gameObject.SetActive(false);
        }
    }

    
    private void OnClickChoice0()
    {
        if (currentNode.choices != null && currentNode.choices.Length > 0)
        {
            AfterChoice(currentNode.choices[0]);
        }
    }

    private void OnClickChoice1()
    {
        if (currentNode.choices != null && currentNode.choices.Length > 1)
        {
            AfterChoice(currentNode.choices[1]);
        }
    }

    private void ResetAllResources()
    {
        Player["Days"] = 0;
        Player["Cans"] = 3;
        Player["HungryDays"] = 0;
        Player["HasEatenMeat"] = 0;

        Debug.Log("All resources reset.");
    }
}

