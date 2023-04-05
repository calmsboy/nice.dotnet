using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nice.Dotnet.Core.IServices;
using Nice.Dotnet.Core.Models;
using Nice.Dotnet.Domain.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Nice.Dotnet.Core.ViewModels
{
    public partial class BaseViewModel:ObservableObject
    {
        private readonly ICustomInfoClientService _customInfoService;

        public BaseViewModel(ICustomInfoClientService customInfoService)
        {
            customInfos = new() { 
                new CustomInfoModel{  Id ="1" , Name = "H1",City="hand1"}
            };
            _customInfoService = customInfoService;

        }

        List<CustomInfoModel> CreateSeedDate(int count)
        {
            var entities = new List<CustomInfoModel>();
            foreach (var i in Enumerable.Range(1,count))
            {
                entities.Add(new CustomInfoModel 
                { 
                    Id = $"{i}",
                    Name= $"Hi{i}",
                    City = $"Hand{i}",
                });
            }
            return entities;
        }


        [ObservableProperty]
        string title = "WPF示例";
        /// <summary>
        /// 用户信息列表
        /// </summary>
        [ObservableProperty]
        ObservableCollection<CustomInfoModel> customInfos;
        /// <summary>
        /// 信息
        /// </summary>
        [ObservableProperty]
        string message;


        

        /// <summary>
        /// 从API加载数据
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task LoadData()
        {
            Title = $"Hello DotNet:{DateTime.Now}";
            var entities=  await _customInfoService.GetCustomInfos();
            CustomInfos.Clear();
            if (entities.Any())
            {
                
                foreach (var entity in entities)
                {
                    CustomInfos.Add(entity);
                }
            }
        }
        [RelayCommand]
        async Task PostData()
        {
            await _customInfoService.PostCustomEntities(CustomInfos.ToList());
        }
        [RelayCommand]
        async Task ClearData()
        {
            await _customInfoService.ClearData();
        }
        /// <summary>
        /// 清理数据
        /// </summary>
        [RelayCommand]
        void Clear()
        {
            CustomInfos.Clear();
        }
    }
}
