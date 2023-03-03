using GreenOnions.Interface;
using GreenOnions.Interface.Configs;

namespace Signin
{
    /// <summary>
    /// 葱葱插件实现类
    /// </summary>
    public class GreenOnionsPlugin : IMessagePlugin
    {
        private string? _pluginPath;
        public string Name => "签到";

        public string Description => "签到插件";

        public bool DisplayedInTheHelp => false;

        public GreenOnionsMessages? HelpMessage => string.Empty;

        public void ConsoleSetting()
        {
        }

        public void OnConnected(long selfId, IGreenOnionsApi api)
        {
        }

        public void OnDisconnected()
        {
        }

        public void OnLoad(string pluginPath, IBotConfig config)
        {
            _pluginPath = pluginPath;
        }

        public bool OnMessage(GreenOnionsMessages msgs, long? senderGroup, Action<GreenOnionsMessages> Response)
        {
            if (msgs.Count > 0 && msgs[0] is GreenOnionsTextMessage textMsg)  //判断传入的消息如果是文本消息
            {
                if (textMsg.Text == "/签到")  //判断消息内容
                {
                    string signinMsg = SigninHandler.Signin(_pluginPath!, msgs.SenderId);  //调用签到主方法
                    Response(signinMsg);  //回复签到响应消息
                    return true;
                }
            }
            return false;
        }

        public bool WindowSetting()
        {
            return false;
        }
    }
}
