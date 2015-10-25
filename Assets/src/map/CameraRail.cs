using UnityEngine;
using System.Collections;

/**

    The camera rail script!

    This script is attached to a ferr2D object that has multiple points
    (preferably with smooth path turned on in the editor options for the terrain) 
    and makes the main camera travel through each point.

    Here's a step-by-step for how it works.


    ---------------------- START() FUNCTION ----------------------


    Firstly, when the script starts up, we get the PolygonCollider2D component 
    attached to the terrain. Note: This collider is only created when the game
    first starts up, and this script might run before the collider is created.
    Therefore, we need to change the script execution order in unity

    (Edit -> Project Settings -> Script Execution Order).

    We then need to add this CameraRail script and set it's time value to 1000 or so.
    On startup, unity goes through this list and runs each script based on it's value.
    At time value 0, all scripts will be run in whatever order. This is where the ferr2D
    terrain mesh will run and where the PolygonCollider2D is created. The next script
    to be run will be this one (or whatever next is on the list). Now that the 
    PolygonCollider2D is created, we can use GetComponent to grab it.

    ----------------------

    Continuing on...
    We have a Vector2 variable called cameraPos which contains our custom
    position for the camera. On update, we set the camera's position to cameraPos.
    This is because a camera's transform.position.x, transform.position.y, ect
    cannot be modified. But the transform.position variable can be assigned to, 
    so we use:

    camera.transform.position = cameraPos;

    This will copy the values of cameraPos to camera.transform.position.
    Changing the values of cameraPos will have no effect to the camera's
    position, unless it is copied to camera.transform.position.

    So, our collider (PolygonCollider2D) has a variable called points which
    is an array of Vector2's. The number of Vector2's in the array can be
    found with collider.points.Length; Each Vector2 is a point in the camera
    rail. The first point at collider.points[0] is the first point on
    the rail. The next point at collider.points[1] is the next point on the rail.
    If you go into the editor, you can see a green line on the object. This
    is the PolygonCollider2D. It might be hard to see each point even if you
    zoom in a lot, especially with smooth path on, but it's there!

    Each Vector2 in the collider is in local space (you can find more information
    about this online), but it basically just means that the collider's position is
    relative the position of the camera rail object.
    For example, say we have an object at position (20, 40). If you child an object
    to it and the child's position is at (0, 0), it would be at the same position
    as the object (20, 40). So the local space is (0, 0) whereas the world space
    is (20, 40).
    How can we convert from local space to world space then? There are multiple ways:

    In our example above, all we have to do is add the parent's position to the child's
    local position.

    So if the parent's position is still (20, 40) but the child's local position is (5, 0), 
    the world space position of the child would be (25, 40).

    Alternatively, child.transform.position is the world space position already calculated.
    However, we aren't using a transform, we're just using Vector2, which doesn't
    have a transform object so we are unable to use that.

    We can also use transform.Transform(childVector2Position);
    This is essentially the same as the 1st option above.

    SO! Now, to convert a collider point to world space, we can use:

    transform.Transform(collider.points[0]);

    This will convert the first point to world space.
    If we did the following:

    camera.transform.position = transform.Transform(collider.points[0]);

    It would move to the first point!

    In our start we're basically doing this, but we wrap our transform.Transform in
    a function called getPoint(index).
    We then do:

    camPos = getPoint(pointIndex);

    pointIndex is set to 0 initially, so we're setting the camera position
    to the first point on the camera rail.

    ----------------------

    The last function in our start function is getNextPoint.
    First, this function adds 1 to the pointIndex, so our pointIndex is currently 1
    at the beginning now. Then, it gets the point from this index (1) and calculates
    the angle from the camera position to this point. It also stores the next point
    value in the nextPoint variable, so we can use it later when moving.
    So, our camera position is at the first point, and our angle is now in the
    direction of the second point. Now if we move in this direction, we will
    end up at the second point.
    We'll go into it more in detail later, but essentially we move to the 2nd point, 
    then get the next point (3rd point) and move in the direction of the 3rd point.
    We then continue this process.



    ---------------------- UPDATE() FUNCTION ----------------------



    We start off with the following code:

    cameraPos.x -= Mathf.Cos(angleToNextPoint) * speed;
    cameraPos.y -= Mathf.Sin(angleToNextPoint) * speed;

    You can find great explanations about how cos and sin work like here
    https://www.mathsisfun.com/geometry/unit-circle.html (where I learnt).

    Basically, all we're doing here is moving the cameraPos x and y position
    in the direction of the next point. Remember, the direction is calculated
    in getNextPoint.
    We then multiply the result by speed (.01 is the current speed) to change how 
    fast the camera will move along the trail.

    ----------------------

    Later on we use Unity's function to calculate the distance between the
    camera position and the next point's position using:

    Vector3.Distance(cameraPos, nextPoint);

    This will let us check if we've arrived at the next point position.
    So by checking that the distance is less than a small number, say, .1f, 
    we now now we have arrived at the next point. Note, if the camera is
    moving too quickly, it can actually go through this check and will
    never arrive at the next point. This is because the distance might be
    .5f but on the next frame, it would be 1 because it travelled on the other
    side of the point.

    So once we have arrived at the next point, we call getNextPoint to
    recalculate the angle to the NEXT point by adding 1 to the
    pointIndex and grabbing, say, the 3rd point (index 2 in the collider
    array).
    We are now travelling to the next point.

    Finally, we set the camera's position to our cameraPos.

    camera.transform.position = new Vector3(cameraPos.x, cameraPos.y, -20);

    The reason we have -20 at the end is that in our game, z represents the 
    depth. So a value of 1000 will be behind all other objects, whereas
    a value of -1000 will be in front of all objects. This is a magic
    number and isn't good! A better solution would be to have a class
    which has values for each depth layer. For example, 

    float BACKGROUND_DEPTH = 0;
    float TRIGGER_DEPTH = -10;
    float TERRAIN_DEPTH = -15;
    float CAMERA_DEPTH = -20;

    ---------------------- CONCLUSION() ----------------------

    And that's it! We grab the first point in the collider, set
    our camera position to it and then calculate the next point.
    We move to the next point, and once we arrive, we calculate
    the next point again. This is then repeated over time!

    Woooo
**/

