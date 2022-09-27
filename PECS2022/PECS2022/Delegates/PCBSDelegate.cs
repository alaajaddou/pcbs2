using PECS2022.Models;

namespace PECS2022
{
    public delegate void onLoginSuccess(User user);
    public delegate void OnSaveSuccess();
    public delegate void OnSaveFailure();
    public delegate void OnDeleteSuccess(object sender);
    public delegate void OnDeleteFailure();

    public delegate void OnRestoreSuccess();
    public delegate void OnRestoreFailure();

    public delegate void CurrentItemChanged();

    public delegate void SaveAndClose(object data);

    public delegate void OnItemSelected<T>(T data);

    public delegate void OnSetToScreenComplete();

    public delegate void WizardDisplayTypeChanged(bool detailVisible);

}