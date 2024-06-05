using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private Rigidbody _body;
    [SerializeField] private MoneyPayer _moneyPayer;
    [SerializeField] private MoneyZone _moneyZone;

    public void EnableInteraction()
    {
        _collider.enabled = true;
        _body.isKinematic = false;
    }

    public void DisableInteraction()
    {
        _collider.enabled = false;
        _body.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MoneyZone"))
        {
            _moneyPayer.Pay(_moneyZone, 100);
        }
    }
}
