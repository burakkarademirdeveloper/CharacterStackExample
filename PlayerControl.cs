using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class PlayerControl : MonoBehaviour
{
    public GameObject MainCharacter;
    public float Speed = 0.2f;

    [Header("-----Stack Objects-----")] 
    public  List<GameObject> SpawnObjects;
    [SerializeField]
    public  List<GameObject> CharacterArray;
    public  GameObject PrefabChracater;
    public GameObject DestroyParticleEffect;
    public  int i = -1;
    

    [Header("------ Touch-----")]
    public float limitX = 0.7f;
    private float _currentSpeed;
    public float xSpeed;


    [Header("------ Character Rotate-----")]
    public Transform targetTransform;
    public float targetAngle;
    public float duration;
    
    void Start()
    {
        _currentSpeed = Speed;
        CharacterArray = new List<GameObject>();
        CharacterArray.Add(Character);
    }

    void Update()
    {
        float newx = 0;
        float touchXDelta = 0;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            touchXDelta = Input.GetTouch(0).deltaPosition.x / Screen.width;
        }
        else if (Input.GetMouseButton(0))
        {
            touchXDelta = Input.GetAxis("Mouse X");
        }

        newx = xSpeed * touchXDelta * Time.deltaTime;
        newx = Mathf.Clamp(newx, -limitX, limitX);

        Quaternion targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        Vector3 movement = targetRotation * new Vector3(newx, 0, -_currentSpeed * Time.deltaTime);
        transform.position += movement;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "+20")
        {
            for (int j = 0; j < 20; j++)
            {
                i++;
                GameObject newCharacter = Instantiate(PrefabChracater,SpawnObjects[i].transform.position, Quaternion.identity);
                CharacterArray.Add(newCharacter);
                newCharacter.transform.parent = SpawnObjects[i].transform;
            }
            Destroy(other.gameObject);
        }

        if (other.tag == "-10")
        {
            for (int j = 0; j < 10; j++)
            {
                Destroy(CharacterArray[CharacterArray.Count-1]);
                Instantiate(DestroyParticleEffect, CharacterArray[CharacterArray.Count-1].transform.position, Quaternion.identity);
                CharacterArray.RemoveAt(CharacterArray.Count-1);
                i--;
            }
            Destroy(other.gameObject);
        }

        if (other.tag == "x2")
        {
            int count = CharacterArray.Count;
            for (int j = 0; j < count; j++)
            {
                i++;
                GameObject newCharacter = Instantiate(PrefabChracater,SpawnObjects[i].transform.position, Quaternion.identity);
                CharacterArray.Add(newCharacter);
                newCharacter.transform.parent = SpawnObjects[i].transform;
            }
            Destroy(other.gameObject);
        }

        if (other.tag == "Rotate")
        {
            RotateToTargetAngle();
        }

        if (other.tag == "+1")
        {
            i++;
            GameObject newCharacter = Instantiate(PrefabChracater,SpawnObjects[i].transform.position, Quaternion.identity);
            newCharacter.transform.Rotate(0,-90,0);
            CharacterArray.Add(newCharacter);
            newCharacter.transform.parent = SpawnObjects[i].transform;
            Destroy(other.gameObject);
        }
        
    }

    public void PlusOne()
    {
        i++;
        GameObject newCharacter = Instantiate(PrefabChracater,SpawnObjects[i].transform.position, Quaternion.identity);
        newCharacter.transform.Rotate(0,-90,0);
        CharacterArray.Add(newCharacter);
        newCharacter.transform.parent = SpawnObjects[i].transform;
    }
    private void RotateToTargetAngle()
    {
        transform.DORotate(new Vector3(0f, targetAngle, 0f), duration)
            .SetEase(Ease.OutSine);
    }
}
