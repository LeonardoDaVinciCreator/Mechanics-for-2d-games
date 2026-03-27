using UnityEngine;

public enum TypeObject
{
    Coin,
    Diamont,
    Bomb
}

[CreateAssetMenu(fileName = "Object", menuName = "Scriptable Objects/Object")]
public class Object : ScriptableObject
{    
    public string _description;

    public TypeObject _typeObject;
}
