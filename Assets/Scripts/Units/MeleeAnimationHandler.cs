using System;
using UnityEngine;

namespace Units
{
    public class MeleeAnimationHandler : MonoBehaviour
    {
        public event Action OnAttackPerformed;

        // Used in animation event
        private void PerformAttack()
        {
            OnAttackPerformed?.Invoke();
        }
    }
}
