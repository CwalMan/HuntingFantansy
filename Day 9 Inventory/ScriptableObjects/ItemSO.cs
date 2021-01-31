using UnityEngine;
using System.Text;

[CreateAssetMenu(menuName = "Items/Items")]
public class ItemSO : ScriptableObject
{
    [SerializeField] string id;
    public string ID { get { return id; } }
    public string ItemName;
    public Sprite Icon;

    [Range(1, 999)]
    public int MaxStacks = 1;

    protected static readonly StringBuilder sb = new StringBuilder();

    private void OnValidate()
    {
        string path = System.Guid.NewGuid().ToString();
        id = path;
    }

    public virtual ItemSO GetCopy()
    {
        return this;
    }

    public virtual void Destroy()
    {

    }
    public virtual string GetItemType()
    {
        return "";
    }

    public virtual string GetDescription()
    {
        return "";
    }
}
