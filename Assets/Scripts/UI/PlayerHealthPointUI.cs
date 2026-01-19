using DefaultNamespace.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthPointUI : MonoBehaviour
{
    private Image healthPointBar;
    private TextMeshProUGUI healthPointText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthPointBar = GetComponent<Image>();
        healthPointText = GetComponentInChildren<TextMeshProUGUI>();
        OnPlayerHPChanged(GameManager.Instance.playerInstance.hp);
        GameManager.Instance.playerInstance.hpChanged.AddListener(OnPlayerHPChanged);
    }

    private void OnPlayerHPChanged(int hp)
    {
        healthPointBar.fillAmount = (float)hp / GameManager.Instance.playerInstance.maxHP;
        healthPointText.text = $"{hp}HP";
    }
}
