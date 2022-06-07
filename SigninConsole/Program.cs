using SigninConsole.CqHttp;
using SigninConsole.MiraiApiHttp;

namespace SigninConsole
{
    public static class Program
	{
		public static async Task Main()
		{
			Console.WriteLine("请选择连接平台: 0 = mirai-api-http,  1 = cqhttp");
		ILRetryProtocol:;
			if (!int.TryParse(Console.ReadLine(), out int protocol) || protocol < 0 || protocol > 1)
			{
				Console.WriteLine("选择的平台不正确, 请重新选择。");
				goto ILRetryProtocol;
			}

			Console.WriteLine("请输入机器人QQ号:");
		ILRetryQQ:;
			if (!long.TryParse(Console.ReadLine(), out long qqId))
			{
				Console.WriteLine("输入的QQ号不正确，请重新输入。");
				goto ILRetryQQ;
			}

			Console.WriteLine("请输入连接 IP:");
			string ip = Console.ReadLine();

			Console.WriteLine("请输入连接端口:");
		ILReadPort:;
			if (!ushort.TryParse(Console.ReadLine(), out ushort port) || port < 0 || port > 65535)
			{
				Console.WriteLine("输入的端口号不正确，请重新输入:");
				goto ILReadPort;
			}

			if (protocol == 0)
			{
				Console.WriteLine("请输入mirai-api-http verifyKey:");
				string verifyKey = Console.ReadLine();
				await MiraiApiHttpMain.Connect(qqId, ip, port, verifyKey, (bConnect, nickNameOrErrorMessage) => Connecting(bConnect, nickNameOrErrorMessage, "mirai-api-http"));
			}
			else
			{
				Console.WriteLine("请输入cqhttp access-token:");
				string accessToken = Console.ReadLine();
				await CqHttpMain.Connect(qqId, ip, port, accessToken, (bConnect, nickNameOrErrorMessage) => Connecting(bConnect, nickNameOrErrorMessage, "cqhttp"));
			}
		}

		public static void Connecting(bool bConnect, string nickNameOrErrorMessage, string protocol)
		{
			if (bConnect)
				Console.WriteLine($"连接状态: 已连接到{protocol}, 登录昵称:{nickNameOrErrorMessage}");
			else if (nickNameOrErrorMessage == null)  //连接失败且没有异常
				Console.WriteLine($"连接失败，请检查{protocol}是否已经正常启动并已配置IP端口相关参数, 以及机器人QQ是否成功登录。");
			else  //发生异常
				if (nickNameOrErrorMessage.Length > 0)
				Console.WriteLine("连接失败，" + nickNameOrErrorMessage);
		}
	}
}
