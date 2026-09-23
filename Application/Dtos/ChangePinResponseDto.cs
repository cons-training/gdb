using GDB.App.Domain.Enums;

namespace GDB.App.Application.Dtos
{
    public class ChangePinResponseDto
    {
        public string AccountNumber { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}