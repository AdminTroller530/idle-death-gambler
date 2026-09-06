using UnityEngine;

public class HealthCard : MonoBehaviour
{
    private int _id;
    private float _bobSpeed = 3f;
    private float _bobAmplitude = 3f;
    private float _bobStartOffset;
    private bool _isActive = true;
    private const float START_Y = 150;

    private Animator _animator;

    public void Initialize(int id)
    {
        _id = id;
        transform.localPosition = new Vector2(-290 + _id * 30, START_Y);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _bobStartOffset = Random.Range(0, 2*Mathf.PI);
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerChangeHealth += PlayerChangeHealth;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerChangeHealth -= PlayerChangeHealth;
    }

    private void PlayerChangeHealth(int health)
    {
        if (_isActive && health <= _id) {
            _isActive = false;
            _animator.SetTrigger("FlipToInactive");
        }
        else if (!_isActive && health > _id)
        {
            _isActive = true;
            _animator.SetTrigger("FlipToActive");
        }
    }

    private void Update()
    {
        transform.localPosition = new Vector2(transform.localPosition.x, (int)(START_Y + (_bobAmplitude * Mathf.Sin((Time.time + _bobStartOffset) * _bobSpeed))));
    }
}
