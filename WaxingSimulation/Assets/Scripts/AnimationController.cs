using UnityEngine;
using WaxingSimulation.Events;

namespace WaxingSimulation.Controllers
{
    public class AnimationController : MonoBehaviour
    {
        Animator _armAnimator;

        void Start() => _armAnimator = GetComponent<Animator>();

        void OnEnable()
        {
            EventManager.OnComplete += PlayHurtAnim;
        }

        void OnDisable()
        {
            EventManager.OnComplete -= PlayHurtAnim;
        }

        void PlayFinishAnim() => _armAnimator.SetTrigger("Finish");

        void PlayHurtAnim() => _armAnimator.SetTrigger("Hurt");
    }
}