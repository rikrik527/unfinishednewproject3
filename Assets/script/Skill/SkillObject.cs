
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "skill_object", menuName = "SkillObject/skill/skill list")]
public class SkillObject : ScriptableObject
{
    [SerializeField]
    public List<DemonState> demonStates;
}
