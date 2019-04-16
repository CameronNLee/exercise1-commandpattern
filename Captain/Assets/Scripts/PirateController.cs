using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Captain.Command;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class PirateController : MonoBehaviour
{
    public IPirateCommand ActiveCommand;
    public GameObject ProductPrefab;

    private bool Active;

    // Start is called before the first frame update
    void Start()
    {
        this.ActiveCommand = ScriptableObject.CreateInstance<NoWorkPirateCommand>();
        this.Active = false;
        
        // Hackneyed way to get Pirate animation to run Idle at start
        this.gameObject.GetComponent<Animator>().SetBool("Exhausted", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.Active)
        {
            var working = this.ActiveCommand.Execute(this.gameObject, this.ProductPrefab);
            if (working)
            {
                this.gameObject.GetComponent<Animator>().SetBool("Exhausted", working);
                this.ActiveCommand = ScriptableObject.CreateInstance<NoWorkPirateCommand>();
            }
        }
    }

    // Has received motivation. A likely source is from on of the Captain's morale inducements.
    public void Motivate()
    {
        var randWorkRate = Random.Range(0, 3);
        switch (randWorkRate)
        {
            case 0:
                this.ActiveCommand = Object.Instantiate(ScriptableObject.CreateInstance<SlowWorkerPirateCommand>());
                Debug.Log(String.Format("Slow work assigned.\n"));
                break;
            case 1:
                this.ActiveCommand = Object.Instantiate(ScriptableObject.CreateInstance<NormalWorkerPirateCommand>());
                Debug.Log(String.Format("Normal work assigned.\n"));
                break;
            case 2:
                this.ActiveCommand = Object.Instantiate(ScriptableObject.CreateInstance<FastWorkerPirateCommand>());
                Debug.Log(String.Format("Fast work assigned.\n"));
                break;
            default : // should not be able to reach this case
                this.ActiveCommand = Object.Instantiate(ScriptableObject.CreateInstance<NoWorkPirateCommand>());
                break;
        }
        
        this.gameObject.GetComponent<Animator>().SetBool("Exhausted", false);
        this.Active = true;
    }
}
