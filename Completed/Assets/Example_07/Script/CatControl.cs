using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VR.WSA.Input;
using UnityEngine.Windows.Speech;

public class CatControl : MonoBehaviour
{
    public float walkDistance = 10.0f;
    public float walkSpeed = 3.5f;
    public float runSpeed = 7.0f;
    NavMeshAgent agent;
    Animator animator;

    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = null;
    GameObject m_ball;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        // NavMeshAgent를 멈춤.
        if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
        {
            agent.isStopped = true;
        }

        m_ball = GameObject.Find("Ball");
        // 음성 키워드 설정.
        keywords = new Dictionary<string, System.Action>();
        keywords.Add("Go", () =>
        {
            OnClickGround(m_ball.transform.position);
        });
        keywords.Add("Move", () =>
        {
            OnClickGround(m_ball.transform.position);
        });
        keywords.Add("Come", () =>
        {
            OnClickGround(Camera.main.transform.position);
        });
        keywords.Add("Come On", () =>
        {
            OnClickGround(Camera.main.transform.position);
        });

        // 음성 인식 초기화 설정.
        try
        {
            keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
            keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
            keywordRecognizer.Start();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Word : " + args.text);
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
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
                OnClickGround(hit.point);
            }
        }

        if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            return;
        }

        // NavMeshAgent가 움직이고 있을 시 목표지점에 도착했는지 확인하는 부분.
        if (!agent.isStopped && isFinishedWalk())
        {
            agent.isStopped = true;
            // 대기(Idle) 애니메이션을 실행.
            playIdleAnimation();
        }
    }

    public void OnClickGround(Vector3 targetPosition)
    {
        // NavMeshAgent를 다시사용.
        agent.isStopped = false;
        //agent.SetDestination(targetPosition);

        // 경로(Path)를 계산하여 해당경로를 에이전트에 입력.
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(targetPosition, path);
        agent.SetPath(path);

        // 걷기(Walk) 애니메이션을 실행.
        playMoveAnimation();
    }

    bool isFinishedWalk()
    {
        if (!agent.pathPending)
        {
            // 목표지점의 남은거리와 멈출거리를 비교.
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                // 남은 경로가 없거나 현재속도가 0일 경우.
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    void playIdleAnimation()
    {
        int n = UnityEngine.Random.Range(1, 3);
        animator.SetInteger("Value", n);
    }

    void playMoveAnimation()
    {
        float fDistance = getTargetDistance();
        int n;
        // 목표거리와 Walk거리를 비교하여 값을 얻음.
        if (fDistance < walkDistance)
        {
            // 걷기(Walk).
            n = 4;
            agent.speed = walkSpeed;
        }
        else
        {
            // 달리기(Run).
            n = 3;
            agent.speed = runSpeed;
        }
        //int n = Random.Range(3, 5);
        animator.SetInteger("Value", n);
    }

    float getTargetDistance()
    {
        // 에이전트로 부터 NavMeshPath정보를 얻음.
        NavMeshPath path = agent.path;
        float fDistance = 0.0f;

        // NavMeshPath의 코너의 위치정보들의 거리를 구한것을 모두 합함.
        for (int i = 1; i < path.corners.Length; ++i)
        {
            fDistance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }
        return fDistance;
    }
}

