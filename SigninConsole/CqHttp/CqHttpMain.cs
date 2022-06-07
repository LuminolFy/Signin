using Sora;
using Sora.Entities.Base;
using Sora.Entities.Info;
using Sora.Interfaces;
using Sora.Net.Config;
using Sora.Util;
using YukariToolBox.LightLog;

namespace SigninConsole.CqHttp
{
    public class CqHttpMain
    {
        public static async Task Connect(long qqId, string ip, ushort port, string authKey, Action<bool, string> ConnectedEvent)
        {
            try
            {
                //实例化Sora服务
                ISoraService service = SoraServiceFactory.CreateService(new ClientConfig() { Host = ip, Port = port, AccessToken = authKey });
                service.Event.OnGroupMessage += MessageEvents.Event_OnGroupMessage;
                service.Event.OnPrivateMessage += MessageEvents.Event_OnPrivateMessage;

                //启动服务并捕捉错误
                await service.StartService().RunCatch(e => Log.Error("Sora Service", Log.ErrorLogBuilder(e)));

                service.Event.OnClientConnect += async (eventType, eventArgs) =>
                {
                    SoraApi api = service.GetApi(service.ServiceId);

                    List<FriendInfo> IFriendInfos = (await api.GetFriendList()).friendList;
                    string nickname = "未知";

                    var self = IFriendInfos.Where(q => q.UserId == qqId).FirstOrDefault();
                    if (self != null)
                        nickname = self.Nick;

                    ConnectedEvent?.Invoke(true, nickname);
                };

                while (true)
                {
                    if (Console.ReadLine() == "exit")
                    {
                        ConnectedEvent?.Invoke(false, "");
                        break;
                    }
                    Task.Delay(100).Wait();
                }
            }
            catch (Exception ex)
            {
                ConnectedEvent(false, ex.Message);
            }
        }
    }
}