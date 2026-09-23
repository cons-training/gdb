using gdb.Application.Services.Implementations;

namespace gdb.Application.Services
{
	public class AccountServiceFactory
	{
		public static AccountService Create()
		{
			return new AccountService();
		}
	}
}