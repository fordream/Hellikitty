using UnityEngine;
using System.Collections;

public class EnemyGun : GunBase
{
    public Enemy parent;
    float rate_timer;

    public void init(Enemy parent) {
        this.parent = parent;
    }

    public void update() {
        update_gun_motion(parent.gameObject, Entities.player.transform.position);

        if (parent.general_ai_state == GeneralAIState.SHOOTING)
        {
            rate_timer += Time.deltaTime;
            if (rate_timer >= .1f)
            {
                rate_timer = 0;
                Bullet.spawn<BasicBullet>(pos).init(angle);
            }
        }
	}
}
