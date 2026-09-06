using System.Collections;
using UnityEngine;

public class RoomDeckAnimation : MonoBehaviour
{
    [SerializeField] private Transform _cardDeckTransform;
    [SerializeField] private Transform _currentCardTransform;
    [SerializeField] private SpriteRenderer _currentCardSpriteRenderer;
    [SerializeField] private Sprite _cardBackSprite;

    private Vector2 CARD_ORIGINAL_POSITION = new Vector2(0, 230);
    private const int CARD_DECK_TARGET_Y = 100;
    private const float INITIAL_MOVE_SPEED = 600f;
    private const float MOVE_DECELERATION = 1400f;
    private const float FLIP_SPEED = 450f;

    private float _currentMoveSpeed;

    public IEnumerator DeckEnterAnimation(RoomCardData card)
    {
        _cardDeckTransform.localPosition = CARD_ORIGINAL_POSITION;
        _currentCardTransform.rotation = Quaternion.identity;
        _currentCardSpriteRenderer.sprite = _cardBackSprite;
        _currentMoveSpeed = INITIAL_MOVE_SPEED;

        // deck comes in
        while (_cardDeckTransform.localPosition.y > CARD_DECK_TARGET_Y)
        {
            _cardDeckTransform.localPosition = Vector2.MoveTowards(_cardDeckTransform.localPosition, new Vector2(_cardDeckTransform.localPosition.x, CARD_DECK_TARGET_Y), _currentMoveSpeed * Time.deltaTime);
            _currentCardTransform.localPosition = _cardDeckTransform.localPosition;
            _currentMoveSpeed -= MOVE_DECELERATION * Time.deltaTime;
            yield return null;

            // FAILSAFE: in case enter animation fails
            if (_currentMoveSpeed < 0) {
                _cardDeckTransform.localPosition = new Vector2(_cardDeckTransform.localPosition.x, CARD_DECK_TARGET_Y);
                _currentCardTransform.localPosition = _cardDeckTransform.localPosition;
                break;
            }
        }

        yield return new WaitForSeconds(0.25f);

        // flip card
        while (_currentCardTransform.localEulerAngles.y < 90)
        {
            _currentCardTransform.localEulerAngles += Vector3.up * FLIP_SPEED * Time.deltaTime;
            yield return null;
        }

        _currentCardTransform.localEulerAngles += Vector3.down * 180;
        _currentCardSpriteRenderer.sprite = card.Sprite;

        while (_currentCardTransform.localEulerAngles.y > 270)
        {
            _currentCardTransform.localEulerAngles += Vector3.up * FLIP_SPEED * Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator DeckExitAnimation()
    {
        _currentMoveSpeed = INITIAL_MOVE_SPEED;

        // deck goes away
        while (_cardDeckTransform.localPosition.y < CARD_ORIGINAL_POSITION.y)
        {
            _cardDeckTransform.localPosition = Vector2.MoveTowards(_cardDeckTransform.localPosition, new Vector2(_cardDeckTransform.localPosition.x, CARD_ORIGINAL_POSITION.y), _currentMoveSpeed * Time.deltaTime);
            _currentCardTransform.localPosition = _cardDeckTransform.localPosition;
            _currentMoveSpeed -= MOVE_DECELERATION * Time.deltaTime;
            yield return null;

            // FAILSAFE: in case enter animation fails
            if (_currentMoveSpeed < 0) {
                _cardDeckTransform.localPosition = new Vector2(_cardDeckTransform.localPosition.x, CARD_ORIGINAL_POSITION.y);
                _currentCardTransform.localPosition = _cardDeckTransform.localPosition;
                break;
            }
        }
    }
}
