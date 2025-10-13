using UnityEngine;

public class Railshot : Projectile
{
    public override void InitializeStates()
    {
        AddState("Idle", new RailshotIdle(this));
        AddState("Active", new RailshotActive(this));

        ChangeState("Active");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();

        /* change the size of the railshot projectile according to projectile data */
        gameObject.transform.localScale = new Vector3(projData.railshotLength, gameObject.transform.localScale.y, 1);
    }
}
