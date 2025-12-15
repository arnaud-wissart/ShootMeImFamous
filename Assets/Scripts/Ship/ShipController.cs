using UnityEngine;

namespace SpaceRocks
{
    [RequireComponent(typeof(Rigidbody))]
    public class ShipController : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed = 12f;

        [SerializeField]
        private float maxSpeed = 12f;

        [SerializeField]
        private float turnSpeed = 720f;

        [SerializeField]
        private Transform muzzle;

        [SerializeField]
        private Projectile projectilePrefab;

        [SerializeField]
        private float fireCooldown = 0.15f;

        private Rigidbody _rigidbody;
        private Vector2 _moveInput;
        private float _nextFireTime;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            ReadMovementInput();
            HandleFiring();
        }

        private void FixedUpdate()
        {
            ApplyMovement();
            ApplyRotation();
        }

        private void ReadMovementInput()
        {
            _moveInput.x = Input.GetAxisRaw("Horizontal");
            _moveInput.y = Input.GetAxisRaw("Vertical");
        }

        private void ApplyMovement()
        {
            var moveDirection = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;
            var targetVelocity = moveDirection * moveSpeed;
            var clamped = Vector3.ClampMagnitude(targetVelocity, maxSpeed);

            // On garde Y à zéro pour rester dans le plan XZ.
            _rigidbody.velocity = new Vector3(clamped.x, 0f, clamped.z);
        }

        private void ApplyRotation()
        {
            if (_moveInput.sqrMagnitude <= 0f)
            {
                return;
            }

            var lookDirection = new Vector3(_moveInput.x, 0f, _moveInput.y);
            var targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            var newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            _rigidbody.MoveRotation(newRotation);
        }

        private void HandleFiring()
        {
            if (!CanFire())
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Fire();
            }
        }

        private bool CanFire()
        {
            return Time.time >= _nextFireTime && projectilePrefab != null && muzzle != null;
        }

        private void Fire()
        {
            Instantiate(projectilePrefab, muzzle.position, transform.rotation);
            _nextFireTime = Time.time + fireCooldown;
        }
    }
}
