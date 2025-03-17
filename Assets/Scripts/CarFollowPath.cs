using UnityEngine;

public class CarFollowPath : MonoBehaviour
{
    [Header("Настройки движения")]
    public Transform[] waypoints;   
    public float moveSpeed = 5f;   
    public float steeringSpeed = 5f;    
    public float stoppingDistance = 0.5f;         
    public float slowDownDistance = 5f;       
    public float offset = 0.5f;    

    [Header("Ссылки на компоненты")]
    public Transform steeringWheel;  
    public Transform carModel;      

    private int currentWaypointIndex = 0;    
    private bool isAtWaypoint = false;     
    private bool isJourneyCompleted = false;    
    private float currentSpeed;    
    float time = 0;
    private void Start()
    {
        currentSpeed = moveSpeed;    
    }

    private void Update()
    {
        if (isJourneyCompleted) return;

        if (waypoints.Length == 0) return;       

        CheckWaypointReached();

        if (!isAtWaypoint)
        {
            MoveTowardsWaypoint();
        }

        SteerWheel();
    }

    private void MoveTowardsWaypoint()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        if (currentWaypointIndex == waypoints.Length - 2)
        {
            SlowDownBeforeLastWaypoint(targetWaypoint);
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        transform.position += direction * currentSpeed * Time.deltaTime;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        carModel.rotation = Quaternion.Lerp(carModel.rotation, targetRotation, Time.deltaTime * steeringSpeed);
    }

    private void SlowDownBeforeLastWaypoint(Transform targetWaypoint)
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, targetWaypoint.position);
        time += Time.deltaTime * 0.2f;
        if (distanceToWaypoint < slowDownDistance)
        {
            currentSpeed = Mathf.Lerp(moveSpeed, 0, time);
        }
    }

    private void CheckWaypointReached()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        if (Vector3.Distance(transform.position, targetWaypoint.position) < stoppingDistance)
        {
            isAtWaypoint = true;

            if (currentWaypointIndex == waypoints.Length - 1)
            {
                CompleteJourney();
            }
            else
            {
                currentWaypointIndex++;

                Invoke(nameof(StartMovingAgain), 0.1f);
            }
        }
        else
        {
            isAtWaypoint = false;
        }
    }

    private void StartMovingAgain()
    {
        isAtWaypoint = false;
    }

    private void SteerWheel()
    {
        if (waypoints.Length == 0 || isJourneyCompleted) return;

        Vector3 currentPos = transform.position;
        Vector3 nextWaypoint = waypoints[currentWaypointIndex].position;

        Vector3 direction = (nextWaypoint - currentPos).normalized;
        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

        Quaternion targetSteeringRotation = Quaternion.Euler(0, 0, -angle);       
        steeringWheel.localRotation = Quaternion.Lerp(steeringWheel.localRotation, targetSteeringRotation, Time.deltaTime * steeringSpeed);
    }

    private void CompleteJourney()
    {
        isJourneyCompleted = true;   
        Debug.Log("Маршрут завершён!");   
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.DrawSphere(waypoints[i].position, 0.2f);

            if (i < waypoints.Length - 1)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }
    }
}
