using UnityEngine;
using UnityEngine.AI;

public class MoveControl : MonoBehaviour {

    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // 마우스 입력 왼쪽클릭 입력.
        if (Input.GetMouseButtonDown(0))
        {
            // 메인카메라에서 Ray를 넘김.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            // 메인카메라에서 받은 Ray를 쏘아서 Collider가 감지되는지 확인 후 
            // 감지가 되었다면 hit변수를 통해서 정보결과를 얻음. 
            if (Physics.Raycast(ray, out hit, 100))
            {
                // hit변수에서 얻은 위치값으로 Agent에 목표지점을 설정함.
                agent.SetDestination(hit.point);
            }
        }
    }
}
