using GDB.App.Domain.Enums;

namespace GDB.App.Application.Dtos
{
    public class CloseAccountResponseDto
    {
        public string AccountNumber { get; set; }
        public AccountStatus Status { get; set; }
        public string Message { get; set; }
    }
}
