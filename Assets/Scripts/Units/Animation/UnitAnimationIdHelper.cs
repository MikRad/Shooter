using UnityEngine;
using System.Collections.Generic;

public static class UnitAnimationIdHelper
{
    private static readonly Dictionary<UnitAnimationState, int> _animationIdMap = new Dictionary<UnitAnimationState, int>();

    private static bool _isInitialized;
        
    public static int GetId(UnitAnimationState state)
    {
        if(!_isInitialized)
        {
            Init();
        }

        if(_animationIdMap.TryGetValue(state, out int id))
        {
            return id;
        }

        return -1;
    }
    
    private static void Init()
    {
        _animationIdMap.Add(UnitAnimationState.Idle, Animator.StringToHash("Idle"));
        _animationIdMap.Add(UnitAnimationState.Move, Animator.StringToHash("MoveSpeed"));
        _animationIdMap.Add(UnitAnimationState.Attack, Animator.StringToHash("Attack"));
        _animationIdMap.Add(UnitAnimationState.Death, Animator.StringToHash("Death"));
        _animationIdMap.Add(UnitAnimationState.Resurrect, Animator.StringToHash("Resurrect"));
        _animationIdMap.Add(UnitAnimationState.SuperBossAttack1, Animator.StringToHash("SBAttack1"));
        _animationIdMap.Add(UnitAnimationState.SuperBossAttack2, Animator.StringToHash("SBAttack2"));
        _animationIdMap.Add(UnitAnimationState.SuperBossAttack3, Animator.StringToHash("SBAttack3"));
        _animationIdMap.Add(UnitAnimationState.SuperBossAttack4, Animator.StringToHash("SBAttack4"));

        _isInitialized = true;
    }
}
