using UnityEngine;
using WaxingSimulation.Events;

namespace WaxingSimulation.Controllers
{
    public class WaxStickController : MonoBehaviour
    {
        [SerializeField] GameObject _rayHitPoint;
        [SerializeField] LayerMask _hitMask;
        [SerializeField] WaxingController _waxingController;
        float _cameraZDistance;


        void Start() => _cameraZDistance = Camera.main.WorldToScreenPoint(transform.position).z;
        void OnEnable() => EventManager.OnWaxDrying += HideStick;
        void OnDisable() => EventManager.OnWaxDrying -= HideStick;

        void FixedUpdate()
        {
            if (!Input.GetMouseButton(0))
                return;

            MoveStick();
        }

        void MoveStick()
        {
            Vector3 mousePos = Input.mousePosition;

            Vector3 screenPosition = new Vector3(mousePos.x, mousePos.y, _cameraZDistance);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

            transform.position = worldPosition;

            Raycast();
        }

        void Raycast()
        {
            RaycastHit hit;
            if (Physics.Raycast(_rayHitPoint.transform.position, _rayHitPoint.transform.forward, out hit, 30f, _hitMask))
            {
                WaxingController wax = hit.transform.GetComponent<WaxingController>();
                Debug.DrawRay(_rayHitPoint.transform.position, _rayHitPoint.transform.forward);
                wax.DeformMeshAtPoint(hit.point);
            }
        }

        void HideStick()
        {
            gameObject.SetActive(false);
        }
    }
}