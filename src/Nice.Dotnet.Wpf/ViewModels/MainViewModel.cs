using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Nice.Dotnet.Core.IServices;
using Nice.Dotnet.Core.Services;
using Nice.Dotnet.Core.ViewModels;
using Nice.Dotnet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Nice.Dotnet.Wpf.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private readonly ChatConnectService _chatConnectService;

    public MainViewModel(ICustomInfoClientService customInfoService
        ,ChatConnectService chatConnectService) : base(customInfoService)
    {
         _chatConnectService = chatConnectService;
         _chatConnectService.BuildConnection("WPF").GetAwaiter();
        _chatConnectService.Watch((online) => { }, (user,msg) =>
        {
            MessageCollect.Add($"{user}\n 发送消息：{msg} \n {DateTime.Now.ToLocalTime()}");
        });
    }
    
    [RelayCommand]
    async Task ReadFile()
    {
        Title = "Hello DotNet";
        var dialog = new Microsoft.Win32.OpenFileDialog() 
        {
            FileName = "data",
            DefaultExt = ".json",
            Filter = "Json documents (.json)|*.json"
        };

        bool? result = dialog.ShowDialog();
        if (result == true)
        {
            await ReadFileStreamHander(dialog.FileName);
        }
    }
    [RelayCommand]
    async Task SendMessage()
    {
        _chatConnectService.SendMessage(Message);
    }

    private async Task ReadFileStreamHander(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return;
        }
        using var fileStream = File.OpenRead(fileName);
        using (var stream = new StreamReader(fileStream))
        {
            var raw = await stream.ReadToEndAsync();
            AppendData(AnalysisFileContent(raw));
        }
    }
    /// <summary>
    /// 解析文件内容
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private List<CustomInfoModel> AnalysisFileContent(string text) 
        => JsonConvert.DeserializeObject<List<CustomInfoModel>>(text)??new List<CustomInfoModel>();

    private void AppendData(List<CustomInfoModel> models)
    {
        CustomInfos.Clear();
        models.ForEach(model =>
        {
            CustomInfos.Add(model);
        });
    }

    
}
