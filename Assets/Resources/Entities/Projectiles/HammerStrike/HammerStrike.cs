using UnityEngine;

public class HammerStrike : Projectile
{
    //public Animn
    private float hammerTimer = 0f;

    public override void InitializeStates()
    {
        AddState("Base", new HammerBase(this));

        ChangeState("Base");
    }
    public override void Start()
    {
        base.Start();
        InitializeStates();
        //audioManager.PlayAudioSource("Impact");
        Destroy(gameObject, audioManager.audioEffects["Impact"].clip.length);
    }

    public new void Update()
    {
        hammerTimer += Time.deltaTime;
        if (hammerTimer > projData.lifeTime)
        {
            spriteRenderer.enabled = false;
            projectileCollider.enabled = false;
        }
    }
}