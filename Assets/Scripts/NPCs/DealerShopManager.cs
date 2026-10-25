using System.Collections.Generic;
using UnityEngine;
using static Rarity;

public class DealerShopManager : Singleton<DealerShopManager>
{
    public bool IsOpen {private set; get;} = false;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _addCardMenu;

    private DealerShopAddCard _addCard;
    
    protected override void Awake()
    {
        base.Awake();
        _addCard = GetComponent<DealerShopAddCard>();
    }

    public void Open()
    {
        _canvas.gameObject.SetActive(true);
        _mainMenu.SetActive(true);

        _addCard.RollShopCards();

        IsOpen = true;
    }

    public void Close()
    {
        _canvas.SetActive(false);
        _addCardMenu.SetActive(false);

        IsOpen = false;
    }

    public void OpenAddCardMenu()
    {
        _mainMenu.SetActive(false);
        _addCard.OpenMenu();
    }
}
