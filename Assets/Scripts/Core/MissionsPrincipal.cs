using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Core
{
    public class MissionsPrincipal : MonoBehaviour, ISaveable
    {
        public static MissionsPrincipal missionsPrincipal;
        public bool[] missionsComplete;

        // Start is called before the first frame update
        void Awake()
        {
            if (missionsPrincipal == null)
                {
                    missionsPrincipal = this;
                }
        }

        public int returnTheActualMission() {
            for (int i = 0; i < missionsComplete.Length; i++) {
                if (missionsComplete[i] == false) return i;
            }
            return 0;
        }

        public object CaptureState()
        {
           return missionsComplete;
        }

        public void RestoreState(object state)
        {
            missionsComplete = (bool[])state;
        }
    }
}
