using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

// Use physics raycast hit from mouse click to set agent destination
[RequireComponent(typeof(NavMeshAgent))]
public class ClickToMove : MonoBehaviour
{
    NavMeshAgent m_Agent;
    RaycastHit m_HitInfo = new RaycastHit();
    private EventSystem eventSys;

    void Start()
    {
        eventSys = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        m_Agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !eventSys.IsPointerOverGameObject())
        {

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo))
                m_Agent.destination = m_HitInfo.point;
        }
    }
}
