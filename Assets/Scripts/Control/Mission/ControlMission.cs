using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace RPG.Control
{
    public class ControlMission : MonoBehaviour
    {
        [Header("Mission")]
        [SerializeField] [TextArea] string[] _stringMissionInfo;

        [Header("UI")]
        [SerializeField] GameObject _UIGOMission;
        [SerializeField] Text _missionText;

        int countMission = 0;

        void Start()
        {

        }

        public void StartWithMission() {
            //animation
            _UIGOMission.SetActive(true);
            _missionText.text = _stringMissionInfo[0];

        }

        public void NextMission() {
            countMission++;
            //animation
            if (countMission < _stringMissionInfo.Length) {
                _missionText.text = _stringMissionInfo[countMission];
            }
        }
    }
}