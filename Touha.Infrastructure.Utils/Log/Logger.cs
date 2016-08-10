using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Touha.Infrastructure.Utils.Log
{
    public class Logger
    {
        #region 字段
        //用于存放写日志任务的队列
        private Queue<Action> _queue;

        //用于写日志的线程
        private Thread _loggingThread;

        //用于通知是否有新日志要写的信号量
        private ManualResetEvent _hasNew;
        #endregion 

        #region 单例模式
        //构造函数，初始化
        private Logger()
        {
            _queue = new Queue<Action>();
            _hasNew = new ManualResetEvent(false);

            _loggingThread = new Thread(Process);
            _loggingThread.IsBackground = true;
            _loggingThread.Start();
        }

        private static readonly Logger _logger = new Logger();

        public static Logger GetInstance()
        {
            return _logger;
        }
        #endregion

        //处理队列中的任务
        private void Process()
        {
            while (true)
            {
                //等待接收信号量，阻塞线程
                _hasNew.WaitOne();

                //接收信号量后，重置信号量，关闭信号量
                _hasNew.Reset();

                //由于队列中的任务可能在急速的增加，这里等待是为了一次能处理更多的任务，减少对队列的频繁”进出“请求
                Thread.Sleep(100);

                //开始执行队列中的任务
                //由于过程中可能还有新的任务，所以不能直接对原来的_queue进行操作
                //先将_queue中的任务复制一份后将其清空，然后的那份拷贝进行处理
                Queue<Action> queueCopy;
                lock (_queue)
                {
                    queueCopy = new Queue<Action>(_queue);
                    _queue.Clear();
                }

                foreach (var action in queueCopy)
                {
                    Console.WriteLine(Thread.CurrentThread.Name + ":" + DateTime.Now);
                    //for (int i = 0; i < 1000; i++)
                        action();
                    Console.WriteLine(Thread.CurrentThread.Name + ":" + DateTime.Now);
                }
            }
        }

        private void WriteLog(string content)
        {
            lock (_queue)
            {
                //将任务加到队列
                _queue.Enqueue(() => File.AppendAllText("log.txt", content + DateTime.Now + Environment.NewLine));
            }
            //打开信号
            _hasNew.Set();
        }

        //公开一个Write方法，共外部调用
        public static void Write(string content)
        {
            Task.Run(() => GetInstance().WriteLog(content));
        }
    }
}
