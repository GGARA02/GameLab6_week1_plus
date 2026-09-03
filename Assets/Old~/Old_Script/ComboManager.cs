using UnityEngine;

public class ComboManager : MonoBehaviour, IScoreHandler
{
    [Header("combo Values")]
    [SerializeField]
    private int maxCombo = 0;
    [SerializeField]
    private float comboRemainTimeMax = 3f;
  

    private ArrowController arrow;

    private int curScore;
    private int currentCombo;
    private float comboRemainTime;

    private int curComboLvl;

    public void Initialize(ArrowController con)
    {
        arrow = con;
    }

    public void Update()
    {
        comboRemainTime -= Time.deltaTime;
        curComboLvl = currentCombo == 0 ? 0 : currentCombo / 5;
        if (comboRemainTime <= 0f)
        {
            // 콤보 레벨 하락. 만약 레벨 더 안내려 가면 0으로 취급
            if(curComboLvl != 0)
            {
                comboRemainTime = comboRemainTimeMax;
                currentCombo -= 5;
            }
            else
            {
                currentCombo = 0;
                comboRemainTime = 0;
            }
        }
    }


    public void ApplyScore(int score)
    {
        curScore += score;
        currentCombo++;
        comboRemainTime = comboRemainTimeMax;
        maxCombo = Mathf.Max(maxCombo, currentCombo);
    }

    public int GetCombo()
    {
        return currentCombo;
    }

    public int GetMaxCombo()
    {
        return maxCombo;
    }

    public int GetComboLevel()
    {
        return curComboLvl;
     //   return currentCombo == 0 ? 0 : currentCombo / 5;
    }

    public int GetScore()
    {
        return curScore;
    }

    public float GetComboRemainTime()
    {
        return comboRemainTime;
    }

    public float GetMaxComboRemainTime()
    {
        return comboRemainTimeMax;
    }



}
