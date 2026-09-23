using GDB.App.Domain.Enums;

namespace GDB.App.Application.Dtos
{
    public class ViewAllAccountsResponseDto
    {
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }

        public AccountType AccountType { get; set; }

        public int Age { get; set; }

        public AccountStatus AccountStatus { get; set; }
        public AccountPrivilege AccountPrivilege { get; set; }
    }
}
