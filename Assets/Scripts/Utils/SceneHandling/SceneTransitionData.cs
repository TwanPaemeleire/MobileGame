using UnityEngine;

[CreateAssetMenu(fileName = "SceneTransitionData", menuName = "TEMPNAME",order = 1)]
public class SceneTransitionData : ScriptableObject
{
    public bool HasStartAnimation = true;
    public string StartAnimationTriggerName = string.Empty;
    public string EndAnimationTriggerName = string.Empty;
}
