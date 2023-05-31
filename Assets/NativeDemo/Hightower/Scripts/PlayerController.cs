using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
namespace PubScale.SdkOne.NativeAds.Hightower
{
    public class PlayerController : MonoBehaviour
    {
        private static readonly int GroundedKey = Animator.StringToHash("Grounded");
        private static readonly int JumpKey = Animator.StringToHash("Jump");
        private static readonly int FlipKey = Animator.StringToHash("Flip");

        public static event Action<int> OnFloor;
        [SerializeField] private AudioClip[] splatSounds;
        [SerializeField] private AudioClip[] hitSounds;
        [SerializeField] private AudioClip[] jumpSounds;
        [SerializeField] private ParticleSystem blood;
        [SerializeField] private ParticleSystem moveParticle;
        [SerializeField] private ParticleSystem jumpParticle;
        [SerializeField] private ParticleSystem landParticle;
        [SerializeField] private ParticleSystem glassParticle;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Animator anim;
        [SerializeField] private Animator characterAnim;
        [SerializeField] private Transform holder;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private Transform groundCheck2;
        [SerializeField] private Transform wallCheckRight;
        [SerializeField] private Transform wallCheckLeft;
        [SerializeField] private float groundCheckDistance;
        [SerializeField] private float wallCheckDistance;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float wallJumpForce;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private LayerMask wallMask;
        [SerializeField] private float coyoteTime = 0.2f;
        [SerializeField] private float jumpBufferTime = 0.2f;
        [SerializeField] private AudioSource audioSrc;
        private FloorHandler myFloor;
        private bool wasGrounded;
        private bool grounded;
        private bool onRightWall;
        private bool onLeftWall;
        private bool isAlive = true;
        private bool isJumping;
        private bool GameOn = false;
        private bool haveTutorial = false;
        private float coyoteTimeCounter;

        private float jumpBufferCounter;
        private bool facingLeft;
        private bool first;
        private bool CanJump;

        private void Awake()
        {
            haveTutorial = PlayerPrefs.GetInt("tutorial", 1) == 1;
            UIManager.GamePaused += UIManager_GamePaused;
            isAlive = true;
            CanJump = true;
            first = true;
        }

        private void UIManager_GamePaused(bool obj)
        {
            GameOn = !obj;
        }

