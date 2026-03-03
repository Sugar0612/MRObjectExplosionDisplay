using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG;

public class NPCPath : MonoBehaviour
{
    public Transform StartPoint;

    public Transform PausePoint;

    public Transform EndPoint;

    public GameObject NPC;

    void Start()
    {
        StartPath();
    }

    private void StartPath()
    {
        StartCoroutine(StartPathCoroutine());
    }

    private IEnumerator StartPathCoroutine()
    {
        // NPC TODO..
        Vector3 startPoint = new Vector3(StartPoint.position.x, NPC.transform.position.y, StartPoint.position.z);
        Vector3 pausePoint = new Vector3(PausePoint.position.x, NPC.transform.position.y, PausePoint.position.z);
        Vector3 endPoint = new Vector3(EndPoint.position.x, NPC.transform.position.y, EndPoint.position.z);

        NPC.gameObject.transform.DOMove(startPoint, 0.0f).SetEase(Ease.Linear);

        NPC.gameObject.transform.DOMove(pausePoint, 3.0f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(3.0f);

        Debug.Log("旋转角度");

        NPC.gameObject.transform.DOLocalRotate(new Vector3(0, -35.0f, 0), 0f); // 2秒内绕Y轴旋转到90度

        Debug.Log("说话阶段");

        yield return new WaitForSeconds(3.0f);

        NPC.gameObject.transform.DOLocalRotate(new Vector3(0, 53.0f, 0), 0f); // 2秒内绕Y轴旋转到90度
        NPC.gameObject.transform.DOMove(endPoint, 3.0f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(3.0f);

        Debug.Log("结束");

        NPC.gameObject.SetActive(false);
    }
}
