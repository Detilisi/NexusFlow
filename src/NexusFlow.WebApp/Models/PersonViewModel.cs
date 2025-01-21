namespace NexusFlow.WebApp.Models
{
    public class PersonViewModel
    {
        public int Code { get; set; } = 0;
        public string IdNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;   
        public string Surname { get; set; } = string.Empty;
        public List<AccountViewModel> Accounts { get; set; } = [];
    }
}
