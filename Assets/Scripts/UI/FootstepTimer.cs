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
        [SerializeField] bool isThePlayer = true;
        [Range(0, 1)]
        public float leftFoot, rightFoot;
        public AudioClip[] clips;

        float lastNormalizedTime;
        int clipIndex = 0;
        AudioSource audioSource;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (isThePlayer) return;
            audioSource = animator.GetComponent<AudioSource>();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.GetFloat("forwardSpeed") < 1) return;
            var t = stateInfo.normalizedTime % 1 ;

            if (lastNormalizedTime < leftFoot && t >= leftFoot)
            {
                PlaySound(clips[clipIndex]);
                clipIndex = (clipIndex + 1) % clips.Length;
            }
            if (lastNormalizedTime < rightFoot && t >= rightFoot)
            {
                PlaySound(clips[clipIndex]);
                clipIndex = (clipIndex + 1) % clips.Length;
            }
            lastNormalizedTime = t;
        }

        private void PlaySound(AudioClip audioClip) {
            if (isThePlayer) UserInterfaceAudio.PlayClip(audioClip);
            else audioSource.PlayOneShot(audioClip);
        }
    }
}