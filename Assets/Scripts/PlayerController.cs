using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerController : MonoBehaviour
{
    [SerializeField] NavMeshAgent playerAgent;
    [SerializeField] float playerSpeed = 10;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject rightHand; //Для определения места запуска снаряда
    Animator playerAnimator;
    float fireRate = 0.5f;
    float canFire;
    SpawnManager sm;
    Vector3 movePoint;
    bool readyToShoot = false;
    float bulletSpeed = 20;
    float bulletPower = 20;
    void Start()
    {
        playerAgent = GetComponent<NavMeshAgent>();
        playerAnimator = GetComponent<Animator>();
        canFire = Time.time;
        sm = GameObject.Find("SpawnController").GetComponentInChildren<SpawnManager>();
    }

    void Update()
    {
        if(Input.GetMouseButton(0) && readyToShoot && canFire < Time.time)
        {
            canFire = Time.time + fireRate;

            Ray shootRay = sm.activeCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitTarget;
            

            if (Physics.Raycast(shootRay, out hitTarget, 1000))
            {
                transform.LookAt(hitTarget.point);
                playerAnimator.SetBool("IsShooting", true);
                Debug.DrawRay(transform.position, hitTarget.point, Color.red);
                if (hitTarget.collider.tag == "Zombie")
                {
                    Vector3 push = (hitTarget.collider.gameObject.transform.position - transform.position);
                    hitTarget.collider.gameObject.GetComponentInParent<ZombieController>().GotHitted();
                    hitTarget.collider.gameObject.GetComponent<Rigidbody>().AddForce(push * bulletPower, ForceMode.Impulse);
                }
                //Запуск снарядов на выстрел.
                //Vector3 dirToTarget = (hitTarget.point - rightHand.transform.position).normalized;
                //GameObject shot = Instantiate(bullet, rightHand.transform.position, bullet.transform.rotation);
                //shot.GetComponent<Rigidbody>().velocity = dirToTarget * bulletSpeed;
                //shot.transform.rotation = Quaternion.LookRotation(dirToTarget + new Vector3(90, 0, 0));

            }
        }

        if(readyToShoot && Input.GetMouseButtonUp(0))
        {
            playerAnimator.SetBool("IsShooting", false);
        }

        //Перемещение персонажа
        if (Input.GetMouseButtonDown(0) && !readyToShoot)
        {
            Ray ray = sm.activeCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;

            if (Physics.Raycast(ray, out hitData, 1000))
            {
                movePoint = hitData.point;
                playerAgent.SetDestination(movePoint);
                playerAgent.speed = playerSpeed;
           
            }
        }

        //Управление анимациями
        if (Vector3.Distance(transform.position, movePoint) > 0.5f)
        {
            playerAnimator.SetBool("IsRunning", true);
        }

        if (Vector3.Distance(transform.position, movePoint) <= 0.5f)
        {
            playerAnimator.SetBool("IsRunning", false);
        }
    }
    //Изменение режима игры при входе на огневую точку
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("shootPoint"))
        {
            playerAnimator.SetBool("IsGunPlaying", true);
            playerAnimator.SetBool("IsRunning", false);
            playerAgent.ResetPath();
            readyToShoot = true;
            Debug.Log("Aim trigger");
            sm.CameraSwitch(true);
        }
    }
}
