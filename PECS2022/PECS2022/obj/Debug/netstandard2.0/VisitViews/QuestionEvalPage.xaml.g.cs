//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::Xamarin.Forms.Xaml.XamlResourceIdAttribute("PECS2022.VisitViews.QuestionEvalPage.xaml", "VisitViews/QuestionEvalPage.xaml", typeof(global::PECS2022.VisitViews.QuestionEvalPage))]

namespace PECS2022.VisitViews {
    
    
    [global::Xamarin.Forms.Xaml.XamlFilePathAttribute("VisitViews\\QuestionEvalPage.xaml")]
    public partial class QuestionEvalPage : global::Xamarin.Forms.ContentPage {
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.Entry txtQCode;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::PECS2022.CustomEditor txtComments;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Syncfusion.SfDataGrid.XForms.SfDataGrid dataGrid;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private void InitializeComponent() {
            global::Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(QuestionEvalPage));
            txtQCode = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.Entry>(this, "txtQCode");
            txtComments = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::PECS2022.CustomEditor>(this, "txtComments");
            dataGrid = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Syncfusion.SfDataGrid.XForms.SfDataGrid>(this, "dataGrid");
        }
    }
}
