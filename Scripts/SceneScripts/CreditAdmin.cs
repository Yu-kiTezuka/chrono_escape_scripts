using ChangeableDatabase;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class CreditAdmin : MonoBehaviour
{
    [SerializeField] private GameObject blocker;
    [SerializeField] private Text tCredit;
    [SerializeField] private Text tPage;
    [SerializeField] private Image iLogo;

    private List<string> creditTexts;
    private int pageIndex;

    private async void Awake()
    {
        blocker.SetActive(true);

        creditTexts = JsonUtility.FromJson<JCreditTexts>(Resources.Load("System/CreditTexts").ToString()).texts;

        creditTexts.Add($"{SecretCode1()}{SecretCode2()}");
        Refresh();

        SceneChanger.Instance.FadeOut();
        Sound.Instance.Play(SE.ShutterOpen);
        Sound.Instance.Play(BGM.Credit);
        await UniTask.DelayFrame(30);
        blocker.SetActive(false);
    }

    public void ChangeIndex(bool isUp)
    {
        pageIndex += isUp ? 1 : -1;
        pageIndex = Mathf.Clamp(pageIndex, 0, creditTexts.Count - 1);
        Sound.Instance.Play(SE.SelectSub);

        Refresh();
    }

    public void Extra()
    {
        SystemSettings.Save();
        Sound.Instance.Stop();
        Sound.Instance.Play(SE.ShutterClose);
        blocker.SetActive(true);
        SceneChanger.Instance.SceneChange(Scene.ExtraScene);
    }

    private string SecretCode1()
    {
        string code = "\n";
        {
            code += "= SECRET CODE #1:OD =\n";

            int pass = ArchiveDataBase.ArchiveData.best_level;
            if (pass >= 45)
            {
                code += "-E- + -B- + -START-\n";
            }
            else if (pass >= 35)
            {
                code += "-69- + -66- + -START-\n";
                code += $"-Lv.3 UNLOCK = {pass}/45-\n";
            }
            else if (pass >= 25)
            {
                code += "-???- + -???- + -START-\n";
                code += $"-Lv.2 UNLOCK = {pass}/35-\n";
            }
            else
            {
                code += $"-Lv.1 UNLOCK = {pass}/25-\n";
            }
        }
        return code;
    }

    private string SecretCode2()
    {
        string code = "\n";
        {
            code += "= SECRET CODE #2:BO =\n";

            int pass = ArchiveDataBase.ArchiveData.best_playtime_sec;
            if (pass >= 600)
            {
                code += "-7- + -3- + -START-\n";
            }
            else if (pass >= 420)
            {
                code += "-RUBY- + -AQUA- + -START-\n";
                code += $"-Lv.3 UNLOCK = {pass / 60}:{pass % 60}/10:00-\n";
            }
            else if (pass >= 300)
            {
                code += "-???- + -???- + -START-\n";
                code += $"-Lv.2 UNLOCK = {pass / 60}:{pass % 60}/7:00-\n";
            }
            else
            {
                code += $"-Lv.1 UNLOCK = {pass / 60}:{pass % 60}/5:00-\n";
            }
        }
        return code;
    }

    private void Refresh()
    {
        tCredit.text = $"{creditTexts[pageIndex]}";
        tPage.text = $"{pageIndex + 1} / {creditTexts.Count}";
        iLogo.enabled = pageIndex == creditTexts.Count - 2;
    }

    [Serializable]
    public sealed class JCreditTexts
    {
        public List<string> texts;
    }
}