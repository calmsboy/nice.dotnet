using Grpc.Net.Client;
using GrpcService1;
using Hardware.Info;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json.Serialization;

namespace Nice.Dotnet.ConsoleApp;

public partial class Program
{
    /// <summary>
    /// 固定磁盘
    /// </summary>
    public const string FixedDisk = "Fixed hard disk media";
    /// <summary>
    /// 扩展磁盘(移动硬盘)
    /// </summary>
    public const string ExternalDisk = "External hard disk media";
    /// <summary>
    /// 移动优盘
    /// </summary>
    public const string RemovableDisk = "Removable Media";

    static readonly IHardwareInfo hardwareInfo = new HardwareInfo();
    static async Task Main(string[] args)
    {
        await GetGreetDataAsync();


        var taskes = new List<Task>()
        {
            DoStep(1000,1),
            DoStep(500,2),
            DoStep(200,3)
        };
        // 等待所有步骤完成
        //await Task.WhenAll(taskes);
        //Console.WriteLine("所有步骤已完成.");
        //Parallel.ForEachAsync(taskes, async (current,token) => { 
        //    await current;
        //    Console.WriteLine("步骤完成.");
        //}).GetAwaiter();
        Console.WriteLine("所有并发步骤已完成.");
        //await IpTestParse();
        //using var channel = GrpcChannel.ForAddress("https://localhost:7000");
        //var client = new NiceService.NiceServiceClient(channel);
        //int count = 0;
        //while (count <= 1000)
        //{
        //  var response = await client.SayHelloAsync(new HelloRequest { Name = "Client" });
        //  Console.WriteLine(response);
        //  count++;
        //}

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();

        //var Client = new
    }
    static async Task GetGreetDataAsync(CancellationToken token =default)
    {
        var channel = GrpcChannel.ForAddress("https://localhost:5600");
        var client = new Greeter.GreeterClient(channel);
        var result =await client.SayHelloAsync(new HelloRequest() { Name="Dotnet"},cancellationToken: token);
        Console.WriteLine($"请求结果:{JsonConvert.SerializeObject(result)}");
    }
    static async Task GetUseDiskAsync()
    {
        // 创建ManagementObjectSearcher对象
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"Select * From Win32_PhysicalMedia");

