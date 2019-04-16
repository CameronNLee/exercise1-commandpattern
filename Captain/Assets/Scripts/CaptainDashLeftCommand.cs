using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Captain.Command;
using UnityEditor.Experimental.SceneManagement;

namespace Captain.Command
{
    /* Simple custom command using the Fire2 ActiveCommand of CaptainController script.
     * Fire2 is mapped to the left-Shift key code. Upon the button being pressed,
     * Captain receives a slight speed boost that is noticeably faster
     * than the regular walk movements of left and right. This effect lasts
     * for around half a second, and each dash press has a timer cooldown of 1.5 seconds.
     *
     * The actual cooldown timer is set and depleted inside of CaptainController.cs's Update().
     * 
     * Note: for use only with Left movements. */
    public class CaptainDashLeftCommand : ScriptableObject, ICaptainCommand
    {
        private float DashSpeed = 20.0f;
        public void Execute(GameObject gameObject)
        {
            if (CaptainController.cooldown > 0)
            {
                Debug.Log(String.Format("Cooldown: {0}s.\n", Mathf.Round(CaptainController.cooldown * 100f) / 100f));
            }
            else // reset timer
            {
                var rigidBody = gameObject.GetComponent<Rigidbody2D>();
                if (rigidBody != null)
                {
                    rigidBody.velocity = Vector2.left * DashSpeed; // add the actual dash
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                }
                CaptainController.cooldown = 1.5f;
            }
        }
    }
}