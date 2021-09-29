using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGM.UI
{
    /// <summary>
    /// Triggers footsteps sounds during playback of an animation state.
    /// </summary>
    public class FootstepTimer : StateMachineBehaviour
    {
        [Range(0, 1)]
        public float leftFoot, rightFoot;
        public AudioClip[] clips;

        float lastNormalizedTime;
        int clipIndex = 0;

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.GetFloat("forwardSpeed") < 1) return;
            var t = stateInfo.normalizedTime % 1 ;

            if (lastNormalizedTime < leftFoot && t >= leftFoot)
            {
                UserInterfaceAudio.PlayClip(clips[clipIndex]);
                clipIndex = (clipIndex + 1) % clips.Length;
            }
            if (lastNormalizedTime < rightFoot && t >= rightFoot)
            {
                UserInterfaceAudio.PlayClip(clips[clipIndex]);
                clipIndex = (clipIndex + 1) % clips.Length;
            }
            lastNormalizedTime = t;

        }
    }
}