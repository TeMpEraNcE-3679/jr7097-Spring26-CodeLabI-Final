using UnityEngine;

[CreateAssetMenu(fileName = "NodeSO", menuName = "Scriptable Objects/NodeSO")]
public class NodeSO : ScriptableObject
{
    //Node Content
    public string NodeName;
    public string NodeNumber;
    public Sprite Background;

    [TextArea]
    public string narrativeText;

    //Next Node that comes up via players' choices
    public Choice[] choices;

}

[System.Serializable]
public class Choice
{
    public string choiceText;
    public NodeSO nextNode;

    public string changeResource;
    public int changeAmount;
    public string changeResource1;
    public int changeAmount1;

    public bool resetAllResources;

    public SystemCheck[] checks;
}

[System.Serializable]
public class SystemCheck
{
    public string checkResource;
    public CheckType checkType;
    public int checkValue;


    public NodeSO nextNode;
}

public enum CheckType
{
    Equal,
    NotEqual,
    Less,
    LessOrEqual,
    Greater,
    GreaterOrEqual
}