using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class CashDesk : LockedGameObject
{

    [SerializeField] private MoneyZone _moneyZone;
    [SerializeField] private Transform _servePoint;
    [SerializeField] private Transform _customerWaitPoint;
    [SerializeField] private StackPresenter _tombstoneStack;
    [SerializeField] private List<StackPresenter> _coffinStacks;

    public event Action BecameFree;

    public bool Active { get; private set; }
    public bool Free { get; private set; } = true;
    public Transform ServePoint => _servePoint;

    private void Start() =>
        SetActive(false);

    public void Leave()
    {
        SetActive(false);
        Free = true;
        BecameFree?.Invoke();
    }

    public void Place(IEnumerable<Stackable> stackData)
    {
        for (int i = 0; i < stackData.Count(); i++)
            _coffinStacks[i].AddToStack(stackData.ElementAt(i));
    }

    private void SetActive(bool active) =>
        Active = active;

    private IEnumerator MoneyZoneActivationDelay()
    {
        yield return new WaitForSeconds(1f);
        _moneyZone.SetActive(true);
    }
}