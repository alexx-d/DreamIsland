using UnityEngine;

public class Fish : Stackable
{
    public override StackableType Type => StackableType.Fish;
    public int Value { get; private set; }

    private void Awake()
    {
        Value = Random.Range(10, 101);
    }
}