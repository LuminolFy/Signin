using Mirai.CSharp.HttpApi.Handlers;
using Mirai.CSharp.HttpApi.Models.ChatMessages;
using Mirai.CSharp.HttpApi.Models.EventArgs;
using Mirai.CSharp.HttpApi.Parsers;
using Mirai.CSharp.HttpApi.Parsers.Attributes;
using Mirai.CSharp.HttpApi.Session;
using Mirai.CSharp.Models;
using Signin;

namespace SigninConsole.MiraiApiHttp
{
    [RegisterMiraiHttpParser(typeof(DefaultMappableMiraiHttpMessageParser<IFriendMessageEventArgs, FriendMessageEventArgs>))]
    public partial class FriendMessage : IMiraiHttpMessageHandler<IFriendMessageEventArgs>
    {
        public async Task HandleMessageAsync(IMiraiHttpSession session, IFriendMessageEventArgs e)
        {
            if (e.Chain.Length == 2)

            {
                int quoteId = (e.Chain[0] as SourceMessage).Id;
                if (e.Chain[1] is PlainMessage plainMsg)
                {
                    if (plainMsg.Message == "/签到")
                    {
                        string signText = SigninHandler.Signin(e.Sender.Id);
                        _ = session.SendFriendMessageAsync(e.Sender.Id, new[] { new PlainMessage(signText) }, quoteId);
                    }
                }
            }
        }
    }
}
