using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Captain.Command;

public class CaptainController : MonoBehaviour
{
    private ICaptainCommand Fire1;
    private ICaptainCommand Fire2;
    private ICaptainCommand Right;
    private ICaptainCommand Left;

    public UnityEngine.UI.Text Booty;
    public int Mushrooms;
    public int Skulls;
    public int Gems;
    
    // Below are for use with modified Fire2 command.
    public int direction;
    public bool UsingDashCommand;
    public static float DashDuration;
    public static float cooldown = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.AddComponent<CaptainMotivateCommand>();
        this.Fire1 = this.gameObject.GetComponent<CaptainMotivateCommand>();
        this.Fire2 = ScriptableObject.CreateInstance<DoNothing>();
        this.Right = ScriptableObject.CreateInstance<MoveCharacterRight>();
        this.Left = ScriptableObject.CreateInstance<MoveCharacterLeft>();
        this.Booty.text = "Booty";
        this.UsingDashCommand = false;

    }

    // Update is called once per frame
    void Update()
    {

        // Fire2 Dash commands dependent on global cooldown variable within this Update() function.
        // When (cooldown <= 0), able to dash upon calling the Fire2 button press (left alt key).
        // After dashing, the cooldown is reset and counts down once again as time goes on.
        // See CaptainDashLeftCommand.cs or CaptainDashRightCommand.cs for implementation.
        cooldown -= Time.deltaTime;

        // Below if block counts down the duration of the dash. When Fire2 button is pressed,
        // UsingDashCommand is set to true, and this if block is able to be accessed.
        // At the same time, the Left and Right movement keys become blocked, as they are
        // now only accessible if UsingDashCommand = false.        
        // This is to prevent left and right movement keys from simply cancelling out the dash
        // while holding the left or right arrow keys. When DashDuration runs out of time,
        // UsingDashCommand is simply set back to false, and the program is able to now
        // make normal use of Left and Right Movement keys.
        if (UsingDashCommand)
        {
            if (DashDuration <= 0)
            {
                UsingDashCommand = false;
            }
            DashDuration -= Time.deltaTime;
        }
        
        if (Input.GetButtonDown("Fire1"))
        {
            this.Fire1.Execute(this.gameObject);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            UsingDashCommand = true;
            if (this.direction == 1) // facing right
            {
                this.Fire2 = ScriptableObject.CreateInstance<CaptainDashRightCommand>();
            }
            else // facing left
            {
                this.Fire2 = ScriptableObject.CreateInstance<CaptainDashLeftCommand>();
            }
            this.Fire2.Execute(this.gameObject);
        }

        if (!UsingDashCommand)
        {
            DashDuration = 0.5f;
            if (Input.GetAxis("Horizontal") > 0.01)
            {
                this.Right.Execute(this.gameObject);
                this.direction = 1;
            }

            if (Input.GetAxis("Horizontal") < -0.01)
            {
                this.Left.Execute(this.gameObject);
                this.direction = 0;
            }
        }

        var animator = this.gameObject.GetComponent<Animator>();
        animator.SetFloat("Velocity", Mathf.Abs(this.gameObject.GetComponent<Rigidbody2D>().velocity.x/5.0f));
        this.Booty.text = "x" + (this.Mushrooms * 1 + this.Gems * 3 + this.Skulls * 2);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.Log(collision);
        if (collision.gameObject.tag == "Mushroom")
        {
            Destroy(collision.gameObject);
            this.Mushrooms++;
        }
        else if (collision.gameObject.tag == "Skull")
        {
            Destroy(collision.gameObject);
            this.Skulls++;
        }
        else if(collision.gameObject.tag == "Gem")
        {
            Destroy(collision.gameObject);
            this.Gems++;
        }
    }
}