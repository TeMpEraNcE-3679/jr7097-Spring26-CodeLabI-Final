using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Build.Content;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public NodeSO startNode;
    public NodeSO currentNode;

    public Dictionary<string, int> Player = new Dictionary<string, int>();

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Player["Days"] = 0;
        Player["Cans"] = 3;
        Player["HungryDays"] = 0;
        Player["HasEatenMeat"] = 0;

        currentNode = startNode;
        DisplayNode(currentNode);
    }

    public void DisplayNode(NodeSO node)
    {
        currentNode = node;
        Debug.Log("Current Node: " + node.NodeName);
    }

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

    public void ApplyResourceChange(Choice choice)
    {
        if (!string.IsNullOrEmpty(choice.changeResource))
        {
            if (!Player.ContainsKey(choice.changeResource))
            {
                Player[choice.changeResource] = 0;
            }

            Player[choice.changeResource] += choice.changeAmount;
        }
    }

    private NodeSO GetResultNode(Choice choice)
    {
        if (choice.checks == null || choice.checks.Length == 0)
        {
            foreach (SystemCheck check in choice.checks)
            {
                if (ConditionCheck(check))
                {
                    return check.nextNode;
                }
            }
        }

        return choice.nextNode;
    }

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
}

