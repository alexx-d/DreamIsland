using System.Collections.Generic;
using UnityEngine;

//todo split references for different characters
public class CharacterReferences : MonoBehaviour
{
    [SerializeField] private List<TimerStackableProducer> _producers;

    [field: SerializeField] public ConveyorsContainer ConveyorsContainer { get; private set; }
    [field: SerializeField] public Transform ExitPoint { get; private set; }
    [field: SerializeField] public Conveyor CrematoryConveyor { get; private set; }
    [field: SerializeField] public Transform UrnStorage { get; private set; }
    [field: SerializeField] public StackPresenter UrnStack { get; private set; }

    public IEnumerable<TimerStackableProducer> Producers => _producers;
}