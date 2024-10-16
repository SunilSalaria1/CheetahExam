using CheetahExam.WebUI.Shared.Common.Models.GeneralLookUps;
using CheetahExam.WebUI.Shared.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.RegularExpressions;

namespace CheetahExam.WebUI.Client.Pages.GeneralLookUps;

public partial class Edit
{
    #region Render UI

    [Parameter]
    public GeneralLookUpDto GeneralLookUp { get; set; } = null!;

    [Parameter]
    public bool ISCreate { get; set; }

    [CascadingParameter]
    public MudDialogInstance MudDialog { get; set; } = null!;

    #endregion

    #region Services

    #region Properties

    [Inject] private IGeneralLookUpsClient GeneralLookUpsClient { get; set; } = null!;

    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #endregion

    #region Fields

    private MudForm Form = null!;

    private GeneralLookUpsFluentValidator GeneralLookUpsFluentValidation = new GeneralLookUpsFluentValidator();

    #endregion

    #region Methods

    private async Task Submit()
    {
        string commandsReturnStatus;
        string errors = string.Empty;

        await Form.Validate();

        if (!Form.IsValid)
        {
            return;
        }

        var result = ISCreate
                    ? await GeneralLookUpsClient.CreateAsync(GeneralLookUp)
                    : await GeneralLookUpsClient.UpdateAsync(GeneralLookUp);

        if (result.Errors.Any())
        {
            commandsReturnStatus = Constant.CommandsReturnStatus.Exception;
            errors = string.Join(',', result.Errors);
        }
        else
        {
            commandsReturnStatus = ISCreate ? Constant.CommandsReturnStatus.Created : Constant.CommandsReturnStatus.Updated;
        }

        HandleCommandsReturnStatus(commandsReturnStatus, errors);

    }

    private void HandleCommandsReturnStatus(string commandsReturnStatus, string errors)
    {
        switch (commandsReturnStatus)
        {
            case "Created":
                Snackbar.Add("Form Successfully Submitted!", MudBlazor.Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
                break;

            case "Updated":
                Snackbar.Add("Form Successfully Updated!", MudBlazor.Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
                break;

            case "Exception":
                Snackbar.Add(errors, MudBlazor.Severity.Error);
                break;

            default:
                break;
        }
    }

    private void Cancel() => MudDialog.Cancel();

    private string ExtractHeader(string type)
    {
        if (string.IsNullOrEmpty(type))
        {
            return "DefaultHeader";
        }
        // Replace underscores with spaces, and then replace multiple spaces with a single space
        return Regex.Replace(type.Replace('_', ' '), "([a-z])([A-Z])", "$1 $2");
    }

    #endregion

    #region Validation

    public class GeneralLookUpsFluentValidator : AbstractValidator<GeneralLookUpDto>
    {
        public GeneralLookUpsFluentValidator()
        {
            RuleFor(x => x.Value)
                .NotEmpty()
                .Length(1, 100).WithName("Name");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<GeneralLookUpDto>.CreateWithOptions((GeneralLookUpDto)model, x => x.IncludeProperties(propertyName)));

            return result.IsValid
                ? (IEnumerable<string>)Array.Empty<string>()
                : result.Errors.Select(e => e.ErrorMessage);
        };
    }

    #endregion
}
