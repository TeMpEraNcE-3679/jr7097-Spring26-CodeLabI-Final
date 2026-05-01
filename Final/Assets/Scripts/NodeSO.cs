using UnityEngine;

[CreateAssetMenu(fileName = "NodeSO", menuName = "Scriptable Objects/NodeSO")]
public class NodeSO : ScriptableObject
{
    [Header("Node Content")]
    public string NodeName;
    public string NodeNumber;
    public Sprite Background;
    public Sprite Text;

    [Header("Choices")]
    public Choice[] choices;
    
    [Header("System Check")]
    public int NumberofDays;
    public int NumberofCans;
    public int HungryDays;
    public bool DidUEatMeat;
}

[System.Serializable]
public class Choice
{
    public string choiceText;
    public NodeSO nextNode;
}
