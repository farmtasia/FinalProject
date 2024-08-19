using UnityEngine;

[CreateAssetMenu(fileName = "Crop", menuName = "New Crop")]
public class CropSO : ScriptableObject
{
    [Header("Info")]
    public string name;
    public float expValue;
}
