namespace CheetahExam.WebUI.Client.Pages.Organisations;

public partial class Index
{
    public List<OrganisationDto> Items { get; set; } = new()
    {
        new(){ Name = "LpInfotech", ContactNumber ="000-000-0000", Email="LpInfotech123@gmail.com"},
        new(){ Name = "MortgageHouse", ContactNumber ="111-111-1111", Email="MortgageHouse123@gmail.com"},
        new(){ Name = "CheetahExam", ContactNumber ="222-222-2222", Email="CheetahExam123@gmail.com"},
    };

    public class OrganisationDto
    {
        public string? Name { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
    }
}
