using UnityEngine;

[System.Serializable]
public class VowsList
{
    public Vow Vow;
    public string Name;
    public string DescriptionGood;
    public string DescriptionBad;

    public VowsList(Vow vow, string name, string descriptionGood, string descriptionBad)
    {
        Vow = vow;
        Name = name;
        DescriptionGood = descriptionGood;
        DescriptionBad = descriptionBad;
    }
}