        public void InitPlayer()
        {
            isAlive = true;
            GameOn = true;
        }
        bool CheckUI()
        {
            bool canTouch = true;
            bool canTap = true;
            foreach (Touch touch in Input.touches)
            {
                int id = touch.fingerId;
                if (EventSystem.current.IsPointerOverGameObject(id))
                {
                    canTouch = false;
                    break;
                }
                else
                {
                    canTouch = true;
                    break;
                }
            }
            if (EventSystem.current.IsPointerOverGameObject() && Input.touchCount == 0)
                canTap = false;
            else
                canTap = true;

            if (canTap && canTouch)
                return true;
            else
                return false;

        }
        private void Update()
        {
            if (!isAlive)
                return;
            RaycastHit2D groundHit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundMask);
            RaycastHit2D groundHit2 = Physics2D.Raycast(groundCheck2.position, Vector2.down, groundCheckDistance, groundMask);
            RaycastHit2D rightWallHit = Physics2D.Raycast(wallCheckRight.position, transform.right, wallCheckDistance, wallMask);
            RaycastHit2D leftWallHit = Physics2D.Raycast(wallCheckLeft.position, -transform.right, wallCheckDistance, wallMask);
            if (!isJumping)
                grounded = groundHit || groundHit2;
            onRightWall = rightWallHit;
            onLeftWall = leftWallHit;
            rb.velocity = new Vector2(transform.right.x * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
            if (onRightWall)
            {
                audioSrc.PlayOneShot(splatSounds[UnityEngine.Random.Range(0, splatSounds.Length)], 0.2f);
                characterAnim.SetTrigger(FlipKey);
                transform.localRotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y == 0 ? 180 : 0, 0));
                facingLeft = transform.eulerAngles.y == 180;
                if (rightWallHit.collider.CompareTag("glass"))
                {
                    myFloor = rightWallHit.collider.GetComponentInParent<FloorHandler>();
                    myFloor.BreakGlass(glassParticle, rightWallHit.collider.gameObject);
                }
            }
            if (!wasGrounded && grounded)
            {
                //audioSrc.PlayOneShot(_footsteps[UnityEngine.Random.Range(0, _footsteps.Length)]);

                characterAnim.SetTrigger(GroundedKey);
                landParticle.Play();
            }
            if (rb.velocity.y > 0)
            {
                rb.gravityScale = 1.5f;
            }
            else
            {
                rb.gravityScale = 1;
                anim.SetBool("run", false);
                anim.SetBool("jump", false);
            }
            if (groundHit)
            {
                //if (coyoteTimeCounter != coyoteTime)
                //    holder.DOScale(new Vector3(1f, 0.45f, 1), 0.2f).SetEase(Ease.InBounce).OnComplete(() => { holder.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBounce); });
                moveParticle.Play();
                anim.SetBool("run", true);
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                moveParticle.Pause();
                anim.SetBool("run", false);
                coyoteTimeCounter -= Time.deltaTime;
            }
            if ((Input.GetMouseButtonDown(0) || (Input.GetKeyDown(KeyCode.Space) && Time.timeScale != 0 && GameOn)) && (first || CheckUI()))
            {
                first = false;
                // moveParticle.Stop();
                jumpBufferCounter = jumpBufferTime;
                // moveParticle.Stop();
                //Jump();
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }
            if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumping && CanJump)
            {
                rb.gravityScale = 1;
                rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
                StartCoroutine(JumpCooldown());
                jumpParticle.Play();
                anim.SetBool("jump", true);
                //  moveParticle.Stop();
                audioSrc.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
                characterAnim.SetTrigger(JumpKey);
                characterAnim.ResetTrigger(GroundedKey);
                audioSrc.PlayOneShot(jumpSounds[UnityEngine.Random.Range(0, jumpSounds.Length)], 0.3f);
                coyoteTimeCounter = 0f;
                jumpBufferCounter = 0f;
                //StartCoroutine(JumpDelay());
            }
            wasGrounded = grounded;
        }
        private IEnumerator JumpCooldown()
        {
            isJumping = true;
            CanJump = false;
            yield return new WaitForSeconds(0.6f);
            isJumping = false;
            CanJump = true;
        }
        public void Die(bool delay = false)
        {
            print("Die");
            isAlive = false;
            if (delay)
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            //anim.gameObject.SetActive(false);
            moveParticle.Stop();
            GameOn = false;
            if (!delay)
                GameManager.instance.LevelComplete();
            else
                StartCoroutine(DelayFunc());
        }
        IEnumerator DelayFunc()
        {
            yield return new WaitForSeconds(1.2f);
            if (GameManager.instance != null)
                GameManager.instance.LevelComplete();
        }
        IEnumerator DelayFunc1(float delay)
        {
            yield return new WaitForSeconds(delay);
            print("Die");
            isAlive = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            //anim.gameObject.SetActive(false);
            moveParticle.Stop();
            GameOn = false;
            GameManager.instance.LevelComplete();
        }

        public void Revive()
        {
            print("Revive");
            CanJump = true;
            //  this.CancelInvoke();
            myFloor.ResetFloor();
            Vector3 pos = myFloor.transform.position;
            transform.position = new Vector3(pos.x, pos.y + 0.3f, pos.z);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            anim.gameObject.SetActive(true);
            isAlive = true;
            Invoke(nameof(Invinsible), 1);
        }
        void Invinsible()
        {
            GameOn = true;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + (Vector3.down * groundCheckDistance));
            Gizmos.DrawLine(groundCheck2.position, groundCheck2.position + (Vector3.down * groundCheckDistance));
            Gizmos.DrawLine(wallCheckRight.position, wallCheckRight.position + (transform.right * wallCheckDistance));
            Gizmos.DrawLine(wallCheckLeft.position, wallCheckLeft.position + (-transform.right * wallCheckDistance));

        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isAlive)
                return;
            if (collision.CompareTag("Floor"))
            {
                myFloor = collision.GetComponent<FloorHandler>();
                if (!myFloor.first)
                    OnFloor?.Invoke(myFloor.GetXStatus());
                myFloor.PlayerOnFloor();
            }
            else if (collision.CompareTag("bounds"))
            {
                AudioManager.instance.Play("die", 0.3f);
                isAlive = false;
                StartCoroutine(DelayFunc1(0.5f));
                //  Invoke(nameof(Die), 0.5f);
            }
            else if (collision.CompareTag("obstacle"))
            {
                DieNow();
            }
        }
        public void DieNow()
        {
            if (!isAlive)
                return;
            audioSrc.PlayOneShot(hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)]);
            blood.Play();
            Die(true);
            anim.SetBool("run", true);
            anim.Play("Au_DeathAnim");
        }
    }
}