using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace streaming
{
    public class StreamingHub : Hub
    {

        public void SendStreamInit()
        {
            //�����ͻ��˶���
            Clients.All.InvokeAsync("streamStarted");
        }

        //�����ĵ���Ϣ
        public IObservable<string> StartStreaming()
        {
            return Observable.Create(
                async (IObserver<string> observer) =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        observer.OnNext($"��������......{i}");
                        await Task.Delay(1000);
                    }
                });
        }
    }
}
