using CheetahExam.Application.Common.Exceptions;
using CheetahExam.Application.Companies.Commands.Create;
using CheetahExam.Application.Companies.Commands.Delete;
using CheetahExam.Application.Companies.Commands.Update;
using CheetahExam.Application.Companies.Queries.GetAllCompanies;
using CheetahExam.Application.Companies.Queries.GetCompanyWithId;
using CheetahExam.Application.Companies.Queries.GetCompanyWithUserId;
using CheetahExam.WebUI.Shared.Common;
using CheetahExam.WebUI.Shared.Common.Models.Companies;
using CheetahExam.WebUI.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheetahExam.WebUI.Server.Controllers.Company;

[Authorize(Roles = $"{Constant.UserRoles.Administrator},{Constant.UserRoles.SuperAdmin}")]
[Route("/")]
public class CompanyController : ApiControllerBase
{
    [HttpGet("company/getAll")]
    public async Task<CompanyCollectionDto> GetAll(bool isActive)
    {
        return await Mediator.Send(new GetAllCompaniesQuery() { ISActive = isActive });
    }

    [HttpGet("company/getById/{id}")]
    public async Task<CompanyDto> GetById(string id)
    {
        return await Mediator.Send(new GetCompanyWithIdQuery() { CompanyId = id });
    }

    [HttpPost("company/create")]
    public async Task<Result> Create(CompanyDto company)
    {
        return await Mediator.Send(new CreateCompanyCommand() { Company = company });
    }

    [HttpPut("company/update/{companyUniqueId}")]
    public async Task<Result> Update(string companyUniqueId, CompanyDto company)
    {
        return companyUniqueId != company.UniqueId
            ? Result.Failure(new List<string>() { "Bad Request" })
            : await Mediator.Send(new UpdateCompanyCommand() { Company = company });
    }

    [HttpDelete("company/delete/{companyUniqueId}")]
    public async Task<Result> Delete(string companyUniqueId)
    {
        return await Mediator.Send(new DeleteCompanyCommand { UniqueId = companyUniqueId });
    }

    [HttpGet("company/getByUserId/{userUniqueId}")]
    public async Task<CompanyDto> GetByUserId(string userUniqueId)
    {
        return await Mediator.Send(new GetCompanyWithUserIdQuery() { UserId = userUniqueId });
    }
}
