using UnityEngine;

public interface IHoldInteractable
{
    void OnHoldStart(PlayerInteractor interactor);
    void OnHold(PlayerInteractor interactor);
    void OnHoldEnd(PlayerInteractor interactor);
}
