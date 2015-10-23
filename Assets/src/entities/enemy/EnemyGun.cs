using UnityEngine;
using System.Collections;

public class EnemyGun : Gun
{
    public Enemy enemy;
    float rate_timer;

    void Start()
    {
        GameObject obj = transform.parent.FindChild("base").gameObject;
        if (obj == null) Debug.LogError("Enemies require a child named 'base'");
        enemy = obj.GetComponent<Enemy>();
        enemy.gun = this;
    }

    void Update()
    {
        update_gun_motion(enemy.gameObject, Entities.player.transform.position);

        if (enemy.general_ai_state == GeneralAIState.SHOOTING)
        {
            //rate_timer += Time.deltaTime;
            if (rate_timer >= .1f)
            {
                rate_timer = 0;
                Bullet.spawn<BasicBullet>(pos).init(angle);
            }
        }
	}
}
