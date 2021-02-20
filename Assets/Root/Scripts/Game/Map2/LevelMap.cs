using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2
{
    public class LevelMap : Level
    {
        [SerializeField] private bool isBreathingWave1 = true;
        [SerializeField] private bool isBreathingWave2 = true;
        [SerializeField] private bool isBreathingWave3 = true;
        [SerializeField] private List<WaveMap> listWave;

        public override void OnPass()
        {
            listWave[DataController.Instance.IndexWave].OnPass();
        }

        public override void OnFail()
        {
            listWave[DataController.Instance.IndexWave].OnFail();
        }

        private void Start()
        {
            if ((DataController.Instance.IndexWave == 0 && isBreathingWave1) || (DataController.Instance.IndexWave == 1 && isBreathingWave2) || (DataController.Instance.IndexWave == 2 && isBreathingWave3))
            {
                AudioController.Instance.Play(Const.Common.AUDIOS.BREATHING, true, 0.2f);
            }
        }
    }
}
