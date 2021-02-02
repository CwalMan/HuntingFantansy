using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using StatMod.Stat;

public class DisplayGunStat : MonoBehaviour//,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] Text NameText;
    [SerializeField] Text ValueText;
    [SerializeField] GunToolTip toolTip;

    private bool showingToolTip;

    private GunStat _stat;
    public GunStat Stat
    {
        get { return _stat; }
        set { _stat = value; }
    }

    private string _name;
    public string Name
    {
        get { return _name; }
        set { _name = value; NameText.text = _name.ToLower(); }
    }

    private void OnValidate()
    {
        Text[] texts = GetComponentsInChildren<Text>();
        NameText = texts[0];
        ValueText = texts[1];

        if(toolTip==null)
        {
            toolTip = FindObjectOfType<GunToolTip>();
        }
    }

    public void UpdateStatValue()
    {
        ValueText.text = _stat.Value.ToString();
        if(showingToolTip)
        {
            //toolTip.ShowToolTip(Stat, name);
        }
    }
    /*
    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTip.ShowToolTip(Stat, name);
        showingToolTip = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.HideToolTip();
        showingToolTip = false;
    }
    */
}
