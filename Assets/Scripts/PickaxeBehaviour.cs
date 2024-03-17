using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    public class PickaxeBehaviour : MonoBehaviour
    {
        private int duration;
        private const float minPickaxeAngle = 0.15f, maxPickaxeAngle = 0.56f;
        private bool firstStagePickaxe = false;
        private Mine_Tile parent_tile;

        private void Start()
        {
              parent_tile = gameObject.transform.parent.gameObject.GetComponent<Mine_Tile>();
        }
        public void AnimationPickaxe()
        {
           duration = 0;
            StartCoroutine(AnimationPickaxeCoroutine());
        }
        public void ShowPickaxe() => StartCoroutine(ChangeAlpha(1,1,1000));
        public void HidePickaxe() => StartCoroutine(ChangeAlpha(0,-1,1000));
        private IEnumerator AnimationPickaxeCoroutine()
        {
            while (true)
            {
                if (parent_tile.tileState != TileState.ProcessMine)
                {
                    ResetPickaxe();
                    yield break;
                }
                if (!firstStagePickaxe)
                {
                    duration++;
                    Ease ease = new Ease(duration, transform.rotation.z, maxPickaxeAngle, 1, 650, EaseType.CirculIn);
                    transform.Rotate(Vector3.forward, (float)ease.GetValue);
                    if (transform.rotation.z >= maxPickaxeAngle)
                    {
                        firstStagePickaxe = true;
                        duration = 0;
                    }
                }
                else
                {
                    duration++;
                    Ease ease = new Ease(duration, transform.rotation.z, minPickaxeAngle, 1, 650, EaseType.CirculIn);
                    transform.Rotate(-Vector3.forward, (float)ease.GetValue);
                    if (transform.rotation.z <= minPickaxeAngle)
                    {
                        firstStagePickaxe = false;
                        duration = 0;
                    }
                }
                yield return null;
            }
        }
        private void ResetPickaxe()
        {
            duration = 0;
            StartCoroutine(ResetPickaxeCoroutine());
        }
        private IEnumerator ResetPickaxeCoroutine()
        {
            while (true)
            {
                if (transform.rotation.z <= minPickaxeAngle)
                {
                    duration = 0;
                    yield break;
                }
                duration++;
                Ease ease = new Ease(duration, transform.rotation.z, minPickaxeAngle, 1, 1000, EaseType.CirculIn);
                transform.Rotate(-Vector3.forward, (float)ease.GetValue);
                yield return null;
            }

        }
        private IEnumerator ChangeAlpha(int targetValue, int step,int duration)
        {
            int t = 0;
            Material pickaxeMaterial = GetComponentInChildren<Renderer>().material;
            while (true) 
            {
                if (pickaxeMaterial.color.a == targetValue)
                {
                    yield break;
                }
                t++;
                Ease ease = new Ease(t, pickaxeMaterial.color.a, targetValue, step, duration, EaseType.CirculIn);
                pickaxeMaterial.color = new Color(pickaxeMaterial.color.r, pickaxeMaterial.color.g, pickaxeMaterial.color.b,(float)ease.GetValue);
                print($"Alpha pickaxe: {pickaxeMaterial.color.a}");
                yield return null;
            }
        }
    }
}
