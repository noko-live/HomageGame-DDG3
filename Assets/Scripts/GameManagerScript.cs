using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance;

    public GameObject bulletPrefab;
    public Transform bulletSpawnLocation;
  
    float baseBulletSpeed = 7f;
    float fastBulletSpeed = 10f;
    float slowBulletSpeed = 4f;
    
    
    float spawnRangeSize = 15f;

    int playerHP;

    GameObject player;

    bool isPlayerDead = false;
    public TMP_Text deathText;
    public TMP_Text playerHP_txt;

    [Header("Bullet Pattern Variables")]
    public float bulletWaveGapSize = 1f;




    private void Awake()
    {
        GameManagerScript.Instance = this;
    }

    private void Start()
    {
        player = PlayerMoveScript.Instance.gameObject;
        playerHP = 3;
    }

    private void Update()
    {
        if (playerHP <= 0 && !isPlayerDead)
        {
            isPlayerDead = true;
            player.SetActive(false);
            deathText.gameObject.SetActive(true);
        }


        playerHP_txt.text = "HP: " + playerHP.ToString();

    }

    public void RespawnPlayer()
    {
        deathText.gameObject.SetActive(false);
        playerHP = 3;
        isPlayerDead = false;

        player.transform.position = new Vector3(0, 1, 10);

        player.SetActive(true);
    }

    public void BulletHit()
    {
        playerHP -= 1;
    }


    #region Bullet / Patterns

    void ShootBullet(Vector3 S_location, float S_rotation, float S_speed)
    {
        Quaternion newRotation = Quaternion.Euler(new Vector3(0f, S_rotation, 0f));

        GameObject bullet = Instantiate(bulletPrefab, S_location, newRotation, GameObject.FindGameObjectWithTag("WorldObjectHolder").transform);

        bullet.GetComponent<Rigidbody>().AddForce(-bullet.transform.forward * S_speed, ForceMode.Impulse);
    }


    void ShootRandomBullet(float R_speed)
    {
        float randomX = Random.Range(-spawnRangeSize, spawnRangeSize);


        ShootBullet(NewBulletX_Pos(randomX), 0f, R_speed);
    }

    void ShootBulletWave(float W_speed, bool isGap)
    {
        float gap = Mathf.Round(Random.Range(-spawnRangeSize, spawnRangeSize));
        Debug.Log("Gap size: " + (gap + -bulletWaveGapSize) + " " + gap +" " + (gap + bulletWaveGapSize));

        for(float i = -spawnRangeSize; i <= spawnRangeSize; i++)
        {
            if (isGap)
            {
                if (i < gap - bulletWaveGapSize || i > gap + bulletWaveGapSize)
                {
                    ShootBullet(NewBulletX_Pos(i), 0f, W_speed);
                }
            }
            else
            {
                ShootBullet(NewBulletX_Pos(i), 0f, W_speed);
            }
        }
    }


    [ContextMenu("Spray")]
    public void Spray()
    {
        ShootBulletSpray(bulletSpawnLocation.transform.position.x, slowBulletSpeed, 15, 5f);
    }

    void ShootBulletSpray(float SP_location,float SP_speed, int SP_bulletamount, float SP_conesize)
    {
        float angleP = SP_conesize / (SP_bulletamount - 1);
        float startAngle = -SP_conesize / 2f;


        for(int i = 0; i <= SP_bulletamount; i++)
        {
            float currentAngle = startAngle + (angleP * i);
            ShootBullet(NewBulletX_Pos(SP_location), currentAngle * i, SP_speed);

        }
    }





    Vector3 NewBulletX_Pos(float newX)
    {
        if (newX < spawnRangeSize && newX > -spawnRangeSize)
        {
            Vector3 newLoc = new Vector3(newX, bulletSpawnLocation.position.y, bulletSpawnLocation.position.z);
            return newLoc;
        }
        else
        {
            Debug.Log("Bullet pos out of range");
            return Vector3.forward;
        }
    }


    #endregion



    #region Bullet Sequences
    void BulletSequence_1()
    {
        StartCoroutine(_BulletSequence_1());
    }
    IEnumerator _BulletSequence_1()
    {
        float timeBetweenShots = 1f;

        int repeat = 4;
        int count = 0;

        while(count <= repeat)
        { 
            ShootBulletWave(baseBulletSpeed, true);
            yield return new WaitForSeconds(timeBetweenShots);
            count += 1;
        }

        yield return new WaitForSeconds(1.5f);

        repeat = 3;
        count = 0;

        while (count <= repeat)
        {
            ShootBulletWave(fastBulletSpeed, false);
            yield return new WaitForSeconds(timeBetweenShots + 1f);
            count += 1;
        }



        yield return new WaitForSeconds(0f);
    }


    void BulletSequence_2()
    {
        StartCoroutine(_BulletSequence_2());
    }
    IEnumerator _BulletSequence_2()
    {
        float timeBetweenShots = 1f;

        int repeat = 10;
        int count = 0;

        while (count <= repeat)
        {
            ShootBulletSpray(NewBulletX_Pos(12f).x, fastBulletSpeed, 25, 12f);
            yield return new WaitForSeconds(timeBetweenShots + 1f);
            ShootBulletSpray(NewBulletX_Pos(-12f).x, fastBulletSpeed, 25, -12f);
            yield return new WaitForSeconds(timeBetweenShots + 1f);

            count += 1;

        }
        yield return new WaitForSeconds(0f);
    }


    #endregion


    #region Buttons
    
    public void SprayLeftButton()
    {
        ShootBulletSpray(NewBulletX_Pos(-12f).x, fastBulletSpeed, 25, -12f);
    }
    public void SprayRightButton()
    {
        ShootBulletSpray(NewBulletX_Pos(12f).x, fastBulletSpeed, 25, 12f);
    }
    public void SequenceButton()
    {
        StartCoroutine(_BulletSequence_1());
    }
    public void WaveButton()
    {
        ShootBulletWave(baseBulletSpeed, true);
    }

    #endregion

}
