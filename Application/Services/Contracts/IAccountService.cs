using gdb.Domain.Models;

namespace gdb.Application.Services

{
	public interface IAccountService
	{
		IAccount GetAccount(string accountNumber);
	}
}