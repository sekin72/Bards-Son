using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ChangeCurrencyGoldUI : MonoBehaviour
{
    private double _tempCurrencyGold;
    [SerializeField] private Text _tempCurrencyGoldText;
    [Inject] private UserDataManager _userDataManager;
    private Tween _animationTween;
    private Sequence _animationSequence;

    private void Start()
    {
        _tempCurrencyGold = _userDataManager.UserData.CurrencyGold;
        _tempCurrencyGoldText.text = _userDataManager.UserData.CurrencyGold.ToString();
        _userDataManager.OnCurrencyGoldChanged = ChangeCurrencyGoldTextUI;
    }

    public void ChangeCurrencyGoldTextUI()
    {
        if (_animationTween != null)
        {
            _animationTween.Kill();
        }

        if (_animationSequence != null)
        {
            _animationSequence.Kill();
        }

        //_animationTween = DOVirtual.Float((float)_tempCurrencyGold, (float)_userDataManager.UserData.CurrencyGold, 0.5f, value =>
        //  {
        //      _tempCurrencyGoldText.text = value.ToString();
        //      _tempCurrencyGold = value;
        //  });

        _animationSequence = DOTween.Sequence();
        _animationSequence.Join(DOVirtual.Float((float)_tempCurrencyGold, (float)_userDataManager.UserData.CurrencyGold, 0.5f, value =>
        {
            _tempCurrencyGoldText.text = value.ToString();
            _tempCurrencyGold = value;
        }));
    }

}
