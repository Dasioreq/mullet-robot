using UnityEngine;

/// @class Icons
/// @brief Template for upgrade icons
[CreateAssetMenu(fileName = "Icons", menuName = "Scriptable Objects/Icons")]
public class Icons : ScriptableObject
{
    public RandCards.UpgradeType type;
    public string displayName;
    public Sprite icon;
    public Color themeColor = Color.white;
}
