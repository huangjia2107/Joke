using Joke.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace Joke.Utils
{
    /// 参数说明：
    /// RequestCount：  请求的个数
    /// ResponseCount : 回应的个数 
    /// （注：回应个数与请求个数可能不同的原因在于 :总记录数不一定是“RequestCount”的整数倍，故最后一次请求，所回应的个数不一定等于请求个数）
    /// RealCount : 实际返回的个数 
    /// （注：由于个别原因，造成本来应该返回“ResponseCount”的数据，实际返回有可能不符）
    /// <typeparam name="T"></typeparam>
    public class IncrementalLoadingCollection<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        bool busy = false;
        bool IsNotifyNetworkStatusChanged = false;

        int responseCount = 0;
        uint pageIndex = 1;
        CancellationToken cts;

        //参数  
        uint requestCount = 50;
        Func<uint, uint, Task<JokeResponse<T>>> funcGetData;

        public IncrementalLoadingCollection(Func<uint, uint, Task<JokeResponse<T>>> _funcGetData, uint _requestCount = 20)
        {
            funcGetData = _funcGetData;
            requestCount = _requestCount;
        }

        //加载完成通知
        public delegate void LoadStatusChanged(LoadStatusArgs args);
        public event LoadStatusChanged OnLoadStatusChanged;

        public delegate void NetworkStatusChanged(bool IsDisconnected);
        public event NetworkStatusChanged OnNetworkStatusChanged;

        private bool _HasMoreItems = true;
        public bool HasMoreItems
        {
            get
            {
                if (cts.IsCancellationRequested)
                    return false;

                return _HasMoreItems;
            }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            if (busy)
            {
                throw new InvalidOperationException("Only one operation in flight at a time");
            }

            busy = true;

            return AsyncInfo.Run((_cts) => LoadMoreItemsAsync(_cts, count));
        }

        protected async Task<LoadMoreItemsResult> LoadMoreItemsAsync(CancellationToken _cts, uint count)
        {
            try
            {
                cts = _cts;

                // var pageIndex = (uint)loadCount / this.addCount + 1;
                System.Diagnostics.Debug.WriteLine(">> Srart to load, Page Index :" + pageIndex + ", Request Count：" + this.requestCount);

                if (!Algorithm.IsNetworkValid())
                {
                    if (this.OnNetworkStatusChanged != null && !IsNotifyNetworkStatusChanged)
                    {
                        IsNotifyNetworkStatusChanged = true;
                        this.OnNetworkStatusChanged(true);
                    }

                    return new LoadMoreItemsResult { Count = 0 };
                }
                else
                {
                    if (this.OnNetworkStatusChanged != null && IsNotifyNetworkStatusChanged)
                    {
                        IsNotifyNetworkStatusChanged = false;
                        this.OnNetworkStatusChanged(false);
                    }
                }

                var response = await funcGetData(pageIndex, this.requestCount);
                if (response == null)
                {
                    if (this.OnLoadStatusChanged != null)
                        this.OnLoadStatusChanged(new LoadStatusArgs { Status = LoadStatus.Empty });

                    return new LoadMoreItemsResult { Count = 0 };
                }

                pageIndex++;
                _HasMoreItems = (response.count != 0);

                if (_HasMoreItems)
                {
                    this.AddItems(response.items.ToList<T>());
                    responseCount += response.count;
                }

                System.Diagnostics.Debug.WriteLine("   End load, Response Count : " + response.count + ", Real Count : " + response.items.Length);

                if (!_HasMoreItems)
                {
                    System.Diagnostics.Debug.WriteLine("======== Response Total Count : " + responseCount + ", Real Total Count : " + this.Count + " ========");

                    if (this.OnLoadStatusChanged != null)
                        this.OnLoadStatusChanged(
                            new LoadStatusArgs
                            {
                                Status = LoadStatus.Finish,
                                ResponseTotalCount = (int)requestCount,
                                RealTotalCount = this.Count
                            });
                }

                return new LoadMoreItemsResult { Count = response.items == null ? 0 : (uint)response.items.Length };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error:" + ex.Message + ex.StackTrace);
                return new LoadMoreItemsResult { Count = 0 };
            }
            finally
            {
                busy = false;
            }
        }

        private void AddItems(List<T> items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    this.Add(item);
                }
            }
        }
        /*
        private void AddItems(List<T> items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    if (item is JokeInfo)
                    {
                        if (this.FirstOrDefault(joke =>
                        {
                            if (joke is JokeInfo)
                                return (joke as JokeInfo).id == (item as JokeInfo).id;
                            else
                                return false;
                        }) == null)
                            this.Add(item);
                    }

                }
            }
        } 
        */
    }
}
