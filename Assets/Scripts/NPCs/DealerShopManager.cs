using UnityEngine;

public class DealerShopManager : Singleton<DealerShopManager>
{
    public bool IsOpen {private set; get;} = false;
    [SerializeField] private GameObject _canvas;

    private DealerShopAddCard _addCard;

    protected override void Awake()
    {
        base.Awake();
        _addCard = GetComponent<DealerShopAddCard>();
    }

    public void Open()
    {
        _canvas.gameObject.SetActive(true);
        IsOpen = true;
        _addCard.RollCards();
    }

    public void Close()
    {
        _canvas.SetActive(false);
        IsOpen = false;
    }
}
