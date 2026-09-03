using UnityEngine;
using System.Collections.Generic;

public class PlayerUI : MonoBehaviour
{
    private Player player;
    [SerializeField] private List<GameObject> playerHPObjects;

    public void Initialize(Player player)
    {
        this.player = player;
        player.OnPlayerHit += PlayerHPDown;
        player.OnPlayerHeal += PlayerHPUp;
    }

    public void PlayerHPDown()
    {
        GameObject disableHp = playerHPObjects[player.curHp];
        disableHp.SetActive(false);
    }

    public void PlayerHPUp()
    {
        GameObject upHp = playerHPObjects[player.curHp - 1];
        upHp.SetActive(true);
    }
}
