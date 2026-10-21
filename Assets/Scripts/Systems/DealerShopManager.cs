using UnityEngine;

public class DealerShopManager : Singleton<DealerShopManager>
{
    public bool IsOpen {private set; get;} = false;
    [SerializeField] private GameObject _canvas;

    public void Open()
    {
        _canvas.gameObject.SetActive(true);
        IsOpen = true;
    }

    public void Close()
    {
        _canvas.SetActive(false);
        IsOpen = false;
    }
}
