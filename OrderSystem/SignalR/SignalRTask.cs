using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
 



namespace SignalR
{
    
   public class SignalRTask
    {
           
       SignalR.MyHub MyHub = new SignalR.MyHub();

     // static  Thread t = new Thread(new ThreadStart(CheckOnlin));


       public void StartCheck() {
           SignalRTask obj = new SignalRTask();
           //Thread thread = new Thread(new ThreadStart(obj.CheckOnlin));
           //thread.Start();
           //Task T = new Task(CheckOnlin);
           //T.Start();
           System.Timers.Timer t = new System.Timers.Timer(15000);//实例化Timer类，设置时间间隔
           t.Elapsed += new System.Timers.ElapsedEventHandler(obj.CheckOnlin);//到达时间的时候执行事件
           t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)
           t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件
         
           
       }
       public void CheckOnlin(object source, System.Timers.ElapsedEventArgs e)
       {
           
           new MyHub().sendMsgForCheck();
          // Thread.CurrentThread.Join(10000);
           

     
       }


    }
}
