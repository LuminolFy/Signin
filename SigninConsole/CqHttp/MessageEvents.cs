using Signin;
using Sora.Entities;
using Sora.Entities.Segment;
using Sora.Entities.Segment.DataModel;
using Sora.EventArgs.SoraEvent;

namespace SigninConsole.CqHttp
{
    public static class MessageEvents
    {
        public static async ValueTask Event_OnGroupMessage(string eventType, GroupMessageEventArgs eventArgs)
        {
            int quoteId = eventArgs.Message.MessageId;
            if (eventArgs.Message.MessageBody[0].Data is TextSegment textSegment)
            {
                if (textSegment.Content == "/签到")
                {
                    string signText = SigninHandler.Signin(eventArgs.Sender.Id);
                    MessageBody cqHttpMessages = new MessageBody();
                    cqHttpMessages.Add(SoraSegment.Reply(quoteId));  //在开头添加回复消息, 使得消息以"回复"方式发送
                    cqHttpMessages.Add(SoraSegment.Text(signText));  //签到响应文本
                    _ = eventArgs.SoraApi.SendGroupMessage(eventArgs.SourceGroup.Id, cqHttpMessages);
                }
            }
        }

        public static async ValueTask Event_OnPrivateMessage(string eventType, PrivateMessageEventArgs eventArgs)
        {
            if (eventArgs.Message.MessageBody[0].Data is TextSegment textSegment)
            {
                if (textSegment.Content == "/签到")
                {
                    string signText = SigninHandler.Signin(eventArgs.Sender.Id);  //签到响应文本
                    _ = eventArgs.SoraApi.SendPrivateMessage(eventArgs.Sender.Id, SoraSegment.Text(signText));  //签到响应文本
                }
            }
        }
    }
}
