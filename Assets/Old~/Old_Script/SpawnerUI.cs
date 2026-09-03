using TMPro;
using UnityEngine;

public class SpawnerUI : MonoBehaviour, ISpawnUI
{
    [SerializeField]
    private TextMeshProUGUI currentWaveText;
    
    void Start()
    {
        UpdateWaveText(1);
    }

    public void UpdateWaveText(int wave)
    {
        currentWaveText.text = "Wave: " + wave;
    }
}