public class CameraRail : MonoBehaviour {

    public Camera camera;           //camera object (can be changed in editor)
    public float speed;             //the speed the camera travels along it's points (can be changed in editor)

    PolygonCollider2D collider;     //the polygon collider2d component
    Vector2 cameraPos;              //current camera position

    Vector2 nextPoint;              //the calculated next point the camera is travelling to
    int pointIndex;                 //the current position in the collider points array (0 is first, 1 is second, ect)
    float angleToNextPoint;         //the calculated angle to the next point on the rail

	void Start()
    {
        //get polygon collider2d component
        collider = GetComponent<PolygonCollider2D>();

        //sets which point to start at in the collider
        pointIndex = 0;

        //sets the initital camera position to the starting point number (line above)
        cameraPos = getPoint(pointIndex);

        //calculates the next point in the collider
        getNextPoint();
	}

	void Update()
    {
        float dist = Vector3.Distance(cameraPos, nextPoint);

        //moves camera pos towards the next point (calculated in getNextPoint).
        //if the distance to the next point is lower than the speed of the camera, multiply by distance
        cameraPos.x -= Mathf.Cos(angleToNextPoint) * Mathf.Min(dist, speed * Time.deltaTime);
        cameraPos.y -= Mathf.Sin(angleToNextPoint) * Mathf.Min(dist, speed * Time.deltaTime);

        //checks if the distance between the camera and the next point is less than .1f
        //if it is, calculate the next point
        if (Vector3.Distance(cameraPos, nextPoint) <= .1f)
        {
            getNextPoint();
        }

        //update camera position
        camera.transform.position = new Vector3(cameraPos.x, cameraPos.y, -100);
	}

    Vector2 getPoint(int index)
    {
        //gets the point at the input index and transforms it from local space to world space
        return transform.TransformPoint(collider.points[index]);
    }

    void getNextPoint()
    {
        //add 1 to point index (to get the next point in the collider points array)
        ++pointIndex;

        //check if the point index is at the end of the collider points array, and if
        //it is, end the function
        if (pointIndex == collider.points.Length / 2)
        {
            Debug.Log("camera rail done!");
            return;
        }

        //gets the next point and calculates the angle from the camera to the point
        nextPoint = getPoint(pointIndex);
        angleToNextPoint = Mathf.Atan2(cameraPos.y - nextPoint.y, cameraPos.x - nextPoint.x);
    }
}
