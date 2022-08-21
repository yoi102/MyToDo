using MyToDo.Shared;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class HttpRestClient
    {
        private readonly string apiUrl;
        protected readonly RestClient client;

        public HttpRestClient(string apiUrl)
        {
            this.apiUrl = apiUrl;
            client = new RestClient(apiUrl);
        }


        public async Task<ApiResponse> ExecuteAsync(BaseRequest baseRequest)
        {

            var request = new RestRequest();

            request.Method = baseRequest.Method;
            request.AddHeader("Content-Type", baseRequest.ContentType);
            if (baseRequest.Parameter != null)
                request.AddBody(JsonConvert.SerializeObject(baseRequest.Parameter));

            //request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);

            client.Options.BaseUrl = new Uri(apiUrl + baseRequest.Route);
            //client.BuildUri(new Uri(apiUrl + baseRequest.Route));

            var response = await client.ExecuteAsync(request);


            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<ApiResponse>(response.Content);

            else
                return new ApiResponse()
                {
                    Status = false,
                    Message = response.ErrorMessage
                };
        }


        //public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
        //{

        //    var request = new RestRequest("method", baseRequest.Method);

        //    request.AddHeader("Content-Type", baseRequest.ContentType);
        //    if (baseRequest.Parameter != null)
        //        request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);

        //    client.Options.BaseUrl = new Uri(apiUrl + baseRequest.Route);
        //    //client.BuildUri(new Uri(apiUrl + baseRequest.Route));

        //    var response = await client.ExecuteAsync(request);

        //    return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
        //}




        public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
        {
            var request = new RestRequest();

            request.Method = baseRequest.Method;



            //这是完全没有必要的，而且通常是有害的。标Content - Type头是内容标头，而不是请求标头。
            //例如，当使用多部分形式的数据时，身体的各个部分可能会有所不同。RestSharp 根据您的正文格式自动设置正确的内容类型标题，
            //因此不要覆盖它。标Accept头由 RestSharp 根据注册的序列化程序自动设置。默认情况下，XML 和 JSON 都受支持。
            //仅在Accept需要其他内容（例如二进制流或纯文本）时才更改标头。
            //request.AddHeader("Content-Type", baseRequest.ContentType);//看说明，说是有害的。


            if (baseRequest.Parameter != null)
            {
                //新版的restsharp
                //如果你AddParameter(something, something, ParameterType.RequestBody)不起作用，
                //请尝试AddBody，因为它会尽力弄清楚你要添加什么样的身体。
                //request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
                //这几个都能用
                request.AddBody(JsonConvert.SerializeObject(baseRequest.Parameter));
                //request.AddJsonBody(JsonConvert.SerializeObject(baseRequest.Parameter));
                //request.AddJsonBody<object>(JsonConvert.SerializeObject(baseRequest.Parameter));

            }
            client.Options.BaseUrl = new Uri(apiUrl + baseRequest.Route);
            var response = await client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);

            else
                return new ApiResponse<T>()
                {
                    Status = false,
                    Message = response.ErrorMessage
                };
        }








    }
}
