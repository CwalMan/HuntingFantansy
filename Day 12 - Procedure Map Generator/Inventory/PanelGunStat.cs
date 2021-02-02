using StatMod.Stat;
using UnityEngine;

public class PanelGunStat : MonoBehaviour
{
    [SerializeField] DisplayGunStat[] statDisplays;
    [SerializeField] string[] statNames;

    private GunStat[] stats;

    private void OnValidate()
    {
        statDisplays = GetComponentsInChildren<DisplayGunStat>();
    }
    public void SetStats(params GunStat[] gunStats)
    {
        stats = gunStats;

        if(stats.Length > statDisplays.Length)
        {
            Debug.LogError("Not Enough Stat Displays");
            return;
        }

        for(int i =0; i < statDisplays.Length; i++)
        {
            statDisplays[i].gameObject.SetActive(i < statDisplays.Length);
        }
    }

    public void UpdatestatValues()
    {
        for (int i = 0; i < stats.Length; i++)
        {
            statDisplays[i].UpdateStatValue();
        }
    }
    public void UpdatestatNames()
    {
        for (int i = 0; i < statNames.Length; i++)
        {
            statDisplays[i].Name = statNames[i];
        }
    }
}
