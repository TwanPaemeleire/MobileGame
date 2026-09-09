using UnityEngine;

[CreateAssetMenu(fileName = "SceneTransitionData", menuName = "CustomSOs/SceneTransitionData")]
public class SceneTransitionData : ScriptableObject
{
    public bool HasStartAnimation = true;
    public string StartAnimationTriggerName = string.Empty;
    public string EndAnimationTriggerName = string.Empty;
    public string MusicToStartOnSwitchToThisScene = string.Empty;
}