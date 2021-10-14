using System;

public class Life
{
    private float totalLife = 0;
    private float currentLife = 0;

    private Action OnDead = () => { };

    public float TotalLife => totalLife;
    public float CurrentLife => currentLife;

    public Life(float totalLife, Action OnDead)
    {
        this.OnDead += OnDead;
        this.totalLife = totalLife;
        this.currentLife = totalLife;
    }

    public void GetDamage(float damage)
    {
        currentLife -= damage;
        if (currentLife <= 0) OnDead();
    }
}
