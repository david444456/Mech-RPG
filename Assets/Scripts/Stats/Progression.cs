using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/ New progression", order = 0)]
    public class Progression : ScriptableObject {
        [SerializeField] ProgressionCharacterClass[] characterClasses;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookUpTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level) {

            BuildLookup();

            float[] levels = lookUpTable[characterClass][stat];

            if (levels.Length < level) {
                return 0;
            }

            return levels[level - 1];
        }

        public int GetLevels(Stat stat, CharacterClass characterClass) {
            BuildLookup();
            float[] levels = lookUpTable[characterClass][stat];
            return levels.Length;

        }

        public string GetStringDead(CharacterClass characterClass) {
            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                if (progressionClass.character == characterClass) {
                    return progressionClass.textKilledYou;
                }
                
            }
            return null;
        }

        private void BuildLookup()
        {
            if (lookUpTable != null) return;

            lookUpTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass progressionClass in characterClasses) {
                var  statLookupTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }

                lookUpTable[progressionClass.character] = statLookupTable;
            }

        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass character;
            public ProgressionStat[] stats;
            [TextArea] public string textKilledYou;
            
        }
        [System.Serializable]
        class ProgressionStat {
            public Stat stat;
            public float[] levels;
        }

    }

}
