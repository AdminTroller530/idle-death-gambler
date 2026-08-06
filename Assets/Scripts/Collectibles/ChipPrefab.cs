using UnityEngine;

public class ChipPrefab : MonoBehaviour
{
    private int _value;
    private bool _isCollected;

    private void OnEnable()
    {
        _isCollected = false;
    }

    public void Initialize(int value)
    {
        _value = value;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !_isCollected)
        {
            ChipsManager.Instance.IncreaseChips(_value);
            _isCollected = true;
            Destroy(gameObject);
        }
    }
}
