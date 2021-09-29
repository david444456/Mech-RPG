using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Core {
    public class MissionSecondary : MonoBehaviour, ISaveable
    {
        public static MissionSecondary missionsSecondary;
        public Dictionary<int, bool[]> missionsComplete = new Dictionary<int, bool[]>();

        [SerializeField] bool[] startedMissions;
        [SerializeField] bool[] missionsCompleteBool;

        private void Awake()
        {
            startGameSetMissions();
            if (missionsSecondary == null)
            {
                missionsSecondary = this;
            }
        }

        public void startMissions(int indexMissions) {

            startedMissions[indexMissions] = true;
            print(startedMissions[indexMissions]);
            missionsComplete[0] = startedMissions;

        }

        public void CompletedMissionsSecondary(int indexMissions) {

            missionsCompleteBool[indexMissions] = true;
            missionsComplete[1] = missionsCompleteBool;
        }

        public bool[] returnTheActualMissionsIncomplete() {
            return missionsComplete[0];
        }

        public bool returnCompleteMissions(int index) {
            missionsCompleteBool = missionsComplete[1];
            return missionsCompleteBool[index];
        }

        void startGameSetMissions() {
            missionsComplete[0] = startedMissions;
            missionsComplete[1] = missionsCompleteBool;
        }

        void restoreMissions() {
            startedMissions = missionsComplete[0];
            missionsCompleteBool = missionsComplete[1];
        }

        public object CaptureState()
        {
            return missionsComplete;
        }

        public void RestoreState(object state)
        {
            startGameSetMissions();
            missionsComplete = (Dictionary<int, bool[]>)state;
            
            restoreMissions();


        }
    }
}
