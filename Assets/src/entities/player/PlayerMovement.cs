using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    Player parent;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    public Vector3 velocity;
    float velocityXSmoothing;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    public float accelerationTimeAirborne = .2f;
    public float accelerationTimeGrounded = .1f;
    public float moveSpeed = 6;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    public float timeToWallUnstick;

    public float maxClimbAngle = 60;
    public float maxDescendAngle = 40;

    float hop_timer = 0;
    bool hopping = false;
    const float MAX_HOP_FALLVEL = -4.0f;        //if you want to jump while hopping, make sure the fall velocity is greater than this

    int num_jumps = 0;
    const int MAX_JUMPS = 2;

    public void init()
    {
        parent = GetComponent<Player>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    public void update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirX = (parent.controller.collisions.left) ? -1 : 1;

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (parent.controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        bool wallSliding = false;
        if ((parent.controller.collisions.left || parent.controller.collisions.right) && !parent.controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (input.x != wallDirX && input.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallSliding)
            {
                if (wallDirX == input.x)
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if (input.x == 0)
                {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }
                else
                {
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }
            if ((hopping && velocity.y >= MAX_HOP_FALLVEL) || parent.controller.collisions.below)
            {
                hopping = false;
                ++num_jumps;
                velocity.y = maxJumpVelocity;
            }
            else if (num_jumps < MAX_JUMPS)
            {
                hopping = false;
                ++num_jumps;
                //to prevent double jumps off a cliff
                if (num_jumps == 1) ++num_jumps;
                velocity.y = maxJumpVelocity;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            //gun.GetComponent<Gun>().FireBullet();
        }

        velocity.y += gravity * Time.deltaTime;

        parent.controller.Move(velocity * Time.deltaTime);

        if (parent.controller.collisions.above || parent.controller.collisions.below) velocity.y = 0;

        if (parent.controller.collisions.below)
        {
            hop_timer += Time.deltaTime;
            hopping = false;
            num_jumps = 0;
            if (input.x != 0 && hop_timer >= .07f)
            {
                hop_timer = 0;
                velocity.y = 7.0f;
                hopping = true;
            }
        }
    }
}
