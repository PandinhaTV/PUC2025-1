using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CanPlayerSeeMe", story: "[SeesMe] is [true]", category: "Conditions", id: "7068984d3bcf329bc3ad988e0865500f")]
public partial class CanPlayerSeeMeCondition : Condition
{
    [SerializeReference] public BlackboardVariable<bool> SeesMe;
    [Comparison(comparisonType: ComparisonType.Boolean)]
    [SerializeReference] public BlackboardVariable<ConditionOperator> True;

    public override bool IsTrue()
    {
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
