using Mirai.CSharp.Builders;
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
    [RegisterMiraiHttpParser(typeof(DefaultMappableMiraiHttpMessageParser<IGroupMessageEventArgs, GroupMessageEventArgs>))]
    public class GroupMessage : IMiraiHttpMessageHandler<IGroupMessageEventArgs>
    {
        public async Task HandleMessageAsync(IMiraiHttpSession session, IGroupMessageEventArgs e)
        {
            if (e.Chain.Length == 2)
            {
                int quoteId = (e.Chain[0] as SourceMessage).Id;
                if (e.Chain[1] is PlainMessage plainMsg)
                {
                    if (plainMsg.Message == "/签到")
                    {
                        string signText = SigninHandler.Signin(e.Sender.Id);
                        _ = session.SendGroupMessageAsync(e.Sender.Group.Id, new[] { new PlainMessage(signText) }, quoteId);
                    }
                }
            }
        }
    }
}
