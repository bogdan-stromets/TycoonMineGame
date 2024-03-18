using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Tile_Instance : MonoBehaviour
{
    // прокачка тайла(більше ресурсів) + прокачка кирки(менший час добування) прокачка машини(швидше їде)
    public TileState tileState = TileState.Lock;
    protected GameController gameController;
    private Color defaultColor;
    public int unlockPrice { get; set; }
    protected virtual void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        if (IsTileLock())
            LockTile();
    }
    protected bool IsTileLock() => tileState == TileState.Lock; 
    private void LockTile()
    {
        defaultColor = gameObject.GetComponentInChildren<Renderer>().material.color;
        gameObject.GetComponentInChildren<Renderer>().material.color = new Color(0.3019f, 0.3019f, 0.3019f);
    }
    private void UnlockTile()
    {
        tileState = TileState.ReadyToMine;
        AnimationUnlock();
    }
    public void TryToUnlockTile(int price)
    {
        if (!IsTileLock()) return;

        if (gameController.TryPurchase(price))
            UnlockTile();
        
    }

    private IEnumerator TileSpawnAnimation(int step, int duration)
    {
        int t = 0;
        Vector3 defaultPos = transform.position;
        GameObject gm = Instantiate(gameObject, defaultPos + Vector3.up * 20, transform.rotation);
        gm.GetComponentInChildren<Renderer>().material.color = defaultColor;
        gameController.GetTiles.Add(gm);
        tileState = TileState.Used;
        while (true)
        {
            if (gm.transform.position == defaultPos) 
            {
                gm.transform.SetParent(gameController.TileMap.transform);
                gameController.GetTiles.Remove(gameObject);
                Destroy(gameObject);
                yield break;
            }
            t++;
            Ease ease = new Ease(t, gm.transform.position.y, defaultPos.y, step, duration, EaseType.CirculIn);
            gm.transform.position = new Vector3(gm.transform.position.x,(float)ease.GetValue, gm.transform.position.z);
            yield return null;
        }
    }

    private void AnimationUnlock() => StartCoroutine(TileSpawnAnimation(-10, 1000));
}
