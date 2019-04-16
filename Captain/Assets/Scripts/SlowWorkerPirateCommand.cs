using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Captain.Command;
using UnityEditor.Experimental.SceneManagement;

namespace Captain.Command
{
    public class SlowWorkerPirateCommand : ScriptableObject, IPirateCommand
    {
        private float TotalWorkDuration;
        private float TotalWorkDone = 0.0f;
        private float CurrentWork;
        private float TimeInterval;
        private const float PRODUCTION_TIME = 8.0f;
        private bool Exhausted = false;
        private bool WorkDurationAssigned = false;
        
        // Generates the prefab associated with the pirate
        // by shooting it outward from where the pirate is.
        // Prefab generation based on work duration.
        // For Slow pirate worker, work duration is 20 to 40 seconds.
        public bool Execute(GameObject pirate, Object productPrefab)
        {
            // Only enter this if block once per work duration.
            // We want only one randomized duration value for each cycle.
            if (!WorkDurationAssigned)
            {
                TotalWorkDuration = Random.Range(20.0f, 40.0f);
                WorkDurationAssigned = true;
            }
            if (TimeInterval < PRODUCTION_TIME)
            {
                TotalWorkDone += Time.deltaTime;
                TimeInterval += Time.deltaTime;
                Exhausted = false;
            }
            else
            {
                TimeInterval = 0.0f; // interval reset
                var item = Instantiate(productPrefab, pirate.transform.position, Quaternion.identity) as GameObject;
                var rigidBody = item.GetComponent<Rigidbody2D>(); 
                // move prefab upwards when spawning
                if (rigidBody != null)
                {
                    rigidBody.velocity = new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(2.0f, 5.0f));
                }                          
            }
            
            // duration reset
            if (TotalWorkDone > TotalWorkDuration)
            {
                Exhausted = true;
                TotalWorkDone = 0.0f;
                TimeInterval = 0.0f;
                WorkDurationAssigned = false;
            }
            return Exhausted;
        }
    }
}