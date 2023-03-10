using System;
using UnityEngine;

public class Tab_Base: MonoBehaviour , ITabOperation
{
    public CanvasGroup CanvasGroup { get; private set; }

    public ITabOperation This { get; set; }
    public ITabOperation OperationHandler { get; set; }

    protected Transform _subTabUpper, _subTabBottom;

    protected Btn _btnBack, _btnForward;

    protected TabTransition _tabTransition;

    protected TabLoading _tabLoading;

    protected IReset[] _iResets;

    public event Action onGoBack;
    public event Action onGoForward;
    public event Action onTabOpen;
    public event Action onTabClose;



    protected virtual void Awake()
    {
        CanvasGroup = Get<CanvasGroup>.From(gameObject);

        This = this;

        _subTabUpper = transform.Find("SubTab_Upper");

        _subTabBottom = transform.Find("SubTab_Bottom");

        _btnBack = _subTabUpper != null ? _btnBack = Get<Btn>.From(_subTabUpper.Find("Btn_Back").gameObject) : null;

        _btnForward = _subTabBottom != null ? _btnForward = Get<Btn>.From(_subTabBottom.Find("Btn_Forward").gameObject) : null;

        _tabTransition = Get<TabTransition>.FromChild(gameObject);

        _tabLoading = Get<TabLoading>.FromChild(gameObject);

        _iResets = GetComponentsInChildren<IReset>();
    }

    protected virtual void OnEnable()
    {
        if (_btnBack != null)
            _btnBack.onSelect += GoBack;

        if (_btnForward != null)
            _btnForward.onSelect += GoForward;

        TabsOperation.Handler.onOperationSubmitted += OnOperationSubmitted;
    }

    protected virtual void OnDisable()
    {
        if (_btnBack != null)
            _btnBack.onSelect -= GoBack;

        if (_btnForward != null)
            _btnForward.onSelect -= GoForward;

        TabsOperation.Handler.onOperationSubmitted -= OnOperationSubmitted;
    }

    protected virtual void OnOperationSubmitted(ITabOperation handler, TabsOperation.Operation operation, object[] data)
    {
        
    }

    public virtual void OpenTab()
    {
        ResetTab();

        MenuTabs.Activity(CanvasGroup);

        onTabOpen?.Invoke();
    }

    public virtual void CloseTab()
    {
        GlobalFunctions.CanvasGroupActivity(CanvasGroup, false);

        onTabClose?.Invoke();
    }

    protected virtual void GoBack()
    {
        onGoBack?.Invoke();
    }

    protected virtual void GoForward()
    {
        onGoForward?.Invoke();
    }

    protected virtual void OpenLoadingTab() => _tabLoading.Open();

    protected virtual void ResetTab()
    {
        GlobalFunctions.Loop<IReset>.Foreach(_iResets, iReset => { iReset.SetDefault(); });
    }

    public virtual void OnOperationSucceded()
    {
        
    }

    public virtual void OnOperationFailed()
    {
        
    }
}
