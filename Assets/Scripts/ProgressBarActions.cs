using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ProgressBarActions : MonoBehaviour
    {
        private Slider progressBar;
        private Mine_Tile parent_tile;

        private void Start()
        {
            parent_tile = gameObject.transform.parent.
                gameObject.transform.parent.
                GetComponent<Mine_Tile>();
        }
        public float Value 
        { 
            get => progressBar.value; 
            set => progressBar.value = value;
        }

        public void SetupBar(int maxValue)
        {
            progressBar = GetComponent<Slider>();
            progressBar.maxValue = maxValue;
            progressBar.enabled = false;
            progressBar.value = 0;
            ShowProgressBar();
        }

        IEnumerator VisibleProgressBar(float targetValue, int step, int duration)
        {
            int t = 0;
            Image[] barImages = GetComponentsInChildren<Image>();
            while (true)
            {
                if (barImages[0].color.a == targetValue) 
                {
                    if (targetValue == 0)
                    {
                        parent_tile.tileState = TileState.ResourceSpawning;
                    }
                    yield break;
                }
                
                yield return null;
                t++;
                Ease ease = new Ease(t, barImages[0].color.a, targetValue, step, duration, EaseType.ExpIn);
                //print($"Alpha {step}: {barImages[0].color.a}");
                for (int i = 0; i < barImages.Length; i++)
                    barImages[i].color = new Color(barImages[i].color.r, barImages[i].color.g, barImages[i].color.b, (float)ease.GetValue);
            }

        }
        public void HideProgressBar() => StartCoroutine(VisibleProgressBar(0, -10, 200));
        public void ShowProgressBar() => StartCoroutine(VisibleProgressBar(255, 1, 100));
    }
}
