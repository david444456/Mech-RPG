using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepOnlyClip : StateMachineBehaviour
{
    public static float volumeWalkingEnemy = 0.1f;
    public AudioClip clip;

    AudioSource audioSource;
    bool isMoving;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        audioSource = animator.GetComponent<AudioSource>();
        audioSource.volume = volumeWalkingEnemy;
        audioSource.clip = clip;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetFloat("forwardSpeed") > 1)
        {
            if (isMoving) return;

            audioSource.Play();
            isMoving = true;
        }
        else if (isMoving) {
            audioSource.Stop();
            isMoving = false;
        }

    }
    
}
