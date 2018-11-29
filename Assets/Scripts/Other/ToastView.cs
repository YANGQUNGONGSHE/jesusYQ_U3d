using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ToastView : MonoBehaviour
{
    private RichText _mContent;
    private CanvasGroup _mCv;

    private void Awake()
    {
        _mCv = transform.GetComponent<CanvasGroup>();
        _mCv.alpha = 0;
        _mContent = transform.Find("Content/Text").GetComponent<RichText>();
    }

    public void SetToast(string icon, string text, ToastPos pos, float stayTime)
    {
        if (string.IsNullOrEmpty(icon))
        {
            _mContent.text = text;
        }
        else
        {
            _mContent.text = string.Format("<icon={0}> {1}", icon, text);
        }
        Alert(pos, stayTime);
    }

    private void Alert(ToastPos pos, float stayTime)
    {
        _mCv.alpha = 0;
        transform.localPosition = new Vector2(0, (int)pos);
        _mCv.DOFade(1, .3f).OnComplete(() =>
        {
            StartCoroutine(Release(stayTime));
        });
    }

    IEnumerator Release(float time)
    {
        yield return new WaitForSeconds(time);
        _mCv.DOFade(0, .3f).OnComplete(() =>
        {
            Object.Destroy(this.gameObject);
        });
    }
}

public enum ToastPos
{
    Upper = 400,
    Mid = 0,
    Lower = -400,
}
