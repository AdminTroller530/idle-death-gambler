using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    Vector2 mousePos;
    [SerializeField] Transform player;
    Vector2 playerPos;
    Vector2 targetPos;
    Vector2 currentLookAheadOffset;

    float lerpSpeed = 8f;
    float maxDistance = 17f;


    void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        playerPos = new Vector2(player.position.x, player.position.y);

        Vector2 lookDir = mousePos - playerPos;
        lookDir = Vector2.ClampMagnitude(lookDir, maxDistance);

        currentLookAheadOffset = Vector2.Lerp(currentLookAheadOffset, lookDir, Time.deltaTime * lerpSpeed);

        transform.position = playerPos + currentLookAheadOffset;
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
}