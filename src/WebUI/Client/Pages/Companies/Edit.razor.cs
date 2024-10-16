using CheetahExam.WebUI.Shared.Common;
using CheetahExam.WebUI.Shared.Common.Models;
using CheetahExam.WebUI.Shared.Common.Models.Companies;
using CheetahExam.WebUI.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace CheetahExam.WebUI.Client.Pages.Companies;

public partial class Edit
{
    #region Fields

    #region Services

    [Inject] ISnackbar Snackbar { get; set; } = null!;

    [Inject] ICompanyClient CompanyClient { get; set; } = null!;

    [Inject] IFilesClient FilesClient { get; set; } = null!;

    [Inject] NavigationManager NavigationManager { get; set; } = null!;

    #endregion

    #region Parameter

    [Parameter] public string? CompanyId { get; set; }

    #endregion

    MudForm Form = null!;

    string? SelectedImage { get; set; }

    string? PreviousImageUrl { get; set; }

    CompanyDto Company { get; set; } = new();

    FileDetailDto? FileDetail { get; set; }

    #endregion

    #region Method

    protected async override Task OnInitializedAsync()
    {
        if (!string.IsNullOrWhiteSpace(CompanyId))
        {
            Company = await CompanyClient.GetByIdAsync(CompanyId);

            if (Company.Media is not null)
            {
                if (Company.Media.Url is not null)
                {
                    PreviousImageUrl = Company.Media.Url;
                    SelectedImage = PreviousImageUrl.Replace(Constant.FileDirectory.CompanyImage, "").Substring(0, 17) + "...";
                }
            }
        }
    }

    private async Task Submit()
    {
        await Form.Validate();

        if (FileDetail is not null)
        {
            if (FileDetail.MetaData.FileType is Constant.FileTypes.JPEG or Constant.FileTypes.PNG or Constant.FileTypes.GIF or Constant.FileTypes.JPG)
            {
                MediaDto mediaDto = await FilesClient.UploadAsync(FileDetail);

                Company.Media = mediaDto;
            }
        }

        if (Form.IsValid)
        {
            Result result;

            if (string.IsNullOrWhiteSpace(CompanyId))
            {
                result = await CompanyClient.CreateAsync(Company);
            }
            else
            {
                if (Company.Media is not null)
                {
                    if (Company.Media.Url != PreviousImageUrl && PreviousImageUrl is not null)
                    {
                        await FilesClient.DeleteAsync(PreviousImageUrl);
                    }
                }

                result = await CompanyClient.UpdateAsync(CompanyId, Company);
            }

            string snackBarMessage;

            Severity severity;

            if (result.Errors.Any())
            {
                snackBarMessage = string.Join(',', result.Errors);
                severity = Severity.Warning;
            }
            else
            {
                snackBarMessage = string.IsNullOrWhiteSpace(CompanyId) ? "Created" : "Updated";
                severity = Severity.Success;
                Back();
            }

            Snackbar.Add(snackBarMessage, severity);
        }
    }

    private void Back() => NavigationManager.NavigateTo($"/companies/");

    private async Task UploadImage(InputFileChangeEventArgs e)
    {
        FileDetail = new();

        var buffer = new byte[e.File.Size];

        await e.File.OpenReadStream(maxAllowedSize: 10000 * 10000).ReadAsync(buffer);

        FileDetail.MetaData.FileName = e.File.Name;
        FileDetail.MetaData.FileSize = e.File.Size;
        FileDetail.MetaData.FileType = e.File.ContentType;
        FileDetail.MetaData.FileBytes = buffer;

        FileDetail.Path = Constant.FileDirectory.CompanyImage;

        SelectedImage = (e.File.Name.Length > 20) ? e.File.Name.Substring(0, 17) + "..." : e.File.Name;
    }

    private void ClearImage()
    {
        FileDetail = null;
        SelectedImage = null;
        Company.Media = null;
    }

    #endregion
}
