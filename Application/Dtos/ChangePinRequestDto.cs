namespace GDB.App.Application.Dtos
{
    public class ChangePinRequestDto
    {
        public string AccountNumber { get; set; }
        public string OldPin { get; set; }
        public string NewPin { get; set; }
    }
}