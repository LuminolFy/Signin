using GreenOnions.Interface;

namespace Signin
{
    /// <summary>
    /// 葱葱插件实现类
    /// </summary>
    public class GreenOnionsPlugin : IPlugin
    {
        /// <summary>
        /// 发送好友消息的方法(好友QQ号, 消息体, 返回消息Id)
        /// </summary>
        private Func<long, IGreenOnionsMessages, Task<int>> SendFriendMessage;
        /// <summary>
        /// 发送群消息的方法(群号, 消息体, 返回消息Id)
        /// </summary>
        private Func<long, IGreenOnionsMessages, Task<int>> SendGroupMessage;
        /// <summary>
        /// 发送临时消息的方法(好友QQ号, 群号, 消息体, 返回消息Id)
        /// </summary>
        private Func<long, long, IGreenOnionsMessages, Task<int>> SendTempMessage;

        /// <summary>
        /// 插件被葱葱加载时触发
        /// </summary>
        public void OnLoad()
        {

        }

        /// <summary>
        /// 葱葱连接到机器人平台时触发
        /// </summary>
        /// <param name="selfId">连接的QQ号</param>
        public void OnConnected(long selfId, Func<long, IGreenOnionsMessages, Task<int>> SendFriendMessage, Func<long, IGreenOnionsMessages, Task<int>> SendGroupMessage, Func<long, long, IGreenOnionsMessages, Task<int>> SendTempMessage)
        {
            this.SendFriendMessage = SendFriendMessage;
            this.SendGroupMessage = SendGroupMessage;
            this.SendTempMessage = SendTempMessage;
        }

        /// <summary>
        /// 葱葱从机器人平台主动断开时触发
        /// </summary>
        public void OnDisconnected()
        {
        
        }

        /// <summary>
        /// 葱葱收到消息(且没被葱葱自身或其他插件处理时触发)
        /// </summary>
        /// <param name="msgs">消息体</param>
        /// <param name="senderGroup">发送的群, 如果是好友消息则为null</param>
        /// <param name="Response">回复方法(消息体)</param>
        /// <returns></returns>
        public bool OnMessage(IGreenOnionsMessages msgs, long? senderGroup, Action<IGreenOnionsMessages> Response)
        {
            if (msgs.Count > 0 && msgs[0] is IGreenOnionsTextMessage textMsg)  //判断传入的消息如果是文本消息
            {
                if (textMsg.Text == "/签到")  //判断消息内容
                {
                    string signinMsg = SigninHandler.Signin(msgs.SenderId);  //调用签到主方法
                    Response(new Messages { new TextMessage(signinMsg) });  //回复签到响应消息
                    return true;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// 文字消息类型
    /// </summary>
    public class TextMessage : IGreenOnionsTextMessage
    {
        /// <summary>
        /// 消息文本内容
        /// </summary>
        public string Text { get; set; }
        public TextMessage(string text)
        {
            Text = text;
        }
    }

    /// <summary>
    /// 消息链类型, 用于拼接多种类型的消息(在一个消息气泡中发送)
    /// </summary>
    public class Messages : List<TextMessage>, IGreenOnionsMessages
    {
        public bool Reply {get;set;}
        public int RevokeTime { get; set; }
        public long SenderId { get; set; }
        public string SenderName { get; set; }
    }
}
