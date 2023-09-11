using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{

    public float speed = 5f;
    public float jumpForce = 5f;
    public float bulletSpeed = 20f;

    private Rigidbody rb;
    //public GameObject bulletPrefab;
    //public Transform bulletSpawn;

    private Vector2 moveInput;

    //object pooling
    public GameObject bullet;
    public int poolSize = 20;
    public Transform firePoint;
    private List<GameObject> objectPool;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        objectPool = new List<GameObject>();

        for(int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bullet);
            obj.SetActive(false);
            objectPool.Add(obj);
        }

    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            rb.AddForce(Vector3.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
        }
    }

    public void OnShoot(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Shoot();
        }
    }
    
    public GameObject GetBullet()
    {
        for(int i = 0; i < objectPool.Count; i++)
        {
            if (!objectPool[i].activeInHierarchy)
            {
                objectPool[i].SetActive(true);
                return objectPool[i];
            }
        }

        return null;
    }

    public void Shoot()
    {

        GameObject bullet = GetBullet();

        if(bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;

            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.velocity = bullet.transform.forward * bulletSpeed;

            StartCoroutine(ReturnBulletToPool(bullet));
        }
        
        /*GameObject Firebullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.velocity = Firebullet.transform.forward * bulletSpeed;

        Destroy(bullet, 3);*/
    }

    IEnumerator ReturnBulletToPool(GameObject bullet)
    {
        yield return new WaitForSeconds(3);

        if(bullet != null)
        {
            bullet.SetActive(false);
        }
    }

}
