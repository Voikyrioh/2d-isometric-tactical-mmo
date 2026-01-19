using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    private TextMeshProUGUI count;
    private Image itemImg;

    private void Awake()
    {
        count = GetComponentInChildren<TextMeshProUGUI>();
        itemImg = GetComponent<Image>();
    }

    public void setItem(Item item, int nbr)
    {
        if (!itemImg || !count) return;
        itemImg.sprite = item.sprite;
        count.text = nbr.ToString();
    }
    
    public void updateQuantity(int quantity) => count.text = quantity.ToString();
}
