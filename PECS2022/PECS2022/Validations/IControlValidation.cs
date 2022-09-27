namespace PECS2022
{
    public interface IControlValidation
    {
        bool HasError { get;  }
        string ErrorMessage { get; }
        bool ShowErrorMessage { get; set; }
    }
}
