using ScallyWags;
using UnityEngine;

public class Animator_DealDamage : StateMachineBehaviour
{
    private float _timer;
    private float _delay = 0.1f;
    private EnemySword _sword;

    private bool _attacked;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _sword = animator.GetComponentInChildren<EnemySword>();
        _sword.EnableCollider(false);
        _timer = 0;
        _attacked = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_attacked) return;
        
        _timer += Time.deltaTime;
        if (_timer > _delay)
        {
            _sword.EnableCollider(true);
            _timer = 0;
            _attacked = true;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _sword.EnableCollider(false);
    }
}
