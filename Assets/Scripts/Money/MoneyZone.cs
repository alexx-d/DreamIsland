using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoneyZone : MonoBehaviour
{
    [SerializeField] private Dollar _defaultDollar;
    [SerializeField] private string _moneyZoneSavKey;
    [SerializeField] private Collider _collider;

    private List<Dollar> _dollars = new List<Dollar>();

    public event UnityAction Changed;
    public event UnityAction Removed;

    public int Dollars => _dollars.Count;
    public int DollarsValue { get; private set; }
    
    private void OnDisable()
    {
        PlayerPrefs.SetInt(_moneyZoneSavKey, _dollars.Count);
    }

    public void Add(Dollar dollar)
    {
        _dollars.Add(dollar);

        DollarsValue += dollar.Value;
        PlayerPrefs.SetInt(_moneyZoneSavKey, _dollars.Count);

        Changed?.Invoke();
    }

    public Dollar Remove()
    {
        if (_dollars.Count == 0)
            throw new InvalidOperationException("No money");

        var lastDollar = _dollars[_dollars.Count - 1];

        _dollars.Remove(lastDollar);

        DollarsValue -= lastDollar.Value;
        PlayerPrefs.SetInt(_moneyZoneSavKey, _dollars.Count);

        Changed?.Invoke();
        Removed?.Invoke();

        return lastDollar;
    }

    private IEnumerator SpawnMoney(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return null;
            Add(Instantiate(_defaultDollar, transform.position, Quaternion.identity));
        }
    }

    public void SetActive(bool active)
    {
        _collider.enabled = active;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            PlayerStackPresenter stackPresenter = player.GetComponent<PlayerStackPresenter>();
            if (stackPresenter != null)
            {
                Fish fish = player.GetComponentInChildren<Fish>();
                if (fish != null)
                {
                    int fishValue = fish.Value; // Assuming Fish has a Value property
                    stackPresenter.RemoveFromStack(fish);
                    Destroy(fish.gameObject);
                    StartCoroutine(SpawnMoney(fishValue));
                }
            }
        }
    }
}
