using UnityEngine;

[CreateAssetMenu(fileName = "NodeSO", menuName = "Scriptable Objects/NodeSO")]
public class NodeSO : ScriptableObject
{
    [Header("Node Content")]
    public string NodeName;
    public Sprite Background;
    public Sprite Text;

    [Header("Choices")]
    public Choice[] choices;
}

[System.Serializable]
public class Choice
{
    public string choiceText;
    public NodeSO nextNode;
}