        // 遍历所有的USB设备
        foreach (ManagementObject disk in searcher.Get())
        {
            Console.WriteLine("USB Device Serial Number: " + disk["SerialNumber"].ToString());//disk["SerialNumber"].ToString()
        }
        await Task.CompletedTask;
    }
    static async Task DoStep(int time,int index)
    {
        await Task.Delay(time); // 模拟耗时操作
        Console.WriteLine($"步骤{index}完成.");
    }

    public static Task GetExternalDiskAsync(string[]? diskType = null)
    {
        ManagementClass mgtCls = new ManagementClass("Win32_DiskDrive");
        var disks = mgtCls.GetInstances();
        bool condition;
        foreach (ManagementObject mo in disks.Cast<ManagementObject>())
        {
            condition = mo.Properties["MediaType"].Value != null;
            diskType ??= new string[] { ExternalDisk, RemovableDisk };
            condition = condition && diskType.Contains(mo.Properties["MediaType"].Value);
            if (condition)
            {
                GetDiskInformation(mo);
            }
        }
        return Task.CompletedTask;
    }
    /// <summary>
    /// 获取硬盘详细信息
    /// </summary>
    /// <param name="mo"></param>
    private static Task GetDiskInformation(ManagementObject mo)
    {
        foreach (ManagementObject diskPartition in mo.GetRelated("Win32_DiskPartition").Cast<ManagementObject>())
        {
            foreach (ManagementBaseObject disk in diskPartition.GetRelated("Win32_LogicalDisk"))
            {
                var diskInfo = new DiskDeviceInfo()
                {
                    Name = disk.Properties["Name"].Value.ToString() ?? "",
                    VolumeName = disk.Properties["VolumeName"].Value.ToString() ?? "本地磁盘",
                    Caption = mo.Properties["Caption"].Value.ToString() ?? "",
                    Size = mo.Properties["Size"].Value.ToString() ?? "",
                    PNPDeviceID = mo.Properties["PNPDeviceID"].Value.ToString() ?? "",
                    SerialNumber = mo.Properties["SerialNumber"].Value.ToString() ?? "",
                };
                if (string.IsNullOrEmpty(diskInfo.VolumeName))
                {
                    diskInfo.VolumeName = "本地磁盘";
                }
                var properties = disk.Properties;
                //foreach (var property in properties)
                //{
                //    Console.WriteLine($"{property.Name}:{property.Value}");
                //}
                Console.WriteLine($"设备根路径：{diskInfo.RootPath}");
                Console.WriteLine($"产品名称：{diskInfo.Caption}");
                Console.WriteLine($"卷标：{diskInfo.VolumeName}");
                Console.WriteLine($"总容量：{diskInfo.Size}");
                Console.WriteLine($"序列号：{diskInfo.SerialNumber}");
                Console.WriteLine($"PNPDeviceID：{diskInfo.PNPDeviceID}\n");
            }
        }
        return Task.CompletedTask;
    }


    static async Task IpTestParse()
    {
        var ipv6 = "::1";
        Console.WriteLine($"IPV6:{IPAddress.Parse(ipv6)}");
        await Task.CompletedTask;
    }
    static async Task GoTest()
    {
        var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        // Store references to the tasks so that we can wait on them and
        // observe their status after cancellation.
        Task t;
        var tasks = new ConcurrentBag<Task>();

        Console.WriteLine("Press any key to begin tasks...");
        Console.ReadKey(true);
        Console.WriteLine("To terminate the example, press 'c' to cancel and exit...");
        Console.WriteLine();

        // Request cancellation of a single task when the token source is canceled.
        // Pass the token to the user delegate, and also to the task so it can
        // handle the exception correctly.
        t = Task.Run(() => DoSomeWork(1, token), token);
        Console.WriteLine("Task {0} executing", t.Id);
        tasks.Add(t);

        // Request cancellation of a task and its children. Note the token is passed
        // to (1) the user delegate and (2) as the second argument to Task.Run, so
        // that the task instance can correctly handle the OperationCanceledException.
        t = Task.Run(() =>
        {
            // Create some cancelable child tasks.
            Task tc;
            for (int i = 3; i <= 10; i++)
            {
                // For each child task, pass the same token
                // to each user delegate and to Task.Run.
                tc = Task.Run(() => DoSomeWork(i, token), token);
                Console.WriteLine("Task {0} executing", tc.Id);
                tasks.Add(tc);
                // Pass the same token again to do work on the parent task.
                // All will be signaled by the call to tokenSource.Cancel below.
                DoSomeWork(2, token);
            }
        }, token);

        Console.WriteLine("Task {0} executing", t.Id);
        tasks.Add(t);

        // Request cancellation from the UI thread.
        char ch = Console.ReadKey().KeyChar;
        if (ch == 'c' || ch == 'C')
        {
            tokenSource.Cancel();
            Console.WriteLine("\nTask cancellation requested.");

            // Optional: Observe the change in the Status property on the task.
            // It is not necessary to wait on tasks that have canceled. However,
            // if you do wait, you must enclose the call in a try-catch block to
            // catch the TaskCanceledExceptions that are thrown. If you do
            // not wait, no exception is thrown if the token that was passed to the
            // Task.Run method is the same token that requested the cancellation.
        }

        try
        {
            await Task.WhenAll(tasks.ToArray());
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($"\n{nameof(OperationCanceledException)} thrown\n");
        }
        finally
        {
            tokenSource.Dispose();
        }

        // Display status of all tasks.
        foreach (var task in tasks)
            Console.WriteLine("Task {0} status is now {1}", task.Id, task.Status);
    }
    static void DoSomeWork(int taskNum, CancellationToken ct)
    {
        // Was cancellation already requested?
        if (ct.IsCancellationRequested)
        {
            Console.WriteLine("Task {0} was cancelled before it got started.",
                              taskNum);
            return;
        }

        int maxIterations = 100;

        // NOTE!!! A "TaskCanceledException was unhandled
        // by user code" error will be raised here if "Just My Code"
        // is enabled on your computer. On Express editions JMC is
        // enabled and cannot be disabled. The exception is benign.
        // Just press F5 to continue executing your code.
        for (int i = 0; i <= maxIterations; i++)
        {
            // Do a bit of work. Not too much.
            var sw = new SpinWait();
            for (int j = 0; j <= 100; j++)
                sw.SpinOnce();

            if (ct.IsCancellationRequested)
            {
                Console.WriteLine("Task {0} cancelled", taskNum);
                //ct.ThrowIfCancellationRequested();
                break;
            }
        }
    }
    public static async Task Go()
    {
        //hardwareInfo.RefreshOperatingSystem();
        //hardwareInfo.RefreshMemoryStatus();
        //hardwareInfo.RefreshBatteryList();
        //hardwareInfo.RefreshBIOSList();
        //hardwareInfo.RefreshCPUList();
        //hardwareInfo.RefreshDriveList();
        //hardwareInfo.RefreshKeyboardList();
        //hardwareInfo.RefreshMemoryList();
        //hardwareInfo.RefreshMonitorList();
        //hardwareInfo.RefreshMotherboardList();
        //hardwareInfo.RefreshMouseList();
        //hardwareInfo.RefreshNetworkAdapterList();
        //hardwareInfo.RefreshPrinterList();
        //hardwareInfo.RefreshSoundDeviceList();
        //hardwareInfo.RefreshVideoControllerList();

        hardwareInfo.RefreshAll();

        Console.WriteLine(hardwareInfo.OperatingSystem);

        Console.WriteLine(hardwareInfo.MemoryStatus);

        foreach (var hardware in hardwareInfo.BatteryList)
            Console.WriteLine(hardware);

        foreach (var hardware in hardwareInfo.BiosList)
            Console.WriteLine(hardware);

        foreach (var cpu in hardwareInfo.CpuList)
        {
            Console.WriteLine(cpu);

            foreach (var cpuCore in cpu.CpuCoreList)
                Console.WriteLine(cpuCore);
        }

        Console.ReadLine();

        foreach (var drive in hardwareInfo.DriveList)
        {
            Console.WriteLine(drive);

            foreach (var partition in drive.PartitionList)
            {
                Console.WriteLine(partition);

                foreach (var volume in partition.VolumeList)
                    Console.WriteLine(volume);
            }
        }

        Console.ReadLine();

        foreach (var hardware in hardwareInfo.KeyboardList)
            Console.WriteLine(hardware);

        foreach (var hardware in hardwareInfo.MemoryList)
            Console.WriteLine(hardware);

        foreach (var hardware in hardwareInfo.MonitorList)
            Console.WriteLine(hardware);

        foreach (var hardware in hardwareInfo.MotherboardList)
            Console.WriteLine(hardware);

        foreach (var hardware in hardwareInfo.MouseList)
            Console.WriteLine(hardware);

        foreach (var hardware in hardwareInfo.NetworkAdapterList)
            Console.WriteLine(hardware);

        foreach (var hardware in hardwareInfo.PrinterList)
            Console.WriteLine(hardware);

        foreach (var hardware in hardwareInfo.SoundDeviceList)
            Console.WriteLine(hardware);

        foreach (var hardware in hardwareInfo.VideoControllerList)
            Console.WriteLine(hardware);

        Console.ReadLine();

        foreach (var address in HardwareInfo.GetLocalIPv4Addresses(NetworkInterfaceType.Ethernet, OperationalStatus.Up))
            Console.WriteLine(address);

        Console.WriteLine();

        foreach (var address in HardwareInfo.GetLocalIPv4Addresses(NetworkInterfaceType.Wireless80211))
            Console.WriteLine(address);

        Console.WriteLine();

        foreach (var address in HardwareInfo.GetLocalIPv4Addresses(OperationalStatus.Up))
            Console.WriteLine(address);

        Console.WriteLine();

        foreach (var address in HardwareInfo.GetLocalIPv4Addresses())
            Console.WriteLine(address);

        Console.ReadLine();
        //double timeoutSeconds = 6;//超时时间 秒
        //int maxRetryCount = 2;//最大重试次数
        //CancellationTokenSource cts = new CancellationTokenSource();
        //bool isSuccess = false;
        //string result = string.Empty;
        ////1.超时取消任务 无返回值
        ////isSuccess = await TaskExtensions.TimeoutCancelAsync((cts) => DoActionNoResult(cts), timeoutSeconds, cts);

        ////1.超时取消任务 无返回值
        ////(isSuccess, result) = await TaskExtensions.TimeoutCancelAsync((cts) => DoActionWithResult(cts), timeoutSeconds, cts);

        ////3.超时取消并重试任务 无返回值
        ////isSuccess = await TaskExtensions.TimeoutRetryAsync((cts) => DoActionNoResult(cts), timeoutSeconds, maxRetryCount, cts);

        ////4.超时取消并重试任务 带返回值任务
        //(isSuccess, result) = await TaskExtensions.TimeoutRetryAsync((cts) => DoActionWithResult(cts), timeoutSeconds, maxRetryCount, cts);

        //if (isSuccess)
        //{
        //    Console.WriteLine("任务执行成功，结果：" + result);
        //}
        //else
        //{
        //    Console.WriteLine("任务执行失败！");
        //}
        //Console.ReadLine();
    }

    public static async Task DoActionNoResult(CancellationTokenSource cts)
    {
        await Task.Delay(200);
        for (int i = 1; i <= 5; i++)
        {
            if (cts.IsCancellationRequested)//在业务任务每个耗时的操作开始之前判断取消令牌是否已取消
                return;
            Console.WriteLine($"num:{i}");
            await Task.Delay(1000);//模拟业务操作，耗时任务。
        }
    }

    public static async Task<string> DoActionWithResult(CancellationTokenSource cts)
    {
        await Task.Delay(200);
        for (int i = 1; i <= 5; i++)
        {
            if (cts.IsCancellationRequested)//在业务任务每个耗时的操作开始之前判断取消令牌是否已取消
                return "";
            Console.WriteLine($"num:{i}");
            await Task.Delay(1000);//模拟业务操作，耗时任务。
        }
        return "666";
    }
    /// <summary>
    /// 监控U盘状态
    /// </summary>
    public static async Task WatchUDataChange()
    {
       var uDiskWatcher = BuildWatchUDataChange(new EventArrivedEventHandler(UDataChangeHandle));
        //开启监控
        uDiskWatcher.Start();
    }
    private static void UDataChangeHandle(object sender, EventArrivedEventArgs e)
    {
        Debug.WriteLine("U盘状态变更...");
        GetExternalDiskAsync().GetAwaiter().GetResult();
    }
    /// <summary>
    /// 监控U盘插入和移除事件
    /// </summary>
    /// <param name="arrivedEventHandler"></param>
    /// <returns></returns>
    public static ManagementEventWatcher BuildWatchUDataChange(EventArrivedEventHandler eventHandle)
    {
        // 创建一个WMI查询，用于监控设备插入和移除事件
        WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2 or EventType = 3");

        // 创建一个管理对象，用于执行WMI查询
        ManagementEventWatcher watcher = new(query);

        // 设置事件处理程序
        watcher.EventArrived -= new EventArrivedEventHandler(eventHandle);
        watcher.EventArrived += new EventArrivedEventHandler(eventHandle);

        return watcher;
    }
}