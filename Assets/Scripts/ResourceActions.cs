using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class ResourceActions : MonoBehaviour
    {
        private GameController gameController;
        private bool spawned, taked;
        private Mine_Tile mine_Tile;
        private PathCreator tilePath;
        public GameController GameController 
        {
            set 
            {
                gameController = value;
            }
        }
        void Start () 
        {
            SpawnResource();
            mine_Tile = transform.parent.GetComponent<Mine_Tile>();
            tilePath = transform.parent.GetComponentInChildren<PathCreator>();
        }

        void Update () 
        {
            if (spawned && !taked) 
            {
                RotateAnim();
            }
            TryToTakeResource();
        }
        private void ResourceToCharacter()
        {
            taked = true;
            // animation to character
            StartCoroutine(AnimResourceToCharacter(-1,1000));
        }
        IEnumerator AnimResourceToCharacter(int step, int duration)
        {
            int t = 0;
            while (true) 
            {
                t++;
                float changeValue = transform.localScale.x;
                if (changeValue == 0) 
                {
                    TakeResource();
                    yield break;
                }
                Ease ease = new Ease(t, changeValue, 0, step, duration, EaseType.ExpIn);
                transform.localScale = Vector3.one * (float)ease.GetValue;
                yield return null;
            }
        }
        protected void TryToTakeResource()
        {
            if (gameController.CharacterScr.target_tile == null) return;

            PathCreator characterPath = gameController.CharacterScr.target_tile.GetComponentInChildren<PathCreator>();

            if (tilePath == null || characterPath == null) return;
            if (gameController.CharacterScr.characterState == CharacterState.Busy
                && mine_Tile.tileState == TileState.ResourceReady
                && tilePath == characterPath)
            {
                ResourceToCharacter(); 
            }
        }
        private void TakeResource()
        {
            if(gameController.Character.GetComponent<FollowPath>() != null) return;
            gameController.Character.AddComponent<FollowPath>().Path = tilePath;
            mine_Tile.pickaxeBehaviour.ShowPickaxe();
            mine_Tile.tileState = TileState.ReadyToMine;
            Destroy(gameObject); 
        }
   /*     private bool HasFreeCells()
        {
           // return gameController.transform.childCount;
        }*/
        void RotateAnim()
        {
            transform.Rotate(0, 0.2f, 0);
        }
        private void SpawnResource() => StartCoroutine(SpawnAnim(1,500));
        IEnumerator SpawnAnim(int step,int duration)
        {
            int t = 0;
            while (true) 
            {
                if (transform.position.y == transform.parent.position.y + 2)
                {
                    spawned = true;
                    transform.parent.gameObject.GetComponent<Tile_Instance>().tileState = TileState.ResourceReady;
                    yield break;
                }
                t++;
                Ease ease = new Ease(t, transform.position.y, transform.parent.position.y + 2, step, duration, EaseType.ExpIn);
                transform.position = new Vector3(transform.position.x,(float)ease.GetValue ,transform.position.z);
                yield return null;
            }
        }
    }
}
