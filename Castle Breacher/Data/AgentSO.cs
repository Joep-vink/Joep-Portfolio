using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Agents/AgentData")]
public class AgentSO : ScriptableObject
{
    public int maxHealth, damage;
    public float speed = 10f, timeBetweenAttack;

    [HideInInspector] public int Xp, DropOnDeath;
    [HideInInspector] public string targetLayer;
    [HideInInspector] public float SummonerTime;
    [HideInInspector] public int MaxSummonAmount;

    [field: SerializeField]
    public AgentTypeEnum AgentType { get; set; }

    [field: SerializeField]
    public AgentSideEnum AgentSide { get; set; }

    #region Editor
#if UNITY_EDITOR

    [CustomEditor(typeof(AgentSO))]
    public class TestEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            AgentSO agent = (AgentSO)target;

            EditorUtility.SetDirty(target);

            EditorGUILayout.Space();

            if (agent.AgentType == AgentTypeEnum.Summoner)
            {
                agent.SummonerTime = EditorGUILayout.FloatField("SummonerTime", agent.SummonerTime);
                agent.MaxSummonAmount = EditorGUILayout.IntField("MaxSummonAmount", agent.MaxSummonAmount);
            }

            if (agent.AgentSide == AgentSideEnum.Enemy)
            {
                agent.targetLayer = "Defender";
                agent.Xp = EditorGUILayout.IntField("Xp", agent.Xp);
                agent.DropOnDeath = EditorGUILayout.IntField("DropOnDeath", agent.DropOnDeath);
            }
            else
                agent.targetLayer = "Enemy";
        }
    }

#endif
    #endregion 
}

public enum AgentSideEnum
{
    Friendly,
    Enemy
}

public enum AgentTypeEnum
{
    Melee,
    Ranger,
    Summoner
}
