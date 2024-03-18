using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;

public class Mine_Tile : Tile_Instance
{
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private GameObject pickaxe_prefab, resource_prefab;
    [SerializeField] private UnlockPrice price;
    
    private GameObject pickaxe;
    public PickaxeBehaviour pickaxeBehaviour { get; private set; }
    private ProgressBarActions progressBar;

    private int miningTime;
    private bool processUnlock;

    private void OnEnable()
    {
        unlockPrice = (int)price;
    }
    private void Start()
    {
        SpawnPickaxe();
        progressBar = GetComponentInChildren<ProgressBarActions>();
    }
    private void SpawnPickaxe()
    {
        if (pickaxe_prefab == null || IsTileLock()) return;

        pickaxe = new GameObject("PickaxeParent");
        pickaxe.transform.SetParent(transform, false);
        pickaxe.transform.localPosition = new Vector3(0.03f, 0.35f, -0.14f);
        pickaxe.transform.localScale = Vector3.one * 0.2f;
        pickaxe.transform.localRotation = new Quaternion(0.345551431f, 0.6728158f, 0.186718136f, 0.626936615f);
        
        GameObject obj = Instantiate(pickaxe_prefab);
        obj.transform.SetParent(pickaxe.transform, false);
        obj.transform.localPosition = new Vector3(0,0.3f,0);
        obj.transform.localScale = Vector3.one * 1.932033f;

        pickaxeBehaviour = pickaxe.AddComponent<PickaxeBehaviour>();
    }
    void Update()
    {
        print($"Tile State: {tileState}");
    }
    

    private void OnMouseDown()
    {
        if (processUnlock) return;
        switch (tileState)
        {
            case TileState.ReadyToMine:    ProcessMine(); break;
            case TileState.ResourceReady: PickupResource(); break;
        }
    }

    public void PickupResource()
    {
        if (gameController.CharacterScr.characterState == CharacterState.Move ||
            gameController.CharacterScr.characterState == CharacterState.Busy ||
            gameController.GetTruck.GetComponentInChildren<TruckController>().Getstate == TruckState.Full) return;

        PathCreator pathToTile = GetComponentInChildren<PathCreator>();
        //pathToTile.pathReversed = false;
        gameController.Character.AddComponent<FollowPath>().Path = pathToTile;
        gameController.CharacterScr.target_tile = this;
        //GetComponentInChildren<ResourceActions>().TakeResource();
    }

    private void ProcessMine()
    {
        if (tileState == TileState.ProcessMine || tileState == TileState.Used) return;
        tileState = TileState.ProcessMine;
        miningTime = 0;

        pickaxeBehaviour.AnimationPickaxe();
        progressBar.SetupBar(TimeToMine());
        StartCoroutine(TimerMine(miningTime));
    }
    private void SpawnResource()
    {
        pickaxeBehaviour.HidePickaxe();
        GameObject resource = Instantiate(resource_prefab,transform.position + Vector3.down * 2,Quaternion.identity);
        resource.transform.localScale = Vector3.one * 3;
        resource.transform.SetParent(transform, true);
        ResourceActions resourceActions = resource.AddComponent<ResourceActions>();
        resourceActions.GameController = gameController;

        //tileState = TileState.ResourceReady;
        // закінчив тут
    }
    IEnumerator TimerMine(int miningTime)
    {
        while (miningTime != TimeToMine()) 
        {
            yield return new WaitForSeconds(1f);
            miningTime++;
            progressBar.Value = Mathf.Lerp(progressBar.Value, miningTime,100);
            Debug.Log($"Mining Time: {miningTime}");
        }
        yield return null;        
        progressBar.HideProgressBar();
        SpawnResource();
    }
    private int TimeToMine()
    {
        switch (resourceType)
        {
            case ResourceType.Clay: return 1;//5;
            case ResourceType.Coal: return 1;//10;
            case ResourceType.Stone: return 20;
            case ResourceType.Iron: return 15;
            case ResourceType.Gold: return 25;
            case ResourceType.Diamond: return 30;
            default: return 0;
        }
    }
}
