using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    private Player parent;

    private float gravity;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    public Vector3 velocity;
    private float velocityXSmoothing;

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
    private float timeToWallUnstick;

    private float hop_timer = 0;
    private bool hopping = false;
    private const float MAX_HOP_FALLVEL = -4.0f;        //if you want to jump while hopping, make sure the fall velocity is greater than this

    private int num_jumps = 0;
    public int max_jumps = 2;
    private bool wallSliding = false;

    public void init()
    {
        parent = GetComponent<Player>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    public void update()
    {
        velocity.y += gravity * Time.deltaTime;

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirX = (parent.controller.collisions.left) ? -1 : 1;

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (parent.controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        wallSliding = false;
        if ((parent.controller.collisions.left || parent.controller.collisions.right) && !parent.controller.collisions.below && input.x != -wallDirX)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }
			velocityXSmoothing = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
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
            else if ((hopping && hop_timer >= 1) || parent.controller.collisions.below || num_jumps < max_jumps)
            {
                hopping = false;
                ++num_jumps;
                velocity.y = maxJumpVelocity;
            }
        }

        parent.controller.Move(velocity * Time.deltaTime);

        if (parent.controller.collisions.above || parent.controller.collisions.below) velocity.y = 0;

        hop_timer += Time.deltaTime;
        if (parent.controller.collisions.below)
        {
            hopping = false;
            num_jumps = 0;
            if (input.x != 0)
            {
                hop_timer = 0;
                velocity.y = 7.0f;
                hopping = true;
            }
        }
    }
}
