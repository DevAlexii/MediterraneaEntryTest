using System.Collections;
using UnityEngine;

interface ICollectable
{
    public void OnCollected();
}
public class ItemBase : MonoBehaviour,ICollectable
{
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float floatingSpeed;
    private float angle = -1;
    private Vector3 startPos;

    public void OnCollected()
    {
        StartCoroutine(OnCollectedEvent());
    }

    IEnumerator OnCollectedEvent()
    {
        AudioManager.Instance.PlayClipSound(ClipType.Collected);
        GameManager.Instance.UpdateScore(Random.Range(5, 10));
        GetComponent<Collider>().enabled = false;
        GameManager.Instance.IncrementCollectedItems();
        GameUIManager.Instance.DeactiveInteractionPopUP();

        float timer = 1;
        while (timer > 0)
        {
            timer -= Time.deltaTime * 4f;
            transform.localScale = Vector3.one * .2f * timer;
            yield return null;
        }
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        startPos = transform.position;
        GetComponent<Collider>().enabled = true;
        transform.localScale = Vector3.one * .2f;
    }

    private void Update()
    {
        transform.eulerAngles += new Vector3(0, rotationSpeed * Time.deltaTime, 0);

        angle += Time.deltaTime * floatingSpeed;
        if (angle >= 360)
        {
            angle = 0;
        }
        transform.position = new Vector3(startPos.x, startPos.y + Mathf.Sin(angle) * .05f, startPos.z);
    }
}
