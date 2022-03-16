using ChangeableDatabase;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MainAdmin : MonoBehaviour
{
    [SerializeField] private FieldAdmin field;
    private Cursor cursor;

    private bool isUpdate;

    private async void Start()
    {
        field.GameInit();
        cursor = field.GetCursor();

        await UniTask.DelayFrame(120);
        SceneChanger.Instance.FadeOut();
        Sound.Instance.PlayField(SystemSettings.IndexFieldBGM);
        await UniTask.DelayFrame(10);

        await SceneChanger.Instance.GetReady();

        isUpdate = true;
    }

    private void Update()
    {
        if (!isUpdate)
        {
            cursor.CursorUpdate(0);
            return;
        }

        if (field.GameUpdate())
        {
            isUpdate = false;
            Gameover();
        }
    }

    private async void Gameover()
    {
        Sound.Instance.Stop();
        CDB.FieldInfo = field.GetFieldInfo();
        await SceneChanger.Instance.Gameover();
        await UniTask.DelayFrame(30);
        Sound.Instance.Play(SE.Gameover);

        SceneChanger.Instance.SceneChange(Scene.ResultScene, FadeType.Failed);
    }
}